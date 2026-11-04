using System;
using UnityEngine;

[Serializable]
public struct ItemStack
{
    public ItemData Item;
    public int Count;

    public ItemStack(ItemData item, int count) { Item = item; Count = count; }
    public bool IsEmpty => Item == null || Count <= 0;
}