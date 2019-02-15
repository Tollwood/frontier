using UnityEngine;

public class PlowingManager : AbstractPrimaryActionManager
{
    public int textureIndex = 3;
    public int size = 1;

    Terrain terrain;
    TerrainData terrainData;
    Vector3 terrainPos;

    private void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
    }

    protected override void ExecutePrimaryAction()
    {
        Vector3 WorldPos = player.position + player.forward;

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        //float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ - (size / 2), 1, size);

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < 1; z++)
            {
                for (int i = 0; i < splatmapData.GetUpperBound(2) + 1; i++)
                {
                    if (textureIndex == i)
                    {
                        splatmapData[x, z, i] = 1;
                    }
                    else
                    {
                        splatmapData[x, z, i] = 0;
                    }

                }
            }
        }



        terrainData.SetAlphamaps(mapX, mapZ - (size / 2), splatmapData);
    }

    protected override Capability GetCapability()
    {
        return Capability.Plowing;
    }
}