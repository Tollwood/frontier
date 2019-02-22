using UnityEngine;

public class PlowingManager : AbstractPrimaryActionManager
{
    public int textureIndex = 3;
    public int size = 1;

    protected override void ExecutePrimaryAction()
    {
        Vector3 WorldPos = currentPlayer().position + currentPlayer().forward;
        float[,,] splatmapData = TerrainHelper.GetSplatMapAt(WorldPos, size);
        UpdateTerrain(splatmapData);
        TerrainHelper.SetAlphamapsAt(WorldPos, size, splatmapData);
    }

    private void UpdateTerrain(float[,,] splatmapData)
    {
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
    }

    protected override Capability GetCapability()
    {
        return Capability.Plowing;
    }
}