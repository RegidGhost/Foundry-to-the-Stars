using UnityEngine;

[CreateAssetMenu(menuName = "Foundry/Item")]
public class ItemData : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public Sprite Icon;
    public Color Tint = Color.white;
    public ItemType Type = ItemType.Solid;
}

public enum ItemType { Solid, Fluid }