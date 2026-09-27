# ImKit

Building blocks for Dear ImGui tools and editors in .NET, on top of
[ImGui.NET](https://github.com/ImGuiNET/ImGui.NET).

ImGui.NET gives you the raw immediate-mode API. ImKit adds the structure you end up
writing anyway when the tool grows past one window: scope-safe `Begin`/`End` wrappers, a
widget host, a dock-space layout, and reusable file-system components — none of which
depend on a graphics framework.

| Package | What it is |
| --- | --- |
| [`ImKit`](https://www.nuget.org/packages/ImKit) | Backend-agnostic core. Depends only on `ImGui.NET`. |
| [`ImKit.MonoGame`](https://www.nuget.org/packages/ImKit.MonoGame) | MonoGame/FNA renderer, texture binding, DI wiring. |

Both target `net10.0` and are MIT licensed.

## Install

```sh
dotnet add package ImKit            # core only — bring your own renderer
dotnet add package ImKit.MonoGame   # core + MonoGame backend
```

## Features

### Scope-based ImGui extensions

Dear ImGui's `Begin`/`End` pairs must stay balanced, and an early `return` in the middle
of a window silently corrupts the stack. `ImGuiExtensions` adds extension members on
`ImGui` that take the body as a callback and always emit the matching `End`:

```csharp
using ImKit;      // brings the extension members on ImGui into scope
using ImGuiNET;

ImGui.Begin("Inspector", () =>
{
    ImGui.SeparatorText("Transform", () =>
    {
        ImGui.Button("Reset", onClick: ResetTransform);
    });

    ImGui.BeginChild("Children", new Vector2(0, -30), ImGuiChildFlags.FrameStyle, () =>
    {
        foreach (var child in children)
            ImGui.Selectable(child.Name, selected: child == _selected, () => _selected = child);
    });
});
```

Covered: `Begin`, `BeginChild`, `BeginPopupModal`, `BeginMenuBar`, `BeginMainMenuBar`,
`BeginMenu`, `MenuItem`, `Button`, `Selectable`, `SeparatorText`.

### Widget host

Implement `IGuiWidget` per panel; `GuiHost` opens the frame, draws the dock space, draws
every widget whose `ShouldDraw` is true, then closes the frame.

```csharp
public class InspectorWidget : IGuiWidget
{
    public bool ShouldDraw => true;

    public void Draw(FrameTime frameTime) => ImGui.Begin("Inspector", () => { /* ... */ });
}

IGui gui = new GuiHost(backend, widgets, dockSpace);   // dockSpace optional

gui.Initialize();                 // once the graphics device exists
gui.Draw(new FrameTime(total, elapsed));   // each frame
```

`ShouldDraw` is the place to gate a widget on application state — no project loaded,
panel closed — instead of early-returning inside `Draw`.

`FrameTime` is a `readonly record struct` of `Total` / `Elapsed`, so widgets never take a
dependency on a specific framework's time type.

### Dock space

`DockSpaceBase` builds a full-viewport dock space split into left / middle / right /
bottom panels. Widgets dock themselves into a panel by id:

```csharp
public class EditorDockSpace : DockSpaceBase
{
    public bool ProjectLoaded { get; set; }

    // Nothing docks until a project is open; the viewport background still fills.
    protected override bool ShouldDraw => ProjectLoaded;

    protected override float LeftPanelRatio => 0.25f;
}

// in a widget
if (dockSpace.LeftPanelId == 0)
    return;   // layout not built yet

ImGui.SetNextWindowDockID(dockSpace.LeftPanelId, ImGuiCond.Once);
ImGui.Begin("Files", () => _fileTree.Draw());
```

Overridable: `ShouldDraw`, `BackgroundColor`, `LeftPanelRatio`, `RightPanelRatio`,
`BottomPanelRatio`, and `BuildLayout` for a different arrangement. `ResetLayout()`
rebuilds on the next frame.

Dear ImGui's DockBuilder API is not exposed by ImGui.NET, so `ImGuiDockBuilder` provides
P/Invoke bindings against the `cimgui` native library ImGui.NET already ships. Docking
must be enabled on the context:

```csharp
ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
```

(`ImKit.MonoGame`'s renderer does this for you.)

### File-system components

`FileTree` — expandable directory tree, read fresh every frame so external changes show
up immediately. `FilePicker` — modal file/folder picker. `PathSelection` — the selectable
rows both are built from. All callbacks receive the entry's **full path**.

```csharp
var tree = new FileTree(
    title: "Assets",
    workingDirectory: projectPath,
    skippedExtensions: [".meta", ".tmp"],
    onOpen: path => OpenAsset(path),
    onRightClick: path => ShowContextMenu(path));

tree.Draw();
```

```csharp
var picker = new FilePicker(
    title: "Open Folder",
    startingDirectory: Environment.CurrentDirectory,
    skippedExtensions: [],
    onOpen: path => OpenFolder(path),
    onCancel: () => _picking = false);
```

Links, device files, and dot-files are filtered out. Unreadable directories render as
empty rather than throwing mid-window.

`FilePicker.Draw()` must be called every frame while it should be open; it invokes
`onOpen` or `onCancel` exactly once and then closes itself, so stop calling `Draw` from
inside that callback.

## MonoGame backend

```csharp
services.AddImKit(
    graphicsDeviceFactory: sp => sp.GetRequiredService<MyGame>().GraphicsDevice,
    gameWindowFactory:     sp => sp.GetRequiredService<MyGame>().Window);

services.AddDockSpace<EditorDockSpace>();
services.AddGuiWidget<MainMenuWidget>();
services.AddGuiWidget<InspectorWidget>();
```

The graphics device and window are passed as factories because neither exists until
`Game.Initialize` has run — usually after the container is built. They resolve lazily, on
first use of the renderer.

`AddGuiWidget<T>` registers one instance under both `T` and `IGuiWidget`, so a widget can
also be injected by its concrete type (or resolved as an event subscriber) without a
second instance being constructed.

Then drive it from `Game`:

```csharp
protected override void Initialize()
{
    base.Initialize();
    _gui = _services.GetRequiredService<IGui>();
    _gui.Initialize();          // uploads the font atlas
    _gui.LoadContent();
}

protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.Black);
    _gui.Draw(gameTime.ToFrameTime());
}
```

`ToFrameTime()` converts MonoGame's `GameTime` to `FrameTime`.

Textures:

```csharp
IntPtr id = renderer.BindTexture(texture);
ImGui.Image(id, new Vector2(texture.Width, texture.Height));
// ...
renderer.UnbindTexture(id);   // does not dispose the texture
```

Every bound handle must be released, or the renderer keeps the texture alive. The
`ImGuiRenderer` constructor creates the ImGui context and enables docking — construct
exactly one per application.

## Other backends

`IGuiBackend` is the seam a renderer implements:

```csharp
public interface IGuiBackend
{
    void RebuildFontAtlas();              // upload font atlas, before the first frame
    void BeforeLayout(FrameTime frame);   // push input + timing, open the ImGui frame
    void AfterLayout();                   // close the frame, render draw data
}
```

Implement it against Veldrid, Silk.NET, or anything else, and every widget, dock space,
and component above works unchanged.

## Example app

`src/ImKit.Example` is a runnable MonoGame editor shell: four-panel dock layout, main
menu, folder picker, file tree, inspector, log panel, and the ImGui demo window.

```sh
dotnet run --project src/ImKit.Example            # start empty, use File > Open Folder
dotnet run --project src/ImKit.Example -- ~/code  # start with a folder open
```

## Building

```sh
dotnet build ImKit.slnx
dotnet test ImKit.slnx
```

Requires the .NET 10 SDK (pinned in `global.json`).

## License

MIT — see [LICENSE](LICENSE).
