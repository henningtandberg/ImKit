namespace ImKit;

/// <summary>
/// Per-frame entry point for the whole GUI. Drive these from the host application's
/// initialize / load / draw callbacks.
/// </summary>
public interface IGui
{
    void Initialize();
    void LoadContent();
    void Draw(FrameTime frameTime);
}
