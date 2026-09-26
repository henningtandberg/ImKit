using System;
using System.Numerics;
using ImGuiNET;

namespace ImKit;

/// <summary>
/// Scope-based wrappers over Dear ImGui's Begin/End pairs. Each overload takes the
/// body as a callback and guarantees the matching End call, removing the class of
/// bugs where an early return leaks an unbalanced stack.
/// </summary>
public static class ImGuiExtensions
{
    extension(ImGui)
    {
        public static void Begin(string name, Action content)
        {
            if (ImGui.Begin(name))
            {
                content();
            }

            ImGui.End();
        }

        public static void Begin(string name, ImGuiWindowFlags flags, Action content)
        {
            if (ImGui.Begin(name, flags))
            {
                content();
            }

            ImGui.End();
        }

        public static void Begin(string name, ref bool isOpen, ImGuiWindowFlags flags, Action content)
        {
            if (ImGui.Begin(name, ref isOpen, flags))
            {
                content();
            }

            ImGui.End();
        }

        public static void BeginChild(string id, Action content)
        {
            if (ImGui.BeginChild(id))
            {
                content();
            }

            ImGui.EndChild();
        }

        public static void BeginChild(string id, Vector2 size, ImGuiChildFlags flags, Action content)
        {
            if (ImGui.BeginChild(id, size, flags))
            {
                content();
            }

            ImGui.EndChild();
        }

        public static void BeginChild(string id, Vector2 size, ImGuiChildFlags childFlags, ImGuiWindowFlags windowFlags,
            Action content)
        {
            if (ImGui.BeginChild(id, size, childFlags, windowFlags))
            {
                content();
            }

            ImGui.EndChild();
        }

        public static void BeginPopupModal(string name, ref bool open, Action content)
        {
            if (!ImGui.BeginPopupModal(name, ref open))
            {
                return;
            }

            content();
            ImGui.EndPopup();
        }

        #region Menu

        public static void BeginMenuBar(Action content)
        {
            if (!ImGui.BeginMenuBar())
                return;

            content();
            ImGui.EndMenuBar();
        }

        public static void BeginMainMenuBar(Action content)
        {
            if (!ImGui.BeginMainMenuBar())
                return;

            content();
            ImGui.EndMainMenuBar();
        }

        public static void BeginMenu(string label, Action action)
        {
            if (!ImGui.BeginMenu(label))
                return;

            action();
            ImGui.EndMenu();
        }

        public static void MenuItem(string label, Action onClick)
        {
            if (ImGui.MenuItem(label))
            {
                onClick();
            }
        }

        public static void MenuItem(string label, string shortcut, Action onClick)
        {
            if (ImGui.MenuItem(label, shortcut))
            {
                onClick();
            }
        }

        #endregion // Menu

        #region Button

        public static void Button(string label, Action onClick)
        {
            if (ImGui.Button(label))
            {
                onClick();
            }
        }

        public static void Button(string label, Action onClick, int id)
        {
            ImGui.PushID(id);

            if (ImGui.Button(label))
            {
                onClick();
            }

            ImGui.PopID();
        }

        #endregion // Button

        #region Selectable

        public static void Selectable(string label, Action onClick)
        {
            if (ImGui.Selectable(label))
            {
                onClick();
            }
        }

        public static void Selectable(string label, bool selected, Action onClick)
        {
            if (ImGui.Selectable(label, selected))
            {
                onClick();
            }
        }

        #endregion // Selectable

        #region Separator

        public static void SeparatorText(string label, Action content)
        {
            ImGui.SeparatorText(label);
            content();
        }

        #endregion // Separator
    }
}
