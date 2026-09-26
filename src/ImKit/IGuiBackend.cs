namespace ImKit;

/// <summary>
/// The rendering backend that owns the ImGui context and turns ImGui draw data into
/// pixels. Deliberately free of any graphics-framework types so <see cref="GuiHost"/>
/// and every widget stay portable; backend packages (for example
/// <c>ImKit.MonoGame</c>) extend this with their own texture-binding surface.
/// </summary>
public interface IGuiBackend
{
    /// <summary>
    /// Uploads the ImGui font atlas to the GPU. Call once the graphics device exists
    /// but before the first frame is drawn.
    /// </summary>
    void RebuildFontAtlas();

    /// <summary>
    /// Pushes input state and frame timing into ImGui and opens a new frame.
    /// </summary>
    void BeforeLayout(FrameTime frameTime);

    /// <summary>
    /// Closes the frame and renders the resulting draw data.
    /// </summary>
    void AfterLayout();
}
