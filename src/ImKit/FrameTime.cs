using System;

namespace ImKit;

/// <summary>
/// Backend-agnostic frame timing. Replaces a direct dependency on a specific
/// game framework's time type (for example MonoGame's <c>GameTime</c>) so that
/// widgets and dock spaces stay portable across rendering backends.
/// </summary>
/// <param name="Total">Time elapsed since the application started.</param>
/// <param name="Elapsed">Time elapsed since the previous frame.</param>
public readonly record struct FrameTime(TimeSpan Total, TimeSpan Elapsed)
{
    /// <summary>
    /// Time elapsed since the previous frame, in seconds. This is the value
    /// Dear ImGui expects in <c>ImGuiIO.DeltaTime</c>.
    /// </summary>
    public float DeltaSeconds => (float)Elapsed.TotalSeconds;
}
