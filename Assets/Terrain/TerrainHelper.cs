using System;
using UnityEngine;

public static class TerrainHelper
{
    // reuse in plowing and terrain modification
    public static float[,,] GetSplatMapAt(Vector3 position, int size)
    {

        Terrain terrain = Terrain.activeTerrain;
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        //float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        return terrainData.GetAlphamaps(mapX, mapZ - (size / 2), 1, size);
    }

    public static void SetAlphamapsAt(Vector3 position, int size, float[,,] splatmapData)
    {
        Terrain terrain = Terrain.activeTerrain;
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        terrainData.SetAlphamaps(mapX, mapZ - (size / 2), splatmapData);
    }
}