using UnityEngine;

public class Inventory
{
    public ItemStack[] Slots { get; private set; }
    public int Capacity => Slots.Length;

    public Inventory(int capacity) { Slots = new ItemStack[Mathf.Max(0, capacity)]; }

    public bool TryAdd(ItemData item, int count = 1)
    {
        if (item == null || count <= 0) return false;
        for (int i = 0; i < Slots.Length; i++)
            if (Slots[i].Item == item) { Slots[i] = new ItemStack(item, Slots[i].Count + count); return true; }
        for (int i = 0; i < Slots.Length; i++)
            if (Slots[i].IsEmpty) { Slots[i] = new ItemStack(item, count); return true; }
        return false;
    }

    public bool TryRemove(ItemData item, int count = 1)
    {
        if (item == null || count <= 0) return false;
        for (int i = 0; i < Slots.Length; i++)
            if (Slots[i].Item == item && Slots[i].Count >= count)
            {
                int rem = Slots[i].Count - count;
                Slots[i] = rem > 0 ? new ItemStack(item, rem) : default;
                return true;
            }
        return false;
    }

    public int CountOf(ItemData item)
    {
        if (item == null) return 0;
        int total = 0;
        foreach (var s in Slots) if (s.Item == item) total += s.Count;
        return total;
    }

    public bool Has(ItemData item, int count = 1) => CountOf(item) >= count;

    public void Print()
    {
        Debug.Log("=== Inventory ===");
        for (int i = 0; i < Slots.Length; i++)
            if (!Slots[i].IsEmpty) Debug.Log($"[{i}] {Slots[i].Item.DisplayName} x{Slots[i].Count}");
    }
}