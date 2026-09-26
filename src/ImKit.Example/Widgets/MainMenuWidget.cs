using ImGuiNET;
using ImKit.Components.FileSystem;
using ImKit.Example.State;

namespace ImKit.Example.Widgets;

/// <summary>
/// Main menu bar plus the modal folder picker it opens. The picker closes itself after
/// invoking a callback, so this widget drops its reference from inside those callbacks.
/// </summary>
/// <remarks>
/// <paramref name="demoWindow"/> is injected by its concrete type — <c>AddGuiWidget</c>
/// registers a widget under both <see cref="IGuiWidget"/> and itself, so this is the
/// same instance the host draws, not a second one.
/// </remarks>
public class MainMenuWidget(AppState state, DemoWindowWidget demoWindow) : IGuiWidget
{
    private FilePicker? _picker;

    public bool ShouldDraw => true;

    public void Draw(FrameTime frameTime)
    {
        ImGui.BeginMainMenuBar(() =>
        {
            ImGui.BeginMenu("File", () =>
            {
                ImGui.MenuItem("Open Folder...", "Ctrl+O", OpenPicker);
                ImGui.Separator();
                ImGui.MenuItem("Exit", "Alt+F4", state.RequestExit);
            });

            ImGui.BeginMenu("View", () =>
                ImGui.MenuItem($"{(demoWindow.IsOpen ? "*" : " ")} ImGui Demo Window", demoWindow.Toggle));

            ImGui.BeginMenu("Help", () => ImGui.MenuItem("Clear Log", () => state.Log("--- cleared ---")));

            ImGui.Text($"   {1f / MathF.Max(frameTime.DeltaSeconds, 0.0001f):F0} fps");
        });

        _picker?.Draw();
    }

    private void OpenPicker() => _picker = new FilePicker(
        title: "Open Folder",
        startingDirectory: Directory.GetCurrentDirectory(),
        skippedExtensions: [],
        onOpen: path =>
        {
            state.OpenFolder(path);
            _picker = null;
        },
        onCancel: () => _picker = null);
}
