using UnityEngine;

public class PlowingManager : MonoBehaviour
{
    public int textureIndex = 3;
    public int size = 1;

    private Transform player;
    Terrain terrain;
    TerrainData terrainData;
    Vector3 terrainPos;

    private void Start()
    {
        EventManager.StartListening(Events.OnPlayerChanged, (System.Object obj) => player = ((GameObject)obj).transform);
        EventManager.StartListening(Events.OnItemEquip, ActivatePlowing);
        EventManager.StartListening(Events.OnItemUnEquip, DeactivatePlowing);
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
    }

    public void Plow()
    {
        Vector3 WorldPos = player.position + player.forward;

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        //float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX , mapZ - (size / 2), 1, size);

        for(int x = 0; x < size; x++)
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


    private Vector2 GetCoordInTerrain(Vector3 position)
    {
        Terrain terr = Terrain.activeTerrain;
        Vector3 tempCoord = (position - terr.gameObject.transform.position);
        Vector3 coord;
        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        // get the position of the terrain heightmap where this game object is
        return new Vector2((int)(coord.x * terr.terrainData.alphamapWidth), (int)(coord.z * terr.terrainData.alphamapHeight));
    }

    public void ActivatePlowing(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.Plowing)
        {
            EventManager.StartListening(Events.OnExecutePrimaryAction, Plow);
        }
    }
    public void DeactivatePlowing(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.Plowing)
        {
            EventManager.StopListening(Events.OnExecutePrimaryAction, Plow);
        }
    }
}
