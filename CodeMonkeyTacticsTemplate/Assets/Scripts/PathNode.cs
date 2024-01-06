using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;

    //Gird Cost 
    private int startCost; //Walking cost from the Start Node (decreases)
    private int aproxCost; // Aproximate cost to reach End Node (increases)
    private int totalCost; // total cost of startCost + aproxCost
    private PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

}
