using System;

namespace ImKit.Components;

/// <summary>
/// A reusable piece of UI that owns state across frames — an open document, a bound
/// texture, an expansion set — and therefore needs deterministic teardown.
/// </summary>
public interface IStatefulComponent : IDisposable
{
    string Title { get; }

    void Draw();
}
