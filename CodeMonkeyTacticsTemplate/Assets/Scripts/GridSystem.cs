using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
    private int width;
    private int length;
    private int height;

    private float cellSize;


    //Creating Constructors
    public GridSystem(int width, int length, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.length = length;

        this.cellSize = cellSize;

        //Loop through the width and length
        for (int x = 0; x < width; x++)
        {
            for (int y = 0;  y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    //Visual for drawing the grid
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z) + Vector3.right * .2f, Color.white, 1000);
                }
            }
        }
    }

    // Convert World position to Grid position
    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize;
    }

   
}
