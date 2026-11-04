using UnityEngine;

public class BeltTestSpawner : MonoBehaviour
{
    [SerializeField] private ItemData testItem;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && BeltManager.Instance.Segments.Count > 0)
            BeltManager.Instance.Segments[0].TryAddItem(testItem, 1);
    }
}