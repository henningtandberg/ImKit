namespace ImKit.MonoGame.Wrapper;

/// <summary>
/// Indirection over a MonoGame global that does not exist yet at container-build time
/// (<c>GraphicsDevice</c>, <c>GameWindow</c>, <c>ContentManager</c>). Registering the
/// wrapper instead of the value lets services be resolved before <c>Game.Initialize</c>
/// has run.
/// </summary>
public interface IWrapper<T>
{
    ref T Value { get; }
}
