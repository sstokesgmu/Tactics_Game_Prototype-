using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;


    private GridSystem<PathNode> gridSystem;
    private int width, length, height;
    private Vector3 cellsize;
    private Vector3 startingPos;

    private void Awake()
    {
        width = LevelGrid.Instance.GetWidth();
        length = LevelGrid.Instance.GetLength();
        height = LevelGrid.Instance.GetHeight();
        cellsize = LevelGrid.Instance.GetCellSize();
        startingPos = LevelGrid.Instance.GetStartPos();

        gridSystem = new GridSystem<PathNode>(width, length, height, cellsize, startingPos, 
                                (GridSystem<PathNode> g,GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);   
    }

}
