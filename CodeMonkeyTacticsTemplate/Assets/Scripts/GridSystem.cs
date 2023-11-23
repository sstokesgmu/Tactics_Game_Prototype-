using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
    private int width;
    private int length;
    private float height;

    private float cellSize;
    private Vector3 startPos;

    private GridObject[,,] gridObjectArray;


    //Creating Constructors
    public GridSystem(int width, int height, int length, float cellSize, Vector3 startingPos)
    {
        this.width = width;  //x
        this.height = height; // y
        this.length = length; // z

        this.cellSize = cellSize;
        this.startPos = startingPos;

        gridObjectArray = new GridObject[width,height,length];

        //Loop through the width and length
        for (int x = 0; x < width; x++)
        {
            for (int z = 0;  z < length; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridPosition gridPosition = new GridPosition(x, y, z);
                    gridObjectArray[x,y,z] = new GridObject(this, gridPosition);

                }
            }
        }
    }
 
    //Convert Grid Position to World Position with height included
    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize;
    }

    // Convert World position to Grid position
    public GridPosition GetGridPosition(Vector3 worldposition)
    {
        return new GridPosition(Mathf.RoundToInt(worldposition.x / cellSize),
            Mathf.RoundToInt(worldposition.y / cellSize),
            Mathf.RoundToInt(worldposition.z / cellSize)
            );
    }


    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Calculate world position using starting position of the test obj
                    Vector3 worldPosition = startPos + GetWorldPosition(x, y, z);
                    GameObject.Instantiate(debugPrefab, worldPosition, debugPrefab.rotation);
                }
            }
        }
    }
}
