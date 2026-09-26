using System;
using System.Numerics;
using ImGuiNET;

namespace ImKit;

/// <summary>
/// Scope-based wrappers over Dear ImGui's Begin/End pairs. Each overload takes the
/// body as a callback and guarantees the matching End call, removing the class of
/// bugs where an early return leaks an unbalanced stack.
///
/// The interactive widgets follow the same idea one level down: instead of an
/// <c>if</c> around a bool return, they take the reaction as a callback.
/// </summary>
/// <remarks>
/// Conventions, so a call reads the same no matter which widget it is:
/// <list type="bullet">
/// <item>The callback is always the last parameter, so it can be written as a
/// trailing lambda.</item>
/// <item>Scope wrappers take <c>Action content</c>, invoked only when the scope is
/// actually open. The End call happens either way where Dear ImGui requires it
/// (<c>Begin</c>, <c>BeginChild</c>, <c>BeginGroup</c>, <c>BeginDisabled</c>,
/// <c>BeginMultiSelect</c>) and only on success everywhere else.</item>
/// <item>Click-like widgets take <c>Action onClick</c>.</item>
/// <item>Value widgets keep their <c>ref</c> parameter and take
/// <c>Action&lt;T&gt; onChanged</c>, invoked with the new value on the frames the
/// widget reports an edit. Multi-component overloads that take a <c>ref int</c>
/// pointing at the first of N elements take a plain <c>Action onChanged</c>, since
/// there is no single value to hand back.</item>
/// <item>Overloads mirror the shortest and the fully-parameterised form of each
/// ImGui method; the intermediate overloads are omitted, as ImGui's own defaults
/// cover them.</item>
/// </list>
/// The pointer-based <c>*Scalar</c> / <c>*ScalarN</c> family is deliberately not
/// wrapped: those take raw <see cref="IntPtr"/> data and gain nothing from a
/// callback. Predicate queries (<c>IsItemHovered</c>, <c>IsKeyPressed</c>, …) are
/// not wrapped either — they are conditions, not scopes.
/// </remarks>
public static class ImGuiExtensions
{
    extension(ImGui)
    {
        #region Window

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

        public static void Begin(string name, ref bool isOpen, Action content)
        {
            if (ImGui.Begin(name, ref isOpen))
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

        #endregion // Window

        #region Child

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

        public static void BeginChild(uint id, Action content)
        {
            if (ImGui.BeginChild(id))
            {
                content();
            }

            ImGui.EndChild();
        }

        public static void BeginChild(uint id, Vector2 size, ImGuiChildFlags childFlags, ImGuiWindowFlags windowFlags,
            Action content)
        {
            if (ImGui.BeginChild(id, size, childFlags, windowFlags))
            {
                content();
            }

            ImGui.EndChild();
        }

        #endregion // Child

        #region Popup

        public static void BeginPopup(string id, Action content)
        {
            if (!ImGui.BeginPopup(id))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopup(string id, ImGuiWindowFlags flags, Action content)
        {
            if (!ImGui.BeginPopup(id, flags))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupModal(string name, Action content)
        {
            if (!ImGui.BeginPopupModal(name))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupModal(string name, ImGuiWindowFlags flags, Action content)
        {
            if (!ImGui.BeginPopupModal(name, flags))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupModal(string name, ref bool open, Action content)
        {
            if (!ImGui.BeginPopupModal(name, ref open))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupModal(string name, ref bool open, ImGuiWindowFlags flags, Action content)
        {
            if (!ImGui.BeginPopupModal(name, ref open, flags))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupContextItem(Action content)
        {
            if (!ImGui.BeginPopupContextItem())
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupContextItem(string id, ImGuiPopupFlags flags, Action content)
        {
            if (!ImGui.BeginPopupContextItem(id, flags))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupContextWindow(Action content)
        {
            if (!ImGui.BeginPopupContextWindow())
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupContextWindow(string id, ImGuiPopupFlags flags, Action content)
        {
            if (!ImGui.BeginPopupContextWindow(id, flags))
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupContextVoid(Action content)
        {
            if (!ImGui.BeginPopupContextVoid())
                return;

            content();
            ImGui.EndPopup();
        }

        public static void BeginPopupContextVoid(string id, ImGuiPopupFlags flags, Action content)
        {
            if (!ImGui.BeginPopupContextVoid(id, flags))
                return;

            content();
            ImGui.EndPopup();
        }

        #endregion // Popup

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

        public static void BeginMenu(string label, bool enabled, Action action)
        {
            if (!ImGui.BeginMenu(label, enabled))
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

        public static void MenuItem(string label, string shortcut, bool selected, bool enabled, Action onClick)
        {
            if (ImGui.MenuItem(label, shortcut, selected, enabled))
            {
                onClick();
            }
        }

        /// <summary>
        /// Checkable menu item. <paramref name="selected"/> is toggled by ImGui itself;
        /// <paramref name="onToggled"/> receives the state it toggled to.
        /// </summary>
        public static void MenuItem(string label, string shortcut, ref bool selected, Action<bool> onToggled)
        {
            if (ImGui.MenuItem(label, shortcut, ref selected))
            {
                onToggled(selected);
            }
        }

        #endregion // Menu

        #region Tab bar

        public static void BeginTabBar(string id, Action content)
        {
            if (!ImGui.BeginTabBar(id))
                return;

            content();
            ImGui.EndTabBar();
        }

        public static void BeginTabBar(string id, ImGuiTabBarFlags flags, Action content)
        {
            if (!ImGui.BeginTabBar(id, flags))
                return;

            content();
            ImGui.EndTabBar();
        }

        public static void BeginTabItem(string label, Action content)
        {
            if (!ImGui.BeginTabItem(label))
                return;

            content();
            ImGui.EndTabItem();
        }

        public static void BeginTabItem(string label, ref bool open, Action content)
        {
            if (!ImGui.BeginTabItem(label, ref open))
                return;

            content();
            ImGui.EndTabItem();
        }

        public static void BeginTabItem(string label, ref bool open, ImGuiTabItemFlags flags, Action content)
        {
            if (!ImGui.BeginTabItem(label, ref open, flags))
                return;

            content();
            ImGui.EndTabItem();
        }

        public static void TabItemButton(string label, Action onClick)
        {
            if (ImGui.TabItemButton(label))
            {
                onClick();
            }
        }

        public static void TabItemButton(string label, ImGuiTabItemFlags flags, Action onClick)
        {
            if (ImGui.TabItemButton(label, flags))
            {
                onClick();
            }
        }

        #endregion // Tab bar

        #region Table

        public static void BeginTable(string id, int columns, Action content)
        {
            if (!ImGui.BeginTable(id, columns))
                return;

            content();
            ImGui.EndTable();
        }

        public static void BeginTable(string id, int columns, ImGuiTableFlags flags, Action content)
        {
            if (!ImGui.BeginTable(id, columns, flags))
                return;

            content();
            ImGui.EndTable();
        }

        public static void BeginTable(string id, int columns, ImGuiTableFlags flags, Vector2 outerSize,
            float innerWidth, Action content)
        {
            if (!ImGui.BeginTable(id, columns, flags, outerSize, innerWidth))
                return;

            content();
            ImGui.EndTable();
        }

        /// <summary>
        /// Advances to the next column and draws into it, unless that column is clipped.
        /// </summary>
        public static void TableNextColumn(Action content)
        {
            if (ImGui.TableNextColumn())
            {
                content();
            }
        }

        public static void TableSetColumnIndex(int column, Action content)
        {
            if (ImGui.TableSetColumnIndex(column))
            {
                content();
            }
        }

        #endregion // Table

        #region Tree

        public static void TreeNode(string label, Action content)
        {
            if (!ImGui.TreeNode(label))
                return;

            content();
            ImGui.TreePop();
        }

        public static void TreeNode(string id, string format, Action content)
        {
            if (!ImGui.TreeNode(id, format))
                return;

            content();
            ImGui.TreePop();
        }

        public static void TreeNodeEx(string label, Action content)
        {
            if (!ImGui.TreeNodeEx(label))
                return;

            content();
            ImGui.TreePop();
        }

        public static void TreeNodeEx(string label, ImGuiTreeNodeFlags flags, Action content)
        {
            if (!ImGui.TreeNodeEx(label, flags))
                return;

            content();
            ImGui.TreePop();
        }

        public static void TreeNodeEx(string id, ImGuiTreeNodeFlags flags, string format, Action content)
        {
            if (!ImGui.TreeNodeEx(id, flags, format))
                return;

            content();
            ImGui.TreePop();
        }

        /// <summary>
        /// Pushes a tree level without a node label, for content that is indented and
        /// id-scoped but has no header of its own.
        /// </summary>
        public static void TreePush(string id, Action content)
        {
            ImGui.TreePush(id);
            content();
            ImGui.TreePop();
        }

        /// <summary>
        /// Drawn open or closed; <paramref name="content"/> runs only while open. A
        /// collapsing header has no matching pop.
        /// </summary>
        public static void CollapsingHeader(string label, Action content)
        {
            if (ImGui.CollapsingHeader(label))
            {
                content();
            }
        }

        public static void CollapsingHeader(string label, ImGuiTreeNodeFlags flags, Action content)
        {
            if (ImGui.CollapsingHeader(label, flags))
            {
                content();
            }
        }

        /// <summary>
        /// Collapsing header with a close button. <paramref name="visible"/> is cleared
        /// by ImGui when that button is pressed.
        /// </summary>
        public static void CollapsingHeader(string label, ref bool visible, ImGuiTreeNodeFlags flags, Action content)
        {
            if (ImGui.CollapsingHeader(label, ref visible, flags))
            {
                content();
            }
        }

        #endregion // Tree

        #region Combo and list box

        public static void BeginCombo(string label, string previewValue, Action content)
        {
            if (!ImGui.BeginCombo(label, previewValue))
                return;

            content();
            ImGui.EndCombo();
        }

        public static void BeginCombo(string label, string previewValue, ImGuiComboFlags flags, Action content)
        {
            if (!ImGui.BeginCombo(label, previewValue, flags))
                return;

            content();
            ImGui.EndCombo();
        }

        public static void BeginListBox(string label, Action content)
        {
            if (!ImGui.BeginListBox(label))
                return;

            content();
            ImGui.EndListBox();
        }

        public static void BeginListBox(string label, Vector2 size, Action content)
        {
            if (!ImGui.BeginListBox(label, size))
                return;

            content();
            ImGui.EndListBox();
        }

        public static void Combo(string label, ref int currentItem, string[] items, int itemsCount,
            Action<int> onChanged)
        {
            if (ImGui.Combo(label, ref currentItem, items, itemsCount))
            {
                onChanged(currentItem);
            }
        }

        public static void Combo(string label, ref int currentItem, string[] items, int itemsCount,
            int popupMaxHeightInItems, Action<int> onChanged)
        {
            if (ImGui.Combo(label, ref currentItem, items, itemsCount, popupMaxHeightInItems))
            {
                onChanged(currentItem);
            }
        }

        public static void ListBox(string label, ref int currentItem, string[] items, int itemsCount,
            Action<int> onChanged)
        {
            if (ImGui.ListBox(label, ref currentItem, items, itemsCount))
            {
                onChanged(currentItem);
            }
        }

        public static void ListBox(string label, ref int currentItem, string[] items, int itemsCount,
            int heightInItems, Action<int> onChanged)
        {
            if (ImGui.ListBox(label, ref currentItem, items, itemsCount, heightInItems))
            {
                onChanged(currentItem);
            }
        }

        #endregion // Combo and list box

        #region Tooltip

        public static void BeginTooltip(Action content)
        {
            if (!ImGui.BeginTooltip())
                return;

            content();
            ImGui.EndTooltip();
        }

        /// <summary>
        /// Tooltip for the previous item, shown only once that item has been hovered
        /// long enough.
        /// </summary>
        public static void BeginItemTooltip(Action content)
        {
            if (!ImGui.BeginItemTooltip())
                return;

            content();
            ImGui.EndTooltip();
        }

        #endregion // Tooltip

        #region Drag and drop

        public static void BeginDragDropSource(Action content)
        {
            if (!ImGui.BeginDragDropSource())
                return;

            content();
            ImGui.EndDragDropSource();
        }

        public static void BeginDragDropSource(ImGuiDragDropFlags flags, Action content)
        {
            if (!ImGui.BeginDragDropSource(flags))
                return;

            content();
            ImGui.EndDragDropSource();
        }

        public static void BeginDragDropTarget(Action content)
        {
            if (!ImGui.BeginDragDropTarget())
                return;

            content();
            ImGui.EndDragDropTarget();
        }

        #endregion // Drag and drop

        #region Group, disabled and multi-select

        public static void BeginGroup(Action content)
        {
            ImGui.BeginGroup();
            content();
            ImGui.EndGroup();
        }

        public static void BeginDisabled(Action content)
        {
            ImGui.BeginDisabled();
            content();
            ImGui.EndDisabled();
        }

        public static void BeginDisabled(bool disabled, Action content)
        {
            ImGui.BeginDisabled(disabled);
            content();
            ImGui.EndDisabled();
        }

        /// <summary>
        /// Opens a multi-select scope, hands the entry request list to
        /// <paramref name="content"/>, and returns the exit request list produced by
        /// <c>EndMultiSelect</c>. Both have to be applied to the caller's selection
        /// state.
        /// </summary>
        public static ImGuiMultiSelectIOPtr BeginMultiSelect(ImGuiMultiSelectFlags flags, int selectionSize,
            int itemsCount, Action<ImGuiMultiSelectIOPtr> content)
        {
            var io = ImGui.BeginMultiSelect(flags, selectionSize, itemsCount);
            content(io);
            return ImGui.EndMultiSelect();
        }

        #endregion // Group, disabled and multi-select

        #region Id and style stack

        public static void PushID(string id, Action content)
        {
            ImGui.PushID(id);
            content();
            ImGui.PopID();
        }

        public static void PushID(int id, Action content)
        {
            ImGui.PushID(id);
            content();
            ImGui.PopID();
        }

        public static void PushID(IntPtr id, Action content)
        {
            ImGui.PushID(id);
            content();
            ImGui.PopID();
        }

        public static void PushStyleVar(ImGuiStyleVar variable, float value, Action content)
        {
            ImGui.PushStyleVar(variable, value);
            content();
            ImGui.PopStyleVar();
        }

        public static void PushStyleVar(ImGuiStyleVar variable, Vector2 value, Action content)
        {
            ImGui.PushStyleVar(variable, value);
            content();
            ImGui.PopStyleVar();
        }

        public static void PushStyleVarX(ImGuiStyleVar variable, float valueX, Action content)
        {
            ImGui.PushStyleVarX(variable, valueX);
            content();
            ImGui.PopStyleVar();
        }

        public static void PushStyleVarY(ImGuiStyleVar variable, float valueY, Action content)
        {
            ImGui.PushStyleVarY(variable, valueY);
            content();
            ImGui.PopStyleVar();
        }

        public static void PushStyleColor(ImGuiCol index, uint color, Action content)
        {
            ImGui.PushStyleColor(index, color);
            content();
            ImGui.PopStyleColor();
        }

        public static void PushStyleColor(ImGuiCol index, Vector4 color, Action content)
        {
            ImGui.PushStyleColor(index, color);
            content();
            ImGui.PopStyleColor();
        }

        public static void PushItemWidth(float itemWidth, Action content)
        {
            ImGui.PushItemWidth(itemWidth);
            content();
            ImGui.PopItemWidth();
        }

        public static void PushItemFlag(ImGuiItemFlags option, bool enabled, Action content)
        {
            ImGui.PushItemFlag(option, enabled);
            content();
            ImGui.PopItemFlag();
        }

        public static void PushTextWrapPos(Action content)
        {
            ImGui.PushTextWrapPos();
            content();
            ImGui.PopTextWrapPos();
        }

        public static void PushTextWrapPos(float wrapLocalPosX, Action content)
        {
            ImGui.PushTextWrapPos(wrapLocalPosX);
            content();
            ImGui.PopTextWrapPos();
        }

        public static void PushFont(ImFontPtr font, Action content)
        {
            ImGui.PushFont(font);
            content();
            ImGui.PopFont();
        }

        public static void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect,
            Action content)
        {
            ImGui.PushClipRect(clipRectMin, clipRectMax, intersectWithCurrentClipRect);
            content();
            ImGui.PopClipRect();
        }

        public static void Indent(Action content)
        {
            ImGui.Indent();
            content();
            ImGui.Unindent();
        }

        public static void Indent(float indentWidth, Action content)
        {
            ImGui.Indent(indentWidth);
            content();
            ImGui.Unindent(indentWidth);
        }

        #endregion // Id and style stack

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

        public static void Button(string label, Vector2 size, Action onClick)
        {
            if (ImGui.Button(label, size))
            {
                onClick();
            }
        }

        public static void SmallButton(string label, Action onClick)
        {
            if (ImGui.SmallButton(label))
            {
                onClick();
            }
        }

        public static void InvisibleButton(string id, Vector2 size, Action onClick)
        {
            if (ImGui.InvisibleButton(id, size))
            {
                onClick();
            }
        }

        public static void InvisibleButton(string id, Vector2 size, ImGuiButtonFlags flags, Action onClick)
        {
            if (ImGui.InvisibleButton(id, size, flags))
            {
                onClick();
            }
        }

        public static void ArrowButton(string id, ImGuiDir direction, Action onClick)
        {
            if (ImGui.ArrowButton(id, direction))
            {
                onClick();
            }
        }

        public static void ImageButton(string id, IntPtr textureId, Vector2 imageSize, Action onClick)
        {
            if (ImGui.ImageButton(id, textureId, imageSize))
            {
                onClick();
            }
        }

        public static void ImageButton(string id, IntPtr textureId, Vector2 imageSize, Vector2 uv0, Vector2 uv1,
            Vector4 backgroundColor, Vector4 tintColor, Action onClick)
        {
            if (ImGui.ImageButton(id, textureId, imageSize, uv0, uv1, backgroundColor, tintColor))
            {
                onClick();
            }
        }

        public static void ColorButton(string descriptionId, Vector4 color, Action onClick)
        {
            if (ImGui.ColorButton(descriptionId, color))
            {
                onClick();
            }
        }

        public static void ColorButton(string descriptionId, Vector4 color, ImGuiColorEditFlags flags, Vector2 size,
            Action onClick)
        {
            if (ImGui.ColorButton(descriptionId, color, flags, size))
            {
                onClick();
            }
        }

        public static void TextLink(string label, Action onClick)
        {
            if (ImGui.TextLink(label))
            {
                onClick();
            }
        }

        /// <summary>
        /// Runs <paramref name="onPressed"/> when the key chord is pressed and routed to
        /// this scope.
        /// </summary>
        public static void Shortcut(ImGuiKey keyChord, Action onPressed)
        {
            if (ImGui.Shortcut(keyChord))
            {
                onPressed();
            }
        }

        public static void Shortcut(ImGuiKey keyChord, ImGuiInputFlags flags, Action onPressed)
        {
            if (ImGui.Shortcut(keyChord, flags))
            {
                onPressed();
            }
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

        public static void Selectable(string label, bool selected, ImGuiSelectableFlags flags, Vector2 size,
            Action onClick)
        {
            if (ImGui.Selectable(label, selected, flags, size))
            {
                onClick();
            }
        }

        /// <summary>
        /// Self-toggling selectable: ImGui flips <paramref name="selected"/> and
        /// <paramref name="onChanged"/> receives the state it flipped to.
        /// </summary>
        public static void Selectable(string label, ref bool selected, Action<bool> onChanged)
        {
            if (ImGui.Selectable(label, ref selected))
            {
                onChanged(selected);
            }
        }

        public static void Selectable(string label, ref bool selected, ImGuiSelectableFlags flags, Vector2 size,
            Action<bool> onChanged)
        {
            if (ImGui.Selectable(label, ref selected, flags, size))
            {
                onChanged(selected);
            }
        }

        #endregion // Selectable

        #region Checkbox and radio

        public static void Checkbox(string label, ref bool value, Action<bool> onChanged)
        {
            if (ImGui.Checkbox(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void CheckboxFlags(string label, ref int flags, int flagsValue, Action<int> onChanged)
        {
            if (ImGui.CheckboxFlags(label, ref flags, flagsValue))
            {
                onChanged(flags);
            }
        }

        public static void CheckboxFlags(string label, ref uint flags, uint flagsValue, Action<uint> onChanged)
        {
            if (ImGui.CheckboxFlags(label, ref flags, flagsValue))
            {
                onChanged(flags);
            }
        }

        public static void RadioButton(string label, bool active, Action onClick)
        {
            if (ImGui.RadioButton(label, active))
            {
                onClick();
            }
        }

        public static void RadioButton(string label, ref int value, int buttonValue, Action<int> onChanged)
        {
            if (ImGui.RadioButton(label, ref value, buttonValue))
            {
                onChanged(value);
            }
        }

        #endregion // Checkbox and radio

        #region Text input

        public static void InputText(string label, ref string input, uint maxLength, Action<string> onChanged)
        {
            if (ImGui.InputText(label, ref input, maxLength))
            {
                onChanged(input);
            }
        }

        public static void InputText(string label, ref string input, uint maxLength, ImGuiInputTextFlags flags,
            Action<string> onChanged)
        {
            if (ImGui.InputText(label, ref input, maxLength, flags))
            {
                onChanged(input);
            }
        }

        public static void InputTextWithHint(string label, string hint, ref string input, uint maxLength,
            Action<string> onChanged)
        {
            if (ImGui.InputTextWithHint(label, hint, ref input, maxLength))
            {
                onChanged(input);
            }
        }

        public static void InputTextWithHint(string label, string hint, ref string input, uint maxLength,
            ImGuiInputTextFlags flags, Action<string> onChanged)
        {
            if (ImGui.InputTextWithHint(label, hint, ref input, maxLength, flags))
            {
                onChanged(input);
            }
        }

        public static void InputTextMultiline(string label, ref string input, uint maxLength, Vector2 size,
            Action<string> onChanged)
        {
            if (ImGui.InputTextMultiline(label, ref input, maxLength, size))
            {
                onChanged(input);
            }
        }

        public static void InputTextMultiline(string label, ref string input, uint maxLength, Vector2 size,
            ImGuiInputTextFlags flags, Action<string> onChanged)
        {
            if (ImGui.InputTextMultiline(label, ref input, maxLength, size, flags))
            {
                onChanged(input);
            }
        }

        #endregion // Text input

        #region Numeric input

        public static void InputFloat(string label, ref float value, Action<float> onChanged)
        {
            if (ImGui.InputFloat(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void InputFloat(string label, ref float value, float step, float stepFast, string format,
            ImGuiInputTextFlags flags, Action<float> onChanged)
        {
            if (ImGui.InputFloat(label, ref value, step, stepFast, format, flags))
            {
                onChanged(value);
            }
        }

        public static void InputFloat2(string label, ref Vector2 value, Action<Vector2> onChanged)
        {
            if (ImGui.InputFloat2(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void InputFloat2(string label, ref Vector2 value, string format, ImGuiInputTextFlags flags,
            Action<Vector2> onChanged)
        {
            if (ImGui.InputFloat2(label, ref value, format, flags))
            {
                onChanged(value);
            }
        }

        public static void InputFloat3(string label, ref Vector3 value, Action<Vector3> onChanged)
        {
            if (ImGui.InputFloat3(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void InputFloat3(string label, ref Vector3 value, string format, ImGuiInputTextFlags flags,
            Action<Vector3> onChanged)
        {
            if (ImGui.InputFloat3(label, ref value, format, flags))
            {
                onChanged(value);
            }
        }

        public static void InputFloat4(string label, ref Vector4 value, Action<Vector4> onChanged)
        {
            if (ImGui.InputFloat4(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void InputFloat4(string label, ref Vector4 value, string format, ImGuiInputTextFlags flags,
            Action<Vector4> onChanged)
        {
            if (ImGui.InputFloat4(label, ref value, format, flags))
            {
                onChanged(value);
            }
        }

        public static void InputInt(string label, ref int value, Action<int> onChanged)
        {
            if (ImGui.InputInt(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void InputInt(string label, ref int value, int step, int stepFast, ImGuiInputTextFlags flags,
            Action<int> onChanged)
        {
            if (ImGui.InputInt(label, ref value, step, stepFast, flags))
            {
                onChanged(value);
            }
        }

        /// <summary>
        /// <paramref name="value"/> is the first of two adjacent ints, so there is no
        /// single value to hand back — read the array in the callback.
        /// </summary>
        public static void InputInt2(string label, ref int value, Action onChanged)
        {
            if (ImGui.InputInt2(label, ref value))
            {
                onChanged();
            }
        }

        public static void InputInt2(string label, ref int value, ImGuiInputTextFlags flags, Action onChanged)
        {
            if (ImGui.InputInt2(label, ref value, flags))
            {
                onChanged();
            }
        }

        public static void InputInt3(string label, ref int value, Action onChanged)
        {
            if (ImGui.InputInt3(label, ref value))
            {
                onChanged();
            }
        }

        public static void InputInt3(string label, ref int value, ImGuiInputTextFlags flags, Action onChanged)
        {
            if (ImGui.InputInt3(label, ref value, flags))
            {
                onChanged();
            }
        }

        public static void InputInt4(string label, ref int value, Action onChanged)
        {
            if (ImGui.InputInt4(label, ref value))
            {
                onChanged();
            }
        }

        public static void InputInt4(string label, ref int value, ImGuiInputTextFlags flags, Action onChanged)
        {
            if (ImGui.InputInt4(label, ref value, flags))
            {
                onChanged();
            }
        }

        public static void InputDouble(string label, ref double value, Action<double> onChanged)
        {
            if (ImGui.InputDouble(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void InputDouble(string label, ref double value, double step, double stepFast, string format,
            ImGuiInputTextFlags flags, Action<double> onChanged)
        {
            if (ImGui.InputDouble(label, ref value, step, stepFast, format, flags))
            {
                onChanged(value);
            }
        }

        #endregion // Numeric input

        #region Drag

        public static void DragFloat(string label, ref float value, Action<float> onChanged)
        {
            if (ImGui.DragFloat(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void DragFloat(string label, ref float value, float speed, float min, float max, string format,
            ImGuiSliderFlags flags, Action<float> onChanged)
        {
            if (ImGui.DragFloat(label, ref value, speed, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void DragFloat2(string label, ref Vector2 value, Action<Vector2> onChanged)
        {
            if (ImGui.DragFloat2(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void DragFloat2(string label, ref Vector2 value, float speed, float min, float max,
            string format, ImGuiSliderFlags flags, Action<Vector2> onChanged)
        {
            if (ImGui.DragFloat2(label, ref value, speed, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void DragFloat3(string label, ref Vector3 value, Action<Vector3> onChanged)
        {
            if (ImGui.DragFloat3(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void DragFloat3(string label, ref Vector3 value, float speed, float min, float max,
            string format, ImGuiSliderFlags flags, Action<Vector3> onChanged)
        {
            if (ImGui.DragFloat3(label, ref value, speed, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void DragFloat4(string label, ref Vector4 value, Action<Vector4> onChanged)
        {
            if (ImGui.DragFloat4(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void DragFloat4(string label, ref Vector4 value, float speed, float min, float max,
            string format, ImGuiSliderFlags flags, Action<Vector4> onChanged)
        {
            if (ImGui.DragFloat4(label, ref value, speed, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void DragFloatRange2(string label, ref float currentMin, ref float currentMax,
            Action<float, float> onChanged)
        {
            if (ImGui.DragFloatRange2(label, ref currentMin, ref currentMax))
            {
                onChanged(currentMin, currentMax);
            }
        }

        public static void DragFloatRange2(string label, ref float currentMin, ref float currentMax, float speed,
            float min, float max, string format, string formatMax, ImGuiSliderFlags flags,
            Action<float, float> onChanged)
        {
            if (ImGui.DragFloatRange2(label, ref currentMin, ref currentMax, speed, min, max, format, formatMax, flags))
            {
                onChanged(currentMin, currentMax);
            }
        }

        public static void DragInt(string label, ref int value, Action<int> onChanged)
        {
            if (ImGui.DragInt(label, ref value))
            {
                onChanged(value);
            }
        }

        public static void DragInt(string label, ref int value, float speed, int min, int max, string format,
            ImGuiSliderFlags flags, Action<int> onChanged)
        {
            if (ImGui.DragInt(label, ref value, speed, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        /// <summary>
        /// <paramref name="value"/> is the first of two adjacent ints, so there is no
        /// single value to hand back — read the array in the callback.
        /// </summary>
        public static void DragInt2(string label, ref int value, Action onChanged)
        {
            if (ImGui.DragInt2(label, ref value))
            {
                onChanged();
            }
        }

        public static void DragInt2(string label, ref int value, float speed, int min, int max, string format,
            ImGuiSliderFlags flags, Action onChanged)
        {
            if (ImGui.DragInt2(label, ref value, speed, min, max, format, flags))
            {
                onChanged();
            }
        }

        public static void DragInt3(string label, ref int value, Action onChanged)
        {
            if (ImGui.DragInt3(label, ref value))
            {
                onChanged();
            }
        }

        public static void DragInt3(string label, ref int value, float speed, int min, int max, string format,
            ImGuiSliderFlags flags, Action onChanged)
        {
            if (ImGui.DragInt3(label, ref value, speed, min, max, format, flags))
            {
                onChanged();
            }
        }

        public static void DragInt4(string label, ref int value, Action onChanged)
        {
            if (ImGui.DragInt4(label, ref value))
            {
                onChanged();
            }
        }

        public static void DragInt4(string label, ref int value, float speed, int min, int max, string format,
            ImGuiSliderFlags flags, Action onChanged)
        {
            if (ImGui.DragInt4(label, ref value, speed, min, max, format, flags))
            {
                onChanged();
            }
        }

        public static void DragIntRange2(string label, ref int currentMin, ref int currentMax,
            Action<int, int> onChanged)
        {
            if (ImGui.DragIntRange2(label, ref currentMin, ref currentMax))
            {
                onChanged(currentMin, currentMax);
            }
        }

        public static void DragIntRange2(string label, ref int currentMin, ref int currentMax, float speed, int min,
            int max, string format, string formatMax, ImGuiSliderFlags flags, Action<int, int> onChanged)
        {
            if (ImGui.DragIntRange2(label, ref currentMin, ref currentMax, speed, min, max, format, formatMax, flags))
            {
                onChanged(currentMin, currentMax);
            }
        }

        #endregion // Drag

        #region Slider

        public static void SliderFloat(string label, ref float value, float min, float max, Action<float> onChanged)
        {
            if (ImGui.SliderFloat(label, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat(string label, ref float value, float min, float max, string format,
            ImGuiSliderFlags flags, Action<float> onChanged)
        {
            if (ImGui.SliderFloat(label, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat2(string label, ref Vector2 value, float min, float max,
            Action<Vector2> onChanged)
        {
            if (ImGui.SliderFloat2(label, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat2(string label, ref Vector2 value, float min, float max, string format,
            ImGuiSliderFlags flags, Action<Vector2> onChanged)
        {
            if (ImGui.SliderFloat2(label, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat3(string label, ref Vector3 value, float min, float max,
            Action<Vector3> onChanged)
        {
            if (ImGui.SliderFloat3(label, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat3(string label, ref Vector3 value, float min, float max, string format,
            ImGuiSliderFlags flags, Action<Vector3> onChanged)
        {
            if (ImGui.SliderFloat3(label, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat4(string label, ref Vector4 value, float min, float max,
            Action<Vector4> onChanged)
        {
            if (ImGui.SliderFloat4(label, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void SliderFloat4(string label, ref Vector4 value, float min, float max, string format,
            ImGuiSliderFlags flags, Action<Vector4> onChanged)
        {
            if (ImGui.SliderFloat4(label, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void SliderInt(string label, ref int value, int min, int max, Action<int> onChanged)
        {
            if (ImGui.SliderInt(label, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void SliderInt(string label, ref int value, int min, int max, string format,
            ImGuiSliderFlags flags, Action<int> onChanged)
        {
            if (ImGui.SliderInt(label, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        /// <summary>
        /// <paramref name="value"/> is the first of two adjacent ints, so there is no
        /// single value to hand back — read the array in the callback.
        /// </summary>
        public static void SliderInt2(string label, ref int value, int min, int max, Action onChanged)
        {
            if (ImGui.SliderInt2(label, ref value, min, max))
            {
                onChanged();
            }
        }

        public static void SliderInt2(string label, ref int value, int min, int max, string format,
            ImGuiSliderFlags flags, Action onChanged)
        {
            if (ImGui.SliderInt2(label, ref value, min, max, format, flags))
            {
                onChanged();
            }
        }

        public static void SliderInt3(string label, ref int value, int min, int max, Action onChanged)
        {
            if (ImGui.SliderInt3(label, ref value, min, max))
            {
                onChanged();
            }
        }

        public static void SliderInt3(string label, ref int value, int min, int max, string format,
            ImGuiSliderFlags flags, Action onChanged)
        {
            if (ImGui.SliderInt3(label, ref value, min, max, format, flags))
            {
                onChanged();
            }
        }

        public static void SliderInt4(string label, ref int value, int min, int max, Action onChanged)
        {
            if (ImGui.SliderInt4(label, ref value, min, max))
            {
                onChanged();
            }
        }

        public static void SliderInt4(string label, ref int value, int min, int max, string format,
            ImGuiSliderFlags flags, Action onChanged)
        {
            if (ImGui.SliderInt4(label, ref value, min, max, format, flags))
            {
                onChanged();
            }
        }

        public static void SliderAngle(string label, ref float radians, Action<float> onChanged)
        {
            if (ImGui.SliderAngle(label, ref radians))
            {
                onChanged(radians);
            }
        }

        public static void SliderAngle(string label, ref float radians, float degreesMin, float degreesMax,
            string format, ImGuiSliderFlags flags, Action<float> onChanged)
        {
            if (ImGui.SliderAngle(label, ref radians, degreesMin, degreesMax, format, flags))
            {
                onChanged(radians);
            }
        }

        public static void VSliderFloat(string label, Vector2 size, ref float value, float min, float max,
            Action<float> onChanged)
        {
            if (ImGui.VSliderFloat(label, size, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void VSliderFloat(string label, Vector2 size, ref float value, float min, float max,
            string format, ImGuiSliderFlags flags, Action<float> onChanged)
        {
            if (ImGui.VSliderFloat(label, size, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        public static void VSliderInt(string label, Vector2 size, ref int value, int min, int max,
            Action<int> onChanged)
        {
            if (ImGui.VSliderInt(label, size, ref value, min, max))
            {
                onChanged(value);
            }
        }

        public static void VSliderInt(string label, Vector2 size, ref int value, int min, int max, string format,
            ImGuiSliderFlags flags, Action<int> onChanged)
        {
            if (ImGui.VSliderInt(label, size, ref value, min, max, format, flags))
            {
                onChanged(value);
            }
        }

        #endregion // Slider

        #region Color

        public static void ColorEdit3(string label, ref Vector3 color, Action<Vector3> onChanged)
        {
            if (ImGui.ColorEdit3(label, ref color))
            {
                onChanged(color);
            }
        }

        public static void ColorEdit3(string label, ref Vector3 color, ImGuiColorEditFlags flags,
            Action<Vector3> onChanged)
        {
            if (ImGui.ColorEdit3(label, ref color, flags))
            {
                onChanged(color);
            }
        }

        public static void ColorEdit4(string label, ref Vector4 color, Action<Vector4> onChanged)
        {
            if (ImGui.ColorEdit4(label, ref color))
            {
                onChanged(color);
            }
        }

        public static void ColorEdit4(string label, ref Vector4 color, ImGuiColorEditFlags flags,
            Action<Vector4> onChanged)
        {
            if (ImGui.ColorEdit4(label, ref color, flags))
            {
                onChanged(color);
            }
        }

        public static void ColorPicker3(string label, ref Vector3 color, Action<Vector3> onChanged)
        {
            if (ImGui.ColorPicker3(label, ref color))
            {
                onChanged(color);
            }
        }

        public static void ColorPicker3(string label, ref Vector3 color, ImGuiColorEditFlags flags,
            Action<Vector3> onChanged)
        {
            if (ImGui.ColorPicker3(label, ref color, flags))
            {
                onChanged(color);
            }
        }

        public static void ColorPicker4(string label, ref Vector4 color, Action<Vector4> onChanged)
        {
            if (ImGui.ColorPicker4(label, ref color))
            {
                onChanged(color);
            }
        }

        public static void ColorPicker4(string label, ref Vector4 color, ImGuiColorEditFlags flags,
            Action<Vector4> onChanged)
        {
            if (ImGui.ColorPicker4(label, ref color, flags))
            {
                onChanged(color);
            }
        }

        #endregion // Color

        #region Separator

        public static void SeparatorText(string label, Action content)
        {
            ImGui.SeparatorText(label);
            content();
        }

        #endregion // Separator
    }
}
