using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ImGuiNET;

using static ImKit.Components.FileSystem.PathSelection;

namespace ImKit.Components.FileSystem;

/// <summary>
/// Modal file/folder picker. Call <see cref="Draw"/> every frame while the picker
/// should be open; it invokes <c>onOpen</c> or <c>onCancel</c> exactly once and then
/// closes itself, so the owner is expected to stop calling <see cref="Draw"/> from
/// inside that callback.
/// </summary>
public class FilePicker(
    string title,
    string startingDirectory,
    string[] skippedExtensions,
    Action<string> onOpen,
    Action? onCancel = null)
{
    private static readonly Vector2 DefaultFilePickerSize = new(600, 400);

    private string _currentDirectory = startingDirectory;
    private string _selectedPath = startingDirectory;

    public void Draw()
    {
        ImGui.OpenPopup(title);
        ImGui.SetNextWindowSize(DefaultFilePickerSize, ImGuiCond.FirstUseEver);

        var popupOpen = true;
        ImGui.BeginPopupModal(title, ref popupOpen, () =>
        {
            ImGui.Text("Current Folder: " + _currentDirectory);
            ImGui.Text("Selected: " + _selectedPath);

            DrawFileList();

            ImGui.Button("Cancel", onClick: Cancel);
            ImGui.SameLine();
            ImGui.Button("Open", onClick: Open);
        });

        if (!popupOpen)
        {
            Cancel();
        }
    }

    private void DrawFileList() => ImGui.BeginChild("FileList", new Vector2(0, -30), ImGuiChildFlags.FrameStyle, () =>
    {
        var currentDirectoryInfo = new DirectoryInfo(_currentDirectory);

        if (!currentDirectoryInfo.Exists)
        {
            return;
        }

        ParentDirectoryPath(
            directoryInfo: currentDirectoryInfo,
            selectedPath: _selectedPath,
            onSelected: _ => { },
            onDoubleClick: selectedPath => _currentDirectory = _selectedPath = selectedPath,
            onRightClick: _ => { });

        foreach (var entry in EnumerateEntries(currentDirectoryInfo.FullName))
        {
            switch (entry.GetFileType())
            {
                case FileType.Regular:
                    FilePath(
                        path: entry,
                        skippedExtensions: skippedExtensions,
                        isSelected: _selectedPath == entry,
                        onSelected: selectedPath => _selectedPath = selectedPath,
                        onDoubleClick: Open);
                    break;
                case FileType.Directory:
                    DirectoryPath(
                        displayName: Path.GetFileName(entry) + "/",
                        path: entry,
                        selectedPath: _selectedPath,
                        onSelected: selectedPath => _selectedPath = selectedPath,
                        onDoubleClick: selectedPath => _currentDirectory = selectedPath,
                        onRightClick: _ => { });
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
        }
    });

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

    private void Open()
    {
        ImGui.CloseCurrentPopup();

        if (_selectedPath == string.Empty)
        {
            _selectedPath = _currentDirectory;
        }

        onOpen(_selectedPath);
    }

    private void Cancel()
    {
        ImGui.CloseCurrentPopup();

        onCancel?.Invoke();
    }
}
