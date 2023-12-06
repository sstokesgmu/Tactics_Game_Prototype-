using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
    private int width;
    private int length;
    private float height;
    private Vector3 cellSize;
    private Vector3 startPos;
    private GridObject[,,] gridObjectArray;

    //Creating Constructors
    public GridSystem(int width, int length, int height, Vector3 cellSize, Vector3 startingPos)
    {
        this.width = width;//x
        this.height = height;// y
        this.length = length;// z

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

    //Grid Validation
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.y >= 0
               && gridPosition.x < width && gridPosition.z < length && gridPosition.y < height; 
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetLength()
    {
        return length;
    }
    public int GetHeight()
    {
        return (int)height;
    }

 
    //Convert Grid Position to World Position with height included
    public Vector3 GetWorldPosition(GridPosition gridPos)
    {
       return new Vector3(
       gridPos.x * cellSize.x + startPos.x,
       gridPos.y * cellSize.y + startPos.y,
       gridPos.z * cellSize.z + startPos.z
       );
    }

    // Convert World position to Grid position
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
        Mathf.RoundToInt((worldPosition.x - startPos.x) / cellSize.x),
        Mathf.RoundToInt((worldPosition.y - startPos.y) / cellSize.y),
        Mathf.RoundToInt((worldPosition.z - startPos.z) / cellSize.z)
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
                    GridPosition gridPosition = new GridPosition(x, y, z);
                    Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), debugPrefab.rotation);
                    GridDebugObject gridObject = debugTransform.GetComponent<GridDebugObject>();
                    gridObject.SetGridObject(GetGridObjet(gridPosition));
                }
            }
        }
    }

    public GridObject GetGridObjet(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.y, gridPosition.z];
    }
}
