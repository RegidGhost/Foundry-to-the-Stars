using UnityEngine;

public class Tile
{
    public TerrainType Terrain;
    public ResourceType Resource;
    public GameObject Building;

    public Tile(TerrainType terrain = TerrainType.Grass, ResourceType resource = ResourceType.None)
    {
        Terrain = terrain;
        Resource = resource;
        Building = null;
    }

    public bool IsFree => Building == null;
}