using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
    public BuildingData Data { get; private set; }
    public Vector2Int GridPosition { get; private set; }
    public int Rotation { get; private set; }
    public Inventory Inventory { get; protected set; }
    public bool IsPowered { get; set; } = true;

    protected virtual int InventoryCapacity => 4;

    public virtual void Initialize(BuildingData data, Vector2Int gridPos, int rotation)
    {
        Data = data;
        GridPosition = gridPos;
        Rotation = rotation;
        transform.position = GridManager.Instance.GridToWorld(gridPos, data.Size);
        transform.rotation = Quaternion.Euler(0, 0, rotation * 90);
        Inventory = new Inventory(InventoryCapacity);
    }

    protected virtual void OnEnable() => GameManager.OnTick += HandleTick;
    protected virtual void OnDisable() => GameManager.OnTick -= HandleTick;

    private void HandleTick(float dt) { if (Data != null) OnTick(dt); }

    public virtual void OnTick(float dt) { }
    public virtual void OnPlace() { }
    public virtual void OnRemove() { }
    public virtual bool TryAddItem(ItemData item, int count = 1) => Inventory.TryAdd(item, count);

    public virtual ItemStack TakeItem()
    {
        for (int i = 0; i < Inventory.Capacity; i++)
            if (!Inventory.Slots[i].IsEmpty)
            {
                var it = new ItemStack(Inventory.Slots[i].Item, 1);
                Inventory.TryRemove(it.Item, 1);
                return it;
            }
        return default;
    }
}