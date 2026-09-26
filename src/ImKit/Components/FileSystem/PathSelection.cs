using System;
using System.IO;
using System.Linq;
using ImGuiNET;

namespace ImKit.Components.FileSystem;

/// <summary>
/// Selectable rows for a single filesystem entry. The building block shared by
/// <see cref="FilePicker"/> and <see cref="FileTree"/>.
/// </summary>
/// <remarks>
/// Every callback receives the entry's full path, never its display name, so callers
/// can compare against and act on paths directly.
/// </remarks>
public static class PathSelection
{
    #region Directory

    public static void ParentDirectoryPath(
        DirectoryInfo directoryInfo,
        string selectedPath,
        Action<string> onSelected,
        Action<string> onDoubleClick,
        Action<string> onRightClick)
    {
        if (directoryInfo.Parent == null) return;

        var path = directoryInfo.Parent.FullName;
        const ImGuiSelectableFlags flags = ImGuiSelectableFlags.NoAutoClosePopups;

        ImGui.PushStyleColor(ImGuiCol.Text, ColorTheme.Yellow);

        if (ImGui.Selectable(label: "../", selected: path.Equals(selectedPath), flags))
        {
            onSelected(path);
        }

        if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(0))
        {
            onDoubleClick(path);
        }

        if (ImGui.IsItemHovered() && ImGui.IsKeyReleased(ImGuiKey.MouseRight))
        {
            onRightClick(path);
        }

        ImGui.PopStyleColor();
    }

    public static void DirectoryPath(
        string displayName,
        string path,
        string selectedPath,
        Action<string> onSelected,
        Action<string> onDoubleClick,
        Action<string> onRightClick)
    {
        const ImGuiSelectableFlags flags =
            ImGuiSelectableFlags.NoAutoClosePopups |
            ImGuiSelectableFlags.AllowDoubleClick;

        ImGui.PushStyleColor(ImGuiCol.Text, ColorTheme.Yellow);

        if (ImGui.Selectable(label: displayName, selected: path.Equals(selectedPath), flags))
        {
            onSelected(path);
        }

        if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(0))
        {
            onDoubleClick(path);
        }

        if (ImGui.IsItemHovered() && ImGui.IsKeyReleased(ImGuiKey.MouseRight))
        {
            onRightClick(path);
        }

        ImGui.PopStyleColor();
    }

    #endregion // Directory

    #region File

    public static void FilePath(
        string path,
        bool isSelected,
        Action<string> onSelected,
        Action onDoubleClick)
    {
        var displayName = Path.GetFileName(path);
        const ImGuiSelectableFlags flags =
            ImGuiSelectableFlags.NoAutoClosePopups |
            ImGuiSelectableFlags.AllowDoubleClick;

        if (ImGui.Selectable(displayName, isSelected, flags))
        {
            onSelected(path);
        }

        if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(0))
        {
            onDoubleClick();
        }
    }

    public static void FilePath(
        string path,
        string[] skippedExtensions,
        bool isSelected,
        Action<string> onSelected,
        Action onDoubleClick)
    {
        var extension = Path.GetExtension(path);

        if (skippedExtensions.Contains(extension))
        {
            return;
        }

        FilePath(path, isSelected, onSelected, onDoubleClick);
    }

    #endregion // File
}
