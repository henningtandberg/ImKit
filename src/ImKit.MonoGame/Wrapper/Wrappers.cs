using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ImKit.MonoGame.Wrapper;

public class GameWindowWrapper(GameWindow gameWindow) : IWrapper<GameWindow>
{
    private GameWindow _gameWindow = gameWindow;

    public ref GameWindow Value => ref _gameWindow;
}

public class GraphicsDeviceWrapper(GraphicsDevice graphicsDevice) : IWrapper<GraphicsDevice>
{
    private GraphicsDevice _graphicsDevice = graphicsDevice;

    public ref GraphicsDevice Value => ref _graphicsDevice;
}

public class ContentManagerWrapper(ContentManager contentManager) : IWrapper<ContentManager>
{
    private ContentManager _contentManager = contentManager;

    public ref ContentManager Value => ref _contentManager;
}
