namespace ImKit.DockSpace;

/// <summary>
/// A full-viewport dock space with named panels that widgets can dock themselves into
/// via <c>ImGui.SetNextWindowDockID</c>.
/// </summary>
/// <remarks>
/// Panel ids are zero until the layout has been built on the first drawn frame, so
/// widgets must treat zero as "not ready yet" and skip drawing.
/// </remarks>
public interface IDockSpace
{
    uint MainDockSpaceId { get; }
    uint LeftPanelId { get; }
    uint MiddlePanelId { get; }
    uint RightPanelId { get; }
    uint BottomPanelId { get; }

    void Initialize();
    void Draw(FrameTime frameTime);
}
