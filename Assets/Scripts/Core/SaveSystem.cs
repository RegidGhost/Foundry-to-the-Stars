using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string SavePath => Path.Combine(Application.persistentDataPath, "foundry_save.json");

    public static void Save()
    {
        SaveData data = new SaveData();

        foreach (var building in BuildingRegistry.All)
        {
            if (building == null || building.Data == null) continue;

            BuildingSaveData bd = new BuildingSaveData
            {
                Type = building.Data.Id,
                X = building.GridPosition.x,
                Y = building.GridPosition.y,
                Rotation = building.Rotation
            };

            // Сохраняем инвентарь (если есть)
            if (building.Inventory != null)
            {
                for (int i = 0; i < building.Inventory.Capacity; i++)
                {
                    var slot = building.Inventory.Slots[i];
                    if (!slot.IsEmpty)
                    {
                        bd.Inventory.Add(new ItemStackSaveData
                        {
                            ItemId = slot.Item.Id,
                            Count = slot.Count,
                            SlotIndex = i
                        });
                    }
                }
            }

            data.Buildings.Add(bd);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveSystem] Игра сохранена! Построек: {data.Buildings.Count} → {SavePath}");
    }

    public static void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveSystem] Файл сохранения не найден!");
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        if (data == null) return;

        // 1. Удаляем все текущие здания со сцены
        foreach (var b in BuildingRegistry.All.ToArray())
        {
            if (b != null) Object.Destroy(b.gameObject);
        }
        BuildingRegistry.Clear();
        BeltManager.Instance.ClearAll();

        // 2. Очищаем сетку
        for (int x = 0; x < GridManager.Instance.Width; x++)
            for (int y = 0; y < GridManager.Instance.Height; y++)
                GridManager.Instance.Tiles[x, y].Building = null;

        // 3. Восстанавливаем здания
        foreach (var bd in data.Buildings)
        {
            if (!DataRegistry.Buildings.TryGetValue(bd.Type, out var bData))
            {
                Debug.LogWarning($"[SaveSystem] Неизвестное здание: {bd.Type}");
                continue;
            }

            Vector2Int pos = new Vector2Int(bd.X, bd.Y);
            GameObject obj = Object.Instantiate(
                bData.Prefab,
                GridManager.Instance.GridToWorld(pos, bData.Size),
                Quaternion.Euler(0, 0, bd.Rotation * 90)
            );

            var building = obj.GetComponent<BuildingBase>();
            if (building == null) { Object.Destroy(obj); continue; }

            building.Initialize(bData, pos, bd.Rotation);

            // Занимаем тайлы
            for (int x = 0; x < bData.Size.x; x++)
                for (int y = 0; y < bData.Size.y; y++)
                {
                    var tile = GridManager.Instance.GetTileAt(pos + new Vector2Int(x, y));
                    if (tile != null) tile.Building = obj;
                }

            // Восстанавливаем предметы внутри
            foreach (var inv in bd.Inventory)
            {
                if (DataRegistry.Items.TryGetValue(inv.ItemId, out var item))
                {
                    if (inv.SlotIndex >= 0 && inv.SlotIndex < building.Inventory.Capacity)
                        building.Inventory.Slots[inv.SlotIndex] = new ItemStack(item, inv.Count);
                }
            }

            BuildingRegistry.Register(building);
            if (building is BeltSegment belt) BeltManager.Instance.Register(belt);
        }

        // 4. Связываем бельты заново
        BeltManager.Instance.RelinkAll();
        Debug.Log($"[SaveSystem] Игра загружена! Построек: {data.Buildings.Count}");
    }
}