using System;
using Microsoft.Xna.Framework.Graphics;

namespace ImKit.MonoGame.Rendering;

/// <summary>
/// MonoGame <see cref="IGuiBackend"/>, extended with texture binding so components can
/// draw <see cref="Texture2D"/> through <c>ImGui.Image</c>.
/// </summary>
public interface IImGuiRenderer : IGuiBackend
{
    /// <summary>
    /// Registers a texture and returns the handle to pass to <c>ImGui.Image</c>.
    /// Every bound texture must be released with <see cref="UnbindTexture"/>, otherwise
    /// the renderer keeps it alive.
    /// </summary>
    IntPtr BindTexture(Texture2D texture);

    /// <summary>
    /// Releases a handle from <see cref="BindTexture"/>. Does not dispose the texture.
    /// </summary>
    void UnbindTexture(IntPtr textureId);
}
