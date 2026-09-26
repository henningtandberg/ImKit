using ImGuiNET;
using ImKit.Components.FileSystem;
using ImKit.DockSpace;
using ImKit.Example.State;

namespace ImKit.Example.Widgets;

/// <summary>
/// Left panel: the folder the user opened, as an expandable tree. The tree is rebuilt
/// whenever the working directory changes, since its root is fixed at construction.
/// </summary>
public class FileTreeWidget(AppState state, IDockSpace dockSpace) : IGuiWidget
{
    private static readonly string[] SkippedExtensions = [".tmp", ".user"];

    private string? _treeRoot;
    private FileTree? _tree;

    // Panel ids are zero until the dock space has built its layout on the first drawn
    // frame; drawing before that would leave the window undocked for good.
    public bool ShouldDraw => state.WorkingDirectory is not null && dockSpace.LeftPanelId != 0;

    public void Draw(FrameTime frameTime)
    {
        EnsureTree();

        ImGui.SetNextWindowDockID(dockSpace.LeftPanelId, ImGuiCond.Once);
        ImGui.Begin("Files", () => _tree?.Draw());
    }

    private void EnsureTree()
    {
        if (_treeRoot == state.WorkingDirectory)
        {
            return;
        }

        _treeRoot = state.WorkingDirectory;
        _tree = new FileTree(
            title: "FileTree",
            workingDirectory: _treeRoot!,
            skippedExtensions: SkippedExtensions,
            onOpen: state.Select,
            onRightClick: path => state.Log($"Right-clicked {path}"));
    }
}
