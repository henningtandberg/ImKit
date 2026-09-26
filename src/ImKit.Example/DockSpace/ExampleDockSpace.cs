using ImKit.DockSpace;
using ImKit.Example.State;

namespace ImKit.Example.DockSpace;

/// <summary>
/// Four-panel layout: file tree left, inspector right, log bottom, viewport middle.
/// Nothing docks until a folder is open, so the empty state is just the background.
/// </summary>
public class ExampleDockSpace(AppState state) : DockSpaceBase
{
    protected override bool ShouldDraw => state.WorkingDirectory is not null;

    protected override float LeftPanelRatio => 0.25f;

    protected override float RightPanelRatio => 0.25f;

    protected override float BottomPanelRatio => 0.25f;
}
