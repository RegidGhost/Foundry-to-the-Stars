using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    private BuildingData _selected;
    private GameObject _preview;
    private int _rotation;

    private void Update()
    {
        if (_selected == null || _preview == null) return;

        Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition); m.z = 0;
        Vector2Int gp = GridManager.Instance.WorldToGrid(m);
        _preview.transform.position = GridManager.Instance.GridToWorld(gp, _selected.Size);

        if (Input.GetKeyDown(KeyCode.R))
        {
            _rotation = (_rotation + 1) % 4;
            _preview.transform.rotation = Quaternion.Euler(0, 0, _rotation * 90);
        }

        bool canPlace = CanPlace(gp);
        var col = canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        foreach (var r in _preview.GetComponentsInChildren<Renderer>()) r.material.color = col;

        if (Input.GetMouseButtonDown(0)) TryPlace(gp);
        if (Input.GetMouseButtonDown(1)) TryRemove(gp);
    }

    public void SelectBuilding(BuildingData data)
    {
        _selected = data;
        if (_preview != null) Destroy(_preview);
        if (data == null || data.Prefab == null) return;

        _preview = Instantiate(data.Prefab);
        _preview.name = "Preview";
        foreach (var r in _preview.GetComponentsInChildren<Renderer>())
        {
            r.material = new Material(r.material);
            r.material.color = new Color(0, 1, 0, 0.5f);
        }
    }

    private bool CanPlace(Vector2Int gp)
    {
        for (int x = 0; x < _selected.Size.x; x++)
            for (int y = 0; y < _selected.Size.y; y++)
            {
                var t = GridManager.Instance.GetTileAt(gp + new Vector2Int(x, y));
                if (t == null || !t.IsFree) return false;
            }
        return true;
    }

    private void TryPlace(Vector2Int gp)
    {
        if (!CanPlace(gp)) return;

        var obj = Instantiate(_selected.Prefab, GridManager.Instance.GridToWorld(gp), Quaternion.Euler(0, 0, _rotation * 90));
        var b = obj.GetComponent<BuildingBase>();
        if (b == null) { Destroy(obj); return; }

        b.Initialize(_selected, gp, _rotation);
        for (int x = 0; x < _selected.Size.x; x++)
            for (int y = 0; y < _selected.Size.y; y++)
            {
                var t = GridManager.Instance.GetTileAt(gp + new Vector2Int(x, y));
                if (t != null) t.Building = obj;
            }

        BuildingRegistry.Register(b);
        b.OnPlace();

        if (b is BeltSegment belt)
        {
            BeltManager.Instance.Register(belt);
            BeltManager.Instance.RelinkAll();
            PowerManager.Instance.RebuildNetworks();
        }
    }

    private void TryRemove(Vector2Int gp)
    {
        var t = GridManager.Instance.GetTileAt(gp);
        if (t?.Building == null) return;
        var b = t.Building.GetComponent<BuildingBase>();
        if (b == null) return;

        b.OnRemove();
        Vector2Int o = b.GridPosition;
        for (int x = 0; x < b.Data.Size.x; x++)
            for (int y = 0; y < b.Data.Size.y; y++)
            {
                var tt = GridManager.Instance.GetTileAt(o + new Vector2Int(x, y));
                if (tt != null) tt.Building = null;
            }

        if (b is BeltSegment belt) BeltManager.Instance.Unregister(belt);
        BuildingRegistry.Unregister(b);
        Destroy(b.gameObject);
        BeltManager.Instance.RelinkAll();
        PowerManager.Instance.RebuildNetworks();
    }
}