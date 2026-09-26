using System.Numerics;
using ImGuiNET;
using ImKit.DockSpace;
using ImKit.Example.State;

namespace ImKit.Example.Widgets;

/// <summary>
/// Right panel: details for the selected file, and a demo of the scope-based
/// <c>SeparatorText</c> / <c>Button</c> extension members.
/// </summary>
public class InspectorWidget(AppState state, IDockSpace dockSpace) : IGuiWidget
{
    private Vector3 _position = Vector3.Zero;

    public bool ShouldDraw => state.WorkingDirectory is not null && dockSpace.RightPanelId != 0;

    public void Draw(FrameTime frameTime)
    {
        ImGui.SetNextWindowDockID(dockSpace.RightPanelId, ImGuiCond.Once);
        ImGui.Begin("Inspector", () =>
        {
            ImGui.SeparatorText("Selection", () =>
            {
                if (state.SelectedPath is null)
                {
                    ImGui.TextDisabled("Double-click a file in the tree.");
                    return;
                }

                ImGui.TextWrapped(state.SelectedPath);
                ImGui.Text($"Size: {FileSize(state.SelectedPath)}");
            });

            ImGui.SeparatorText("Transform", () =>
            {
                ImGui.DragFloat3("Position", ref _position, 0.1f);
                ImGui.Button("Reset", onClick: () =>
                {
                    _position = Vector3.Zero;
                    state.Log("Transform reset");
                });
            });
        });
    }

    private static string FileSize(string path)
    {
        var info = new FileInfo(path);
        return info.Exists ? $"{info.Length} bytes" : "unavailable";
    }
}
