namespace ImKit.Example.State;

/// <summary>
/// Fixed-size ring of log lines. The oldest line is dropped once <c>capacity</c> is
/// reached, so a long-running session can't grow the log without bound.
/// </summary>
public class LogBuffer(int capacity)
{
    private readonly Queue<string> _lines = new();

    public IReadOnlyList<string> Lines => _lines.ToArray();

    public void Add(string line)
    {
        _lines.Enqueue(line);

        while (_lines.Count > capacity)
        {
            _lines.Dequeue();
        }
    }
}
