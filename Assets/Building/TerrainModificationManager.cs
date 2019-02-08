using System;
using UnityEngine;

public class TerrainModificationManager : Singleton<TerrainModificationManager>
{
    Terrain terr; // terrain to modify
    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    Vector2 coordInTerrrain;

    public int size = 5; // the diameter of terrain portion that will raise under the game object
    public float desiredHeight = 0;
    public float currentHeight = 0f;
    public float increment = 0.02f;

    public bool raiseLevel = true;
    public bool lowerLevel = true;
    private bool canModifyTerrain = false;
    private GameObject currentPlayer;

    void Start()
    {
        //supports only one active terrain at at time
        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapWidth;
        hmHeight = terr.terrainData.heightmapHeight;
        EventManager.StartListening(Events.OnPlayerChanged, OnPlayerChanged);

       EventManager.StartListening(Events.OnItemEquip, ActivateDigging);
       EventManager.StartListening(Events.OnItemUnEquip, DeactivateDigging);
    }

    void Update()
    {
        if (!canModifyTerrain)
            return;
        UpdateCurrentHeight();
    }

    private void UpdateCurrentHeight()
    {
        if (currentPlayer == null)
        {
            currentPlayer = PlayerManager.Instance.CurrentPlayer();
        }

        coordInTerrrain = GetCoordInTerrain(currentPlayer.transform.position);
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

    public float GetHeight(Vector3 position)
    {
        //Vector2 coord = GetCoordInTerrain(position);
        //return terr.terrainData.GetHeights((int)coord.x, (int)coord.y, 1, 1)[0, 0];
        return terr.SampleHeight(position);
    }


    private void dig()
    {
        if (canModifyTerrain)
        {

            // we set an offset so that all the raising terrain is under this game object
            int offset = size / 2;

            // get the heights of the terrain under this game object
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

    private float CurrentHeight()
    {
        return terr.terrainData.GetHeights((int)coordInTerrrain.x, (int)coordInTerrrain.y, 1, 1)[0, 0];
    }

    public float CurrentHeightInMeter()
    {
        return toMeter(CurrentHeight());
    }

    internal float DesiredHeightInMeter()
    {
        return toMeter(desiredHeight);
    }

    private float toMeter(float height)
    {
        return (float)Math.Round(Convert.ToDouble(height * 1000), 2);
    }

    public void ActivateDigging(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.Digging)
        {
            canModifyTerrain = true;
            EventManager.StartListening(Events.OnExecutePrimaryAction, dig);
            EventManager.StartListening(Events.OnIncrease, increaseDesiredHeight);
            EventManager.StartListening(Events.OnDecrease, decreaseDesiredHeight);
            UpdateCurrentHeight();
            desiredHeight = CurrentHeight();
        }
    }

    public void DeactivateDigging(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.Digging)
        {
            canModifyTerrain = false;
            EventManager.StopListening(Events.OnExecutePrimaryAction, dig); 
            EventManager.StopListening(Events.OnIncrease, increaseDesiredHeight);
            EventManager.StopListening(Events.OnDecrease, decreaseDesiredHeight);
        }
 
    }

    public void OnPlayerChanged(System.Object player)
    {
        this.currentPlayer = (GameObject)player;
        // update canModifyTerrain according to currentPlayer
    }


    public void increaseDesiredHeight()
    {
        desiredHeight += increment;
    }

    public void decreaseDesiredHeight()
    {
        desiredHeight = Mathf.Clamp(increment,0,desiredHeight);
    }
}
