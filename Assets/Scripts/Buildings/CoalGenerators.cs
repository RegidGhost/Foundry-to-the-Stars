using UnityEngine;

public class CoalGenerator : BuildingBase
{
    [Header("Параметры генератора")]
    [SerializeField] private ItemData fuelType; // Сюда перетащим Coal
    [SerializeField] private float fuelBurnTime = 10f; // Секунд на 1 уголь
    public float PowerOutput = 100f; // Сколько энергии даёт
    public float Radius = 5f; // Радиус, в котором он питает ЛЭП

    private float _timer;

    public bool HasFuel()
    {
        return Inventory.Has(fuelType, 1);
    }

    public override bool TryAddItem(ItemData item, int count = 1)
    {
        // Принимает только топливо (уголь)
        if (item != fuelType) return false;
        return Inventory.TryAdd(item, count);
    }

    public override void OnTick(float dt)
    {
        if (fuelType == null) return;

        // Если есть уголь, генератор работает и тратит топливо
        if (Inventory.Has(fuelType, 1))
        {
            _timer += dt;
            if (_timer >= fuelBurnTime)
            {
                _timer -= fuelBurnTime;
                Inventory.TryRemove(fuelType, 1);
            }
        }
    }
}