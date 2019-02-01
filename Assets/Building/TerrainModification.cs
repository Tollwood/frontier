using UnityEngine;

public class TerrainModification : MonoBehaviour
{
    Terrain terr; // terrain to modify
    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    int posXInTerrain; // position of the game object in terrain width (x axis)
    int posYInTerrain; // position of the game object in terrain height (z axis)

    public int size = 5; // the diameter of terrain portion that will raise under the game object
    public float desiredHeight = 0; // the height we want that portion of terrain to be

    public float currentHeight = 0f;
    public float increment = 0.02f;

    public bool raiseLevel = true;
    public bool lowerLevel = true;

    void Start()
    {
        //supports only one active terrain at at time
        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapWidth;
        hmHeight = terr.terrainData.heightmapHeight;

    }

    void Update()
    {
        Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
        Vector3 coord;
        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        // get the position of the terrain heightmap where this game object is
        posXInTerrain = (int)(coord.x * hmWidth);
        posYInTerrain = (int)(coord.z * hmHeight);
        currentHeight = terr.terrainData.GetHeights(posXInTerrain, posYInTerrain,1,1)[0,0];

        if (Input.GetKey(KeyCode.X)) {
        
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
                        heights[i, j] =  Mathf.Clamp(terrainHeight + increment, terrainHeight, desiredHeight);
                    }
                    else if(lowerLevel && height > desiredHeight)
                    {
                        heights[i, j] = Mathf.Clamp(terrainHeight - increment, desiredHeight, terrainHeight);

                    }
                }
            }
            terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);
        }

    }
}
