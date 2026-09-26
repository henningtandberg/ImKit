using ImKit.Example;
using ImKit.Example.DockSpace;
using ImKit.Example.State;
using ImKit.Example.Widgets;
using ImKit.MonoGame;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<AppState>();
services.AddSingleton<ExampleGame>();

// The device and window are handed over as factories: neither exists until
// Game.Initialize has run, which is after this container is built.
services.AddImKit(
    graphicsDeviceFactory: sp => sp.GetRequiredService<ExampleGame>().GraphicsDevice,
    gameWindowFactory: sp => sp.GetRequiredService<ExampleGame>().Window);

services.AddDockSpace<ExampleDockSpace>();
services.AddGuiWidget<DemoWindowWidget>();
services.AddGuiWidget<MainMenuWidget>();
services.AddGuiWidget<FileTreeWidget>();
services.AddGuiWidget<InspectorWidget>();
services.AddGuiWidget<LogWidget>();

using var provider = services.BuildServiceProvider();
using var game = provider.GetRequiredService<ExampleGame>();

game.Run();
