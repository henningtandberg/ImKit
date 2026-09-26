namespace ImKit.Example.State;

/// <summary>
/// Everything the widgets share. Deliberately free of ImGui types: the widgets read and
/// mutate this, and it can be exercised in a unit test without a graphics device.
/// </summary>
public class AppState
{
    private readonly LogBuffer _log = new(capacity: 200);

    public string? WorkingDirectory { get; private set; }

    public string? SelectedPath { get; private set; }

    public bool ExitRequested { get; private set; }

    public IReadOnlyList<string> LogLines => _log.Lines;

    public void OpenFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Log($"Not a folder: {path}");
            return;
        }

        WorkingDirectory = path;
        SelectedPath = null;
        Log($"Opened folder {path}");
    }

    public void Select(string path)
    {
        SelectedPath = path;
        Log($"Selected {Path.GetFileName(path)}");
    }

    public void RequestExit()
    {
        ExitRequested = true;
        Log("Exit requested");
    }

    public void Log(string message) => _log.Add(message);
}
