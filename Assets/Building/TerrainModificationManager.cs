using System;
using UnityEngine;

public class TerrainModificationManager : Singleton<TerrainModificationManager>
{
    Terrain terr; // terrain to modify
    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    int posXInTerrain; // position of the game object in terrain width (x axis)
    int posYInTerrain; // position of the game object in terrain height (z axis)

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
        PlayerManager.Instance.PlayerChanged -= OnPlayerChanged;
        PlayerManager.Instance.PlayerChanged += OnPlayerChanged;
        EquipmentManager.Instance.onEquipedCallback -= ActivateDigging;
        EquipmentManager.Instance.onEquipedCallback += ActivateDigging;
       
        EquipmentManager.Instance.onUnEquipCallback -= DeactivateDigging;
        EquipmentManager.Instance.onUnEquipCallback += DeactivateDigging;
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
        Vector3 tempCoord = (currentPlayer.transform.position - terr.gameObject.transform.position);
        Vector3 coord;
        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        // get the position of the terrain heightmap where this game object is
        posXInTerrain = (int)(coord.x * hmWidth);
        posYInTerrain = (int)(coord.z * hmHeight);
        currentHeight = CurrentHeight();
    }

    private void dig()
    {
        if (canModifyTerrain)
        {

            // we set an offset so that all the raising terrain is under this game object
            int offset = size / 2;

            // get the heights of the terrain under this game object
            float[,] heights = terr.terrainData.GetHeights(posXInTerrain - offset, posYInTerrain - offset, size, size);

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
            terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);
        }
    }

    private float CurrentHeight()
    {
        return terr.terrainData.GetHeights(posXInTerrain, posYInTerrain, 1, 1)[0, 0];
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

    public void ActivateDigging(Equipment equipment)
    {
        if(equipment.capabiltiy == Capability.Digging)
        {
            canModifyTerrain = true;
            InputManager.Instance.executePrimaryAction = dig;
            InputManager.Instance.OnIncrease += increaseDesiredHeight;
            InputManager.Instance.OnDecrease += decreaseDesiredHeight;
            UpdateCurrentHeight();
            desiredHeight = CurrentHeight();
        }
    }

    public void DeactivateDigging(Equipment equipment)
    {
        if (equipment.capabiltiy == Capability.Digging)
        {
            canModifyTerrain = false;
            InputManager.Instance.executePrimaryAction -= dig;
            InputManager.Instance.OnIncrease -= increaseDesiredHeight;
            InputManager.Instance.OnDecrease -= decreaseDesiredHeight;
        }
 
    }

    public void OnPlayerChanged(GameObject player)
    {
        this.currentPlayer = player;
        // update canModifyTerrain according to currentPlayer
    }


    public void increaseDesiredHeight()
    {
        desiredHeight += increment;
    }

    public void decreaseDesiredHeight()
    {
        desiredHeight -= increment;
    }
}
