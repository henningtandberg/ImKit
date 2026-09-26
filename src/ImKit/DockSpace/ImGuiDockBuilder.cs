using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace ImKit.DockSpace;

/// <summary>
/// P/Invoke bindings for Dear ImGui's DockBuilder API, which ImGui.NET does not expose.
/// Resolves against the native <c>cimgui</c> library that ImGui.NET already ships, so no
/// extra native dependency is introduced.
/// </summary>
public static class ImGuiDockBuilder
{
    private const string LibName = "cimgui";

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void igDockBuilderRemoveNode(uint node_id);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern uint igDockBuilderAddNode(uint node_id, int flags);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void igDockBuilderSetNodeSize(uint node_id, Vector2 sz);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern uint igDockBuilderSplitNode(uint node_id, int split_dir, float size_ratio_for_node_at_dir,
        out uint out_id_at_dir, out uint out_id_at_opposite_dir);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void igDockBuilderFinish(uint node_id);

    public static void RemoveNode(uint nodeId) => igDockBuilderRemoveNode(nodeId);

    public static void AddNode(uint nodeId, int flags = 0) => igDockBuilderAddNode(nodeId, flags);

    public static void SetNodeSize(uint nodeId, Vector2 size) => igDockBuilderSetNodeSize(nodeId, size);

    public static void Finish(uint nodeId) => igDockBuilderFinish(nodeId);

    public static void SplitNode(uint nodeId, ImGuiDir dir, float ratio, out uint outAtDir, out uint outOpposite)
        => igDockBuilderSplitNode(nodeId, (int)dir, ratio, out outAtDir, out outOpposite);
}
