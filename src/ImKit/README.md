# ImKit

Backend-agnostic building blocks for building Dear ImGui tools and editors in .NET, on
top of [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET).

`ImKit` has no dependency on any graphics framework. For a ready-made MonoGame
renderer and DI wiring, add [`ImKit.MonoGame`](https://www.nuget.org/packages/ImKit.MonoGame).

## Install

```
dotnet add package ImKit
```

Targets `net10.0`.

## Scope-based ImGui extensions

Dear ImGui's `Begin`/`End` pairs have to stay balanced, and an early `return` in the
middle of a window silently corrupts the stack. `ImGuiExtensions` adds extension members
on `ImGui` that take the body as a callback and always emit the matching `End`:

```csharp
using ImKit;   // brings the extension members on ImGui into scope
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

## Widget host

Implement `IGuiWidget` per panel and let `GuiHost` drive them:

```csharp
public class InspectorWidget : IGuiWidget
{
    public bool ShouldDraw => true;

    public void Draw(FrameTime frameTime) => ImGui.Begin("Inspector", () => { /* ... */ });
}

IGui gui = new GuiHost(backend, widgets, dockSpace);

// once the graphics device exists
gui.Initialize();

// each frame
gui.Draw(new FrameTime(totalElapsed, sinceLastFrame));
```

`FrameTime` exists so widgets never take a dependency on a specific framework's time
type. `dockSpace` is optional — pass `null` for free-floating windows.

`IGuiBackend` is the seam a renderer implements (`RebuildFontAtlas`, `BeforeLayout`,
`AfterLayout`). Use `ImKit.MonoGame` or write your own.

## Dock space

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

Override `BuildLayout` for a different arrangement, or `ResetLayout()` to rebuild it.

Dear ImGui's DockBuilder API is not exposed by ImGui.NET, so `ImGuiDockBuilder` provides
P/Invoke bindings against the `cimgui` native library ImGui.NET already ships. Docking
must be enabled on the ImGui context:

```csharp
ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
```

## File-system components

`FileTree` — expandable directory tree. `FilePicker` — modal file/folder picker.
`PathSelection` — the selectable rows both are built from. All callbacks receive the
entry's **full path**.

```csharp
var tree = new FileTree(
    title: "Assets",
    workingDirectory: projectPath,
    skippedExtensions: [".meta", ".tmp"],
    onOpen: path => OpenAsset(path),
    onRightClick: path => ShowContextMenu(path));

tree.Draw();
```

Links, device files, and dot-files are filtered out. Unreadable directories render as
empty rather than throwing mid-window.

## License

MIT
