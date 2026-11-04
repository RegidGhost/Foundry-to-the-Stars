using UnityEngine;

[CreateAssetMenu(menuName = "Foundry/Building Data")]
public class BuildingData : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public Vector2Int Size = Vector2Int.one;
    public GameObject Prefab;
    public Sprite Icon;
    public ItemStack[] Cost;
    public float PowerConsumption;
    public float PowerProduction;
}