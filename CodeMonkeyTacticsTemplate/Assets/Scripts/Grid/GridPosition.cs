
//========= Sterling Stokes, All rights reserved. ============//
//
// Purpose:  
// Notes: 
//
//
//===========================================================//

using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int y;
    public int z;

    public GridPosition(int x, int y, int z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
            x == position.x &&
            y == position.y &&
            z == position.z;
    }

    //Interface implementation
    public bool Equals(GridPosition other)
    {
        return this == other;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    //Note:
    // Right now I removed the height parameter will need to add later
    public override string ToString()
    {
        return $"x: {x};y: {y}; z: {z};";
    }

    //bool operators
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.y - b.y, a.z - b.z);
    }
}
 

