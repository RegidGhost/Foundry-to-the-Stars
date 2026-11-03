using UnityEngine;

public class Tile
{
    public TerrainType Terrain;
    public ResourceType Resource;
    public GameObject Building; // пока GameObject, позже заменим на BuildingBase

    public Tile(TerrainType terrain = TerrainType.Grass, ResourceType resource = ResourceType.None)
    {
        Terrain = terrain;
        Resource = resource;
        Building = null;
    }

    public bool IsFree => Building == null;
}