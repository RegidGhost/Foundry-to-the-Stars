using UnityEngine;

public class Inserter : BuildingBase
{
    [SerializeField] private float moveInterval = 0.5f;
    private float _timer;

    public override void OnTick(float dt)
    {
        _timer += dt;
        if (_timer < moveInterval) return;
        _timer -= moveInterval;

        Vector2Int back = -Dir(Rotation), front = Dir(Rotation);
        var backTile = GridManager.Instance.GetTileAt(GridPosition + back);
        var frontTile = GridManager.Instance.GetTileAt(GridPosition + front);
        if (backTile?.Building == null || frontTile?.Building == null) return;

        var src = backTile.Building.GetComponent<BuildingBase>();
        var dst = frontTile.Building.GetComponent<BuildingBase>();
        if (src == null || dst == null) return;

        var item = src.TakeItem();
        if (item.IsEmpty) return;
        if (!dst.TryAddItem(item.Item, item.Count)) src.TryAddItem(item.Item, item.Count);
    }

    private Vector2Int Dir(int d) => d switch
    {
        0 => Vector2Int.right, 1 => Vector2Int.down, 2 => Vector2Int.left, 3 => Vector2Int.up, _ => Vector2Int.right
    };
}