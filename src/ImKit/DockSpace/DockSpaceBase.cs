using System.Numerics;
using ImGuiNET;

namespace ImKit.DockSpace;

/// <summary>
/// A four-panel dock space (left / middle / right / bottom) filling the main viewport.
///
/// Derive to gate visibility on application state by overriding <see cref="ShouldDraw"/>,
/// or to change the arrangement by overriding <see cref="BuildLayout"/>.
/// </summary>
public abstract class DockSpaceBase : IDockSpace
{
    private const ImGuiWindowFlags DockSpaceWindowFlags =
        ImGuiWindowFlags.NoTitleBar |
        ImGuiWindowFlags.NoCollapse |
        ImGuiWindowFlags.NoResize |
        ImGuiWindowFlags.NoMove |
        ImGuiWindowFlags.NoBringToFrontOnFocus |
        ImGuiWindowFlags.NoNavFocus |
        ImGuiWindowFlags.NoBackground;

    private readonly string _dockSpaceId;
    private readonly string _windowName;

    private bool _layoutInitialized;

    protected DockSpaceBase(string dockSpaceId = "MainDockSpace")
    {
        _dockSpaceId = dockSpaceId;
        _windowName = dockSpaceId + "Window";
    }

    public uint MainDockSpaceId { get; private set; }
    public uint LeftPanelId { get; private set; }
    public uint MiddlePanelId { get; private set; }
    public uint RightPanelId { get; private set; }
    public uint BottomPanelId { get; private set; }

    /// <summary>
    /// Whether the dock space is drawn this frame. The viewport background is still
    /// filled when this is false, so the host does not flash undefined pixels.
    /// </summary>
    protected virtual bool ShouldDraw => true;

    /// <summary>Colour filled behind everything, before the dock space.</summary>
    protected virtual Vector4 BackgroundColor => new(0.1f, 0.1f, 0.1f, 1.0f);

    protected virtual float BottomPanelRatio => 0.25f;
    protected virtual float LeftPanelRatio => 0.20f;
    protected virtual float RightPanelRatio => 0.25f;

    public virtual void Initialize()
    {
    }

    public void Draw(FrameTime frameTime)
    {
        DrawBackground();

        if (!ShouldDraw)
            return;

        PrepareDockSpaceWindow();

        ImGui.Begin(_windowName, DockSpaceWindowFlags, () =>
        {
            // Balances the three PushStyleVar calls in PrepareDockSpaceWindow. Done inside
            // the window so the styles apply to the window itself but not to its contents.
            ImGui.PopStyleVar(3);

            MainDockSpaceId = ImGui.GetID(_dockSpaceId);
            ImGui.DockSpace(MainDockSpaceId, Vector2.Zero, ImGuiDockNodeFlags.None);

            if (_layoutInitialized)
            {
                return;
            }

            _layoutInitialized = true;
            BuildLayout(ImGui.GetMainViewport().WorkSize);
        });
    }

    /// <summary>
    /// Forces the layout to be rebuilt on the next drawn frame, discarding any docking
    /// the user has done since.
    /// </summary>
    protected void ResetLayout() => _layoutInitialized = false;

    /// <summary>
    /// Splits <see cref="MainDockSpaceId"/> into the four panels and assigns the panel ids.
    /// Called once on the first drawn frame.
    /// </summary>
    protected virtual void BuildLayout(Vector2 workSize)
    {
        ImGuiDockBuilder.RemoveNode(MainDockSpaceId);
        ImGuiDockBuilder.AddNode(MainDockSpaceId);
        ImGuiDockBuilder.SetNodeSize(MainDockSpaceId, workSize);

        ImGuiDockBuilder.SplitNode(MainDockSpaceId, ImGuiDir.Down, BottomPanelRatio, out var bottomId, out var topId);
        ImGuiDockBuilder.SplitNode(topId, ImGuiDir.Left, LeftPanelRatio, out var leftId, out var topRemainder);
        ImGuiDockBuilder.SplitNode(topRemainder, ImGuiDir.Right, RightPanelRatio, out var rightId, out var middleId);

        ImGuiDockBuilder.Finish(MainDockSpaceId);

        LeftPanelId = leftId;
        MiddlePanelId = middleId;
        RightPanelId = rightId;
        BottomPanelId = bottomId;
    }

    private void DrawBackground()
    {
        var viewport = ImGui.GetMainViewport();
        var drawList = ImGui.GetBackgroundDrawList();
        drawList.AddRectFilled(viewport.Pos, viewport.Pos + viewport.Size,
            ImGui.ColorConvertFloat4ToU32(BackgroundColor));
    }

    private static void PrepareDockSpaceWindow()
    {
        var viewport = ImGui.GetMainViewport();

        ImGui.SetNextWindowPos(viewport.WorkPos);
        ImGui.SetNextWindowSize(viewport.WorkSize);
        ImGui.SetNextWindowViewport(viewport.ID);

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
    }
}
