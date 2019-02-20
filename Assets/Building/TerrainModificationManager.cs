using System;
using UnityEngine;

public class TerrainModificationManager : AbstractPrimaryActionManager {
    Terrain terr; // terrain to modify
    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    public int size = 5; // the diameter of terrain portion that will raise under the game object
    public float desiredHeight = 0;
    public float currentHeight = 0f;
    public float increment = 0.02f;

    public bool raiseLevel = true;
    public bool lowerLevel = true;
    private bool canModifyTerrain = false;

    void Start()
    {
        //supports only one active terrain at at time
        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapWidth;
        hmHeight = terr.terrainData.heightmapHeight;

    }

    void Update()
    {
        if (!canModifyTerrain)
            return;
        currentHeight = CurrentHeight();
    }

    private Vector2 GetCoordInTerrain(Vector3 position)
    {
        Vector3 tempCoord = (position - terr.gameObject.transform.position);
        Vector3 coord;
        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        // get the position of the terrain heightmap where this game object is
        return new Vector2((int)(coord.x * hmWidth), (int)(coord.z * hmHeight));
    }

    private float CurrentHeight()
    {
        Vector2 coordInTerrrain = GetCoordInTerrain(currentPlayer().position);
        return terr.terrainData.GetHeights((int)coordInTerrrain.x, (int)coordInTerrrain.y, 1, 1)[0, 0];
    }

    public float CurrentHeightInMeter()
    {
        return toMeter(CurrentHeight());
    }

    internal  float DesiredHeightInMeter()
    {
        return toMeter(desiredHeight);
    }

    private float toMeter(float height)
    {
        return (float)Math.Round(Convert.ToDouble(height * 1000), 2);
    }

    protected override void ActivatePrimaryAction(object obj)
    {
        base.ActivatePrimaryAction(obj);
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == GetCapability())
        {
            canModifyTerrain = true;
            EventManager.StartListening(Events.OnIncrease, increaseDesiredHeight);
            EventManager.StartListening(Events.OnDecrease, decreaseDesiredHeight);
            currentHeight = CurrentHeight();
            desiredHeight = CurrentHeight();
        }
    }

    protected override void DeactivatePrimaryAction(object obj)
    {
        base.DeactivatePrimaryAction(obj);
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == GetCapability())
        {
            canModifyTerrain = false;
            EventManager.StopListening(Events.OnIncrease, increaseDesiredHeight);
            EventManager.StopListening(Events.OnDecrease, decreaseDesiredHeight);
        }
    }

    public void increaseDesiredHeight(){desiredHeight += increment;}

    public void decreaseDesiredHeight(){desiredHeight = Mathf.Clamp(increment,0,desiredHeight);}

    protected override void ExecutePrimaryAction()
    {
        if (canModifyTerrain)
        {

            // we set an offset so that all the raising terrain is under this game object
            int offset = size / 2;

            // get the heights of the terrain under this game object
            Vector2 coordInTerrrain = GetCoordInTerrain(currentPlayer().position);
            float[,] heights = terr.terrainData.GetHeights((int)coordInTerrrain.x - offset, (int)coordInTerrrain.y - offset, size, size);

            // we set each sample of the terrain in the size to the desired height

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float height = heights[i, j];
                    float terrainHeight = heights[i, j];
                    if (raiseLevel && height < desiredHeight)
                    {
                        heights[i, j] = Mathf.Clamp(terrainHeight + increment, terrainHeight, desiredHeight);
                    }
                    else if (lowerLevel && height > desiredHeight)
                    {
                        heights[i, j] = Mathf.Clamp(terrainHeight - increment, desiredHeight, terrainHeight);

                    }
                }
            }
            terr.terrainData.SetHeights((int)coordInTerrrain.x - offset, (int)coordInTerrrain.y - offset, heights);
        }
    }

    protected override Capability GetCapability()
    {
        return Capability.Digging;
    }
}
