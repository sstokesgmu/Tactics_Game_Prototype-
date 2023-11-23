
//========= Sterling Stokes, All rights reserved. ============//
//
// Purpose:  
// Notes: 
//
//
//===========================================================//


using UnityEngine;

public struct GridPosition
{
    public int x;
    public int y;
    public int z;
  //  public Vector3 startingpos;


    public GridPosition(int x, int y, int z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public override string ToString()
    {
        return $"x: {x}; y: {y}; z: {z};";
    }

}

