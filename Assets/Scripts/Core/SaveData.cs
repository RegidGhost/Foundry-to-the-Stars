using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Version = 1;
    public List<BuildingSaveData> Buildings = new List<BuildingSaveData>();
}

[Serializable]
public class BuildingSaveData
{
    public string Type;
    public int X;
    public int Y;
    public int Rotation;
    public List<ItemStackSaveData> Inventory = new List<ItemStackSaveData>();
}

[Serializable]
public class ItemStackSaveData
{
    public string ItemId;
    public int Count;
    public int SlotIndex;
}