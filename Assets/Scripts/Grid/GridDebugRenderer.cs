using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GridDebugRenderer : MonoBehaviour
{
    [SerializeField] private Color lineColor = new Color(1, 1, 1, 0.1f);

    private void OnDrawGizmos()
    {
        var gm = GridManager.Instance;
        if (gm == null) return;

        Gizmos.color = lineColor;
        for (int x = 0; x <= 64; x++)
        {
            Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, 64, 0));
        }
        for (int y = 0; y <= 64; y++)
        {
            Gizmos.DrawLine(new Vector3(0, y, 0), new Vector3(64, y, 0));
        }
    }
}