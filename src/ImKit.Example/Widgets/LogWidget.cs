using ImGuiNET;
using ImKit.DockSpace;
using ImKit.Example.State;

namespace ImKit.Example.Widgets;

/// <summary>
/// Bottom panel: whatever the other widgets logged, newest last.
/// </summary>
public class LogWidget(AppState state, IDockSpace dockSpace) : IGuiWidget
{
    public bool ShouldDraw => state.WorkingDirectory is not null && dockSpace.BottomPanelId != 0;

    public void Draw(FrameTime frameTime)
    {
        ImGui.SetNextWindowDockID(dockSpace.BottomPanelId, ImGuiCond.Once);
        ImGui.Begin("Log", () =>
        {
            foreach (var line in state.LogLines)
            {
                ImGui.TextUnformatted(line);
            }
        });
    }
}
