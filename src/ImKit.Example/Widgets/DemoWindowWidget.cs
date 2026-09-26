using ImGuiNET;
using ImKit.DockSpace;
using ImKit.Example.State;

namespace ImKit.Example.Widgets;

/// <summary>
/// Wraps Dear ImGui's built-in demo window as an <see cref="IGuiWidget"/>, docked into
/// the right panel.
/// </summary>
/// <remarks>
/// <c>ImGui.ShowDemoWindow</c> opens its own window, so it can't be wrapped in
/// <c>ImGui.Begin(...)</c>. Docking works the same way regardless:
/// <c>SetNextWindowDockID</c> applies to the next window opened, which is the demo's.
/// </remarks>
public class DemoWindowWidget(AppState state, IDockSpace dockSpace) : IGuiWidget
{
    private bool _isOpen;

    public bool IsOpen => _isOpen;

    public bool ShouldDraw => _isOpen;

    public void Draw(FrameTime frameTime)
    {
        // Zero means the layout hasn't been built yet (no folder open). The demo then
        // floats instead of docking, rather than being suppressed entirely.
        if (dockSpace.RightPanelId != 0)
        {
            ImGui.SetNextWindowDockID(dockSpace.RightPanelId, ImGuiCond.Once);
        }

        ImGui.ShowDemoWindow(ref _isOpen);

        // The demo's own title-bar close button clears the flag, so the menu check mark
        // stays in sync without the menu polling anything.
        if (!_isOpen)
        {
            state.Log("Demo window closed");
        }
    }

    public void Toggle()
    {
        _isOpen = !_isOpen;
        state.Log(_isOpen ? "Demo window opened" : "Demo window closed");
    }
}
