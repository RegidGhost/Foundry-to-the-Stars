using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private int width = 64;
    [SerializeField] private int height = 64;

    public int Width => width;
    public int Height => height;
    public Tile[,] Tiles { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        Tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Tiles[x, y] = new Tile();
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height) return null;
        return Tiles[pos.x, pos.y];
    }

    public Vector2Int WorldToGrid(Vector3 worldPos) =>
        new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));

    // 1. Старый метод (для 1х1 зданий и предметов)
    public Vector3 GridToWorld(Vector2Int gridPos) =>
        new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);

    // 2. НОВЫЙ МЕТОД (для 2х2, 3х3 и т.д.)
    public Vector3 GridToWorld(Vector2Int gridPos, Vector2Int size) =>
        new Vector3(gridPos.x + size.x / 2f, gridPos.y + size.y / 2f, 0);
}