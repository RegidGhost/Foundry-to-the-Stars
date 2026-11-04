using UnityEngine;

public class Furnace : BuildingBase
{
    [SerializeField] private ItemData inputOre;
    [SerializeField] private ItemData outputPlate;
    [SerializeField] private float craftTime = 2f;
    private float _timer;

    public override bool TryAddItem(ItemData item, int count = 1)
    {
        if (item != inputOre) return false;
        return Inventory.TryAdd(item, count);
    }

    public override void OnTick(float dt)
    {
        if (inputOre == null || outputPlate == null) return;
        if (Inventory.Has(inputOre, 1))
        {
            _timer += dt;
            if (_timer >= craftTime)
            {
                _timer -= craftTime;
                Inventory.TryRemove(inputOre, 1);
                Inventory.TryAdd(outputPlate, 1);
                TryOutput();
            }
        }
        else _timer = 0;
    }

    private void TryOutput()
    {
        if (!Inventory.Has(outputPlate, 1)) return;
        Vector2Int[] dirs = { Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up };
        foreach (var dir in dirs)
        {
            var tile = GridManager.Instance.GetTileAt(GridPosition + dir);
            if (tile?.Building == null) continue;
            var belt = tile.Building.GetComponent<BeltSegment>();
            if (belt != null && belt.TryAddItem(outputPlate, 1)) { Inventory.TryRemove(outputPlate, 1); return; }
            var target = tile.Building.GetComponent<BuildingBase>();
            if (target != null && target.TryAddItem(outputPlate, 1)) { Inventory.TryRemove(outputPlate, 1); return; }
        }
    }
}