using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private BuildingPlacer placer;
    [SerializeField] private BuildingData[] buildings;

    private int _selected;

    private void Start() { if (buildings.Length > 0) Select(0); }

    private void Update()
    {
        for (int i = 0; i < buildings.Length && i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) Select(i);
    }

    private void Select(int i)
    {
        _selected = i;
        placer.SelectBuilding(buildings[i]);
    }

    private void OnGUI()
    {
        if (buildings == null || buildings.Length == 0) return;
        float s = 70, sp = 6;
        float total = buildings.Length * s + (buildings.Length - 1) * sp;
        float x0 = (Screen.width - total) * 0.5f;
        float y = Screen.height - s - 20;

        for (int i = 0; i < buildings.Length; i++)
        {
            var r = new Rect(x0 + i * (s + sp), y, s, s);
            GUI.color = i == _selected ? Color.yellow : Color.white;
            GUI.Box(r, $"{i + 1}\n{buildings[i].DisplayName}");
        }
        GUI.color = Color.white;
    }
}