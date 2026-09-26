using System.Collections.Generic;
using ImKit.DockSpace;

namespace ImKit;

/// <summary>
/// Default <see cref="IGui"/> implementation: opens the ImGui frame, draws the optional
/// dock space, then every widget that wants to draw, then closes the frame.
/// </summary>
/// <param name="backend">Renderer that owns the ImGui context.</param>
/// <param name="widgets">
/// Widgets to draw each frame, in registration order. With a DI container this is
/// typically every registered <see cref="IGuiWidget"/>.
/// </param>
/// <param name="dockSpace">
/// Optional dock space drawn before the widgets. Pass <c>null</c> for a GUI made of
/// free-floating windows.
/// </param>
public class GuiHost(
    IGuiBackend backend,
    IEnumerable<IGuiWidget> widgets,
    IDockSpace? dockSpace = null)
    : IGui
{
    public virtual void Initialize()
    {
        backend.RebuildFontAtlas();
        dockSpace?.Initialize();
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Draw(FrameTime frameTime)
    {
        backend.BeforeLayout(frameTime);

        dockSpace?.Draw(frameTime);

        foreach (var widget in widgets)
        {
            if (widget.ShouldDraw)
            {
                widget.Draw(frameTime);
            }
        }

        backend.AfterLayout();
    }
}
