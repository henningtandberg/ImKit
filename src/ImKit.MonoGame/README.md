# ImKit.MonoGame

MonoGame backend for [`ImKit`](https://www.nuget.org/packages/ImKit): a Dear ImGui
renderer for MonoGame/FNA, texture binding, and dependency-injection wiring.

## Install

```
dotnet add package ImKit.MonoGame
```

Targets `net10.0`. Brings in `ImKit`, `ImGui.NET` and
`MonoGame.Framework.DesktopGL`.

## Wiring it up

```csharp
services.AddImKit(
    graphicsDeviceFactory: sp => sp.GetRequiredService<MyGame>().GraphicsDevice,
    gameWindowFactory:     sp => sp.GetRequiredService<MyGame>().Window);

services.AddDockSpace<EditorDockSpace>();
services.AddGuiWidget<MainMenuWidget>();
services.AddGuiWidget<InspectorWidget>();
```

The graphics device and window are passed as factories because neither exists until
`Game.Initialize` has run — usually after the container is built. They are resolved
lazily, on first use of the renderer.

`AddGuiWidget<T>` registers one instance under both `T` and `IGuiWidget`, so a widget can
also be injected by its concrete type (or resolved as an event subscriber) without a
second instance being constructed.

Then drive it from `Game`:

```csharp
protected override void Initialize()
{
    base.Initialize();
    _gui.Initialize();          // uploads the font atlas
}

protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.Black);
    _gui.Draw(gameTime.ToFrameTime());
}
```

`ToFrameTime()` converts MonoGame's `GameTime` to `ImKit`'s backend-agnostic
`FrameTime`.

## Renderer

`ImGuiRenderer` implements `IImGuiRenderer`, which is `ImKit`'s `IGuiBackend` plus
texture binding:

```csharp
IntPtr id = renderer.BindTexture(texture);
ImGui.Image(id, new Vector2(texture.Width, texture.Height));
// ...
renderer.UnbindTexture(id);   // does not dispose the texture
```

Every bound handle must be released, otherwise the renderer keeps the texture alive.

The constructor creates the ImGui context and enables docking, so construct exactly one
per application. Manual construction without a container:

```csharp
var renderer = new ImGuiRenderer(
    new GraphicsDeviceWrapper(GraphicsDevice),
    new GameWindowWrapper(Window));
```

## License

MIT
