using System.Collections.Generic;
using UnityEngine;

public class DataRegistry : MonoBehaviour
{
    public static Dictionary<string, ItemData> Items = new Dictionary<string, ItemData>();
    public static Dictionary<string, BuildingData> Buildings = new Dictionary<string, BuildingData>();

    private void Awake()
    {
        Items.Clear();
        foreach (var item in Resources.LoadAll<ItemData>("Items"))
            if (!string.IsNullOrEmpty(item.Id)) Items[item.Id] = item;

        Buildings.Clear();
        foreach (var b in Resources.LoadAll<BuildingData>("Buildings"))
            if (!string.IsNullOrEmpty(b.Id)) Buildings[b.Id] = b;

        Debug.Log($"[DataRegistry] Loaded {Items.Count} items, {Buildings.Count} buildings.");
    }
}