using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImGuiNET;

using static ImKit.Components.FileSystem.PathSelection;

namespace ImKit.Components.FileSystem;

/// <summary>
/// Expandable directory tree rooted at <paramref name="workingDirectory"/>. Reads the
/// filesystem every frame rather than caching, so external changes show up immediately.
/// </summary>
public class FileTree(
    string title,
    string workingDirectory,
    string[] skippedExtensions,
    Action<string> onOpen,
    Action<string> onRightClick)
{
    private const float IndentPerDepth = 16f;

    private readonly HashSet<string> _expanded = [];
    private string _selectedPath = string.Empty;

    public void Draw() => ImGui.BeginChild(title, () =>
    {
        ImGui.Text(workingDirectory);
        ImGui.Separator();

        DrawDirectory(workingDirectory, 0);
    });

    private void DrawDirectory(string path, int depth)
    {
        foreach (var entry in EnumerateEntries(path).Order())
        {
            var name = Path.GetFileName(entry);

            ImGui.PushID(entry);
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + depth * IndentPerDepth);

            switch (entry.GetFileType())
            {
                case FileType.Regular:
                    FilePath(
                        path: entry,
                        skippedExtensions: skippedExtensions,
                        isSelected: _selectedPath == entry,
                        onSelected: selectedPath => _selectedPath = selectedPath,
                        onDoubleClick: () => onOpen(entry));
                    break;
                case FileType.Directory:
                {
                    var isExpanded = _expanded.Contains(entry);

                    DirectoryPath(
                        displayName: $"{(isExpanded ? "v" : ">")} {name}",
                        path: entry,
                        selectedPath: _selectedPath,
                        onSelected: ToggleExpandedAndSelectPath,
                        onDoubleClick: ToggleExpandedAndSelectPath,
                        onRightClick: onRightClick);

                    if (isExpanded)
                        DrawDirectory(entry, depth + 1);
                }
                    break;
                case FileType.Link:
                case FileType.Special:
                case FileType.Hidden:
                case FileType.DirectoryHidden:
                    // Not interested in displaying
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ImGui.PopID();
        }
    }

    /// <summary>
    /// Enumerates a directory, treating an unreadable directory as empty rather than
    /// letting the exception escape into the middle of an ImGui child window.
    /// </summary>
    private static IEnumerable<string> EnumerateEntries(string path)
    {
        try
        {
            return Directory.EnumerateFileSystemEntries(path);
        }
        catch (Exception e) when (e is UnauthorizedAccessException or IOException)
        {
            return [];
        }
    }

    private void ToggleExpandedAndSelectPath(string path)
    {
        _selectedPath = path;
        if (!_expanded.Add(path))
            _expanded.Remove(path);
    }
}
