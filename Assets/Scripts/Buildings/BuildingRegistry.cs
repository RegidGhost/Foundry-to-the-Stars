using System.Collections.Generic;

public static class BuildingRegistry
{
    public static readonly List<BuildingBase> All = new List<BuildingBase>();
    public static void Register(BuildingBase b) { if (b != null && !All.Contains(b)) All.Add(b); }
    public static void Unregister(BuildingBase b) => All.Remove(b);
    public static void Clear() => All.Clear();
}