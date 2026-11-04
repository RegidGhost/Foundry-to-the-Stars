using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance { get; private set; }

    private float _refreshTimer;
    private const float RefreshInterval = 1f; // Пересчитывать раз в секунду

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        // Автоматически обновляем сети каждую секунду, 
        // чтобы учитывать сгоревший или добавленный уголь
        _refreshTimer += Time.deltaTime;
        if (_refreshTimer >= RefreshInterval)
        {
            _refreshTimer = 0f;
            RebuildNetworks();
        }
    }

    public void RebuildNetworks()
    {
        var allBuildings = BuildingRegistry.All;
        
        foreach (var b in allBuildings) b.IsPowered = false;

        var poles = new List<PowerPole>();
        var generators = new List<CoalGenerator>();
        var consumers = new List<BuildingBase>();

        foreach (var b in allBuildings)
        {
            if (b is PowerPole p) poles.Add(p);
            if (b is CoalGenerator g) generators.Add(g);
            if (b.Data.PowerConsumption > 0) consumers.Add(b);
        }

        var poweredPoles = new HashSet<PowerPole>();
        foreach (var pole in poles)
        {
            foreach (var gen in generators)
            {
                if (gen.HasFuel() && Vector2Int.Distance(pole.GridPosition, gen.GridPosition) <= gen.Radius)
                {
                    poweredPoles.Add(pole);
                    break;
                }
            }
        }

        bool changed = true;
        while (changed)
        {
            changed = false;
            foreach (var pole in poles)
            {
                if (poweredPoles.Contains(pole)) continue;
                foreach (var pPole in poweredPoles)
                {
                    if (Vector2Int.Distance(pole.GridPosition, pPole.GridPosition) <= pPole.Radius)
                    {
                        poweredPoles.Add(pole);
                        changed = true;
                        break;
                    }
                }
            }
        }

        foreach (var consumer in consumers)
        {
            foreach (var pPole in poweredPoles)
            {
                if (Vector2Int.Distance(consumer.GridPosition, pPole.GridPosition) <= pPole.Radius)
                {
                    consumer.IsPowered = true;
                    break;
                }
            }
        }
    }
}