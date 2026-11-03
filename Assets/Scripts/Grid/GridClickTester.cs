using UnityEngine;

public class GridClickTester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = GridManager.Instance.WorldToGrid(mouseWorld);
            Tile tile = GridManager.Instance.GetTileAt(gridPos);

            if (tile != null)
            {
                Debug.Log($"Clicked tile ({gridPos.x}, {gridPos.y}): Terrain={tile.Terrain}, Resource={tile.Resource}");
            }
            else
            {
                Debug.Log($"Clicked outside grid: ({gridPos.x}, {gridPos.y})");
            }
        }
    }
}