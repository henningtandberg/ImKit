using Microsoft.Xna.Framework;

namespace ImKit.MonoGame;

public static class GameTimeExtensions
{
    /// <summary>
    /// Converts MonoGame's <see cref="GameTime"/> into the backend-agnostic
    /// <see cref="FrameTime"/> that <c>ImKit</c> widgets receive.
    /// </summary>
    public static FrameTime ToFrameTime(this GameTime gameTime) =>
        new(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
}
