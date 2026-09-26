using System.Numerics;
using ImGuiNET;
using ImKit.DockSpace;
using ImKit.Example.State;

namespace ImKit.Example.Widgets;

/// <summary>
/// Middle panel: one of (nearly) every scope wrapper in <see cref="ImGuiExtensions"/>,
/// so an unbalanced Begin/End shows up as an ImGui assert the moment the app runs.
/// </summary>
public class ScopesWidget(AppState state, IDockSpace dockSpace) : IGuiWidget
{
    private static readonly string[] Items = ["Alpha", "Beta", "Gamma"];

    private bool _checked;
    private int _radio;
    private int _combo;
    private int _listItem;
    private float _slider = 0.5f;
    private float _drag;
    private float _angle;
    private int _rangeMin;
    private int _rangeMax = 10;
    private string _text = "edit me";
    private Vector3 _color = new(0.4f, 0.7f, 1f);

    public bool ShouldDraw => state.WorkingDirectory is not null && dockSpace.MiddlePanelId != 0;

    public void Draw(FrameTime frameTime)
    {
        ImGui.SetNextWindowDockID(dockSpace.MiddlePanelId, ImGuiCond.Once);
        ImGui.Begin("Scopes", () => ImGui.BeginTabBar("ScopeTabs", () =>
        {
            ImGui.BeginTabItem("Input", DrawInput);
            ImGui.BeginTabItem("Layout", DrawLayout);
        }));
    }

    private void DrawInput()
    {
        ImGui.Checkbox("Checkbox", ref _checked, v => state.Log($"Checkbox -> {v}"));

        ImGui.RadioButton("First", ref _radio, 0, v => state.Log($"Radio -> {v}"));
        ImGui.SameLine();
        ImGui.RadioButton("Second", ref _radio, 1, v => state.Log($"Radio -> {v}"));

        ImGui.Combo("Combo", ref _combo, Items, Items.Length, v => state.Log($"Combo -> {Items[v]}"));
        ImGui.ListBox("List", ref _listItem, Items, Items.Length, v => state.Log($"List -> {Items[v]}"));

        ImGui.SliderFloat("Slider", ref _slider, 0f, 1f, _ => { });
        ImGui.DragFloat("Drag", ref _drag, _ => { });
        ImGui.SliderAngle("Angle", ref _angle, _ => { });
        ImGui.DragIntRange2("Range", ref _rangeMin, ref _rangeMax, (lo, hi) => state.Log($"Range {lo}..{hi}"));

        ImGui.InputText("Text", ref _text, 64, v => state.Log($"Text -> {v}"));
        ImGui.ColorEdit3("Color", ref _color, _ => { });

        ImGui.BeginDisabled(!_checked, () => ImGui.Button("Enabled by the checkbox", () => state.Log("Clicked")));
    }

    private void DrawLayout()
    {
        ImGui.CollapsingHeader("Tree", () =>
        {
            ImGui.TreeNode("Branch", () =>
            {
                ImGui.Indent(() => ImGui.TextUnformatted("Leaf"));
                ImGui.Selectable("Selectable leaf", () => state.Log("Leaf clicked"));
            });
        });

        ImGui.PushStyleColor(ImGuiCol.Text, ColorTheme.Yellow, () => ImGui.TextUnformatted("Styled text"));

        ImGui.BeginGroup(() =>
        {
            ImGui.SmallButton("Small", () => state.Log("Small clicked"));
            ImGui.SameLine();
            ImGui.ArrowButton("arrow", ImGuiDir.Right, () => state.Log("Arrow clicked"));
        });

        ImGui.BeginItemTooltip(() => ImGui.TextUnformatted("Tooltip on the group"));

        ImGui.BeginTable("Table", 2, ImGuiTableFlags.Borders, () =>
        {
            foreach (var item in Items)
            {
                ImGui.TableNextRow();
                ImGui.TableNextColumn(() => ImGui.TextUnformatted(item));
                ImGui.TableNextColumn(() => ImGui.TextUnformatted($"{item.Length} chars"));
            }
        });

        ImGui.BeginChild("Scroller", new Vector2(0, 80), ImGuiChildFlags.FrameStyle, () =>
        {
            for (var i = 0; i < 20; i++)
            {
                ImGui.PushID(i, () => ImGui.TextUnformatted($"row {i}"));
            }
        });

        ImGui.Button("Open popup", () => ImGui.OpenPopup("ScopesPopup"));
        ImGui.BeginPopup("ScopesPopup", () => ImGui.MenuItem("Close", () => ImGui.CloseCurrentPopup()));
    }
}
