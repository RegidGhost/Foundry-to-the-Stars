using UnityEngine;

public class Miner : BuildingBase
{
    [SerializeField] private ItemData oreType;
    [SerializeField] private float mineInterval = 2f;
    private float _timer;

    public override bool TryAddItem(ItemData item, int count = 1) => false;

    public override void OnTick(float dt)
    {
        if (oreType == null) return;
        _timer += dt;
        if (_timer >= mineInterval) { _timer -= mineInterval; TryOutput(); }
    }

    private void TryOutput()
    {
        Vector2Int[] dirs = { Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up };
        foreach (var dir in dirs)
        {
            var tile = GridManager.Instance.GetTileAt(GridPosition + dir);
            if (tile?.Building == null) continue;
            var belt = tile.Building.GetComponent<BeltSegment>();
            if (belt != null && belt.TryAddItem(oreType, 1)) return;
            var target = tile.Building.GetComponent<BuildingBase>();
            if (target != null && target.TryAddItem(oreType, 1)) return;
        }
    }
}