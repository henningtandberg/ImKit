namespace ImKit;

/// <summary>
/// A top-level unit of UI drawn once per frame by <see cref="GuiHost"/>.
/// </summary>
public interface IGuiWidget
{
    /// <summary>
    /// Whether this widget participates in the current frame. Widgets that are gated
    /// on application state (no project loaded, panel closed) return false here rather
    /// than early-returning inside <see cref="Draw"/>.
    /// </summary>
    bool ShouldDraw { get; }

    void Draw(FrameTime frameTime);
}
