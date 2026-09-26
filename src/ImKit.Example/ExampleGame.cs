using ImKit.Example.State;
using ImKit.MonoGame;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImKit.Example;

/// <summary>
/// Host window. The GUI is resolved after <c>base.Initialize()</c> because the
/// renderer needs a live <see cref="GraphicsDevice"/>, which only exists from that
/// point on.
/// </summary>
public class ExampleGame : Game
{
    private readonly IServiceProvider _services;
    private readonly AppState _state;

    private IGui? _gui;

    public ExampleGame(IServiceProvider services, AppState state)
    {
        _services = services;
        _state = state;

        _ = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720
        };

        Window.AllowUserResizing = true;
        Window.Title = "ImKit Example";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _gui = _services.GetRequiredService<IGui>();
        _gui.Initialize();
        _gui.LoadContent();

        _state.Log("Use File > Open Folder to pick a directory.");
    }

    protected override void Update(GameTime gameTime)
    {
        if (_state.ExitRequested)
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _gui?.Draw(gameTime.ToFrameTime());

        base.Draw(gameTime);
    }
}
