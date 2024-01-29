using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Grid;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;


    private GridSystem<PathNode> gridSystem;
    private GridDimensions gridDimensions;
    private CellDimensions cellDimensions;
    private Vector3 startingPos;

    private void Awake()
    {
        gridDimensions.width= LevelGrid.Instance.GetWidth();
        gridDimensions.length = LevelGrid.Instance.GetLength();
        gridDimensions.height = LevelGrid.Instance.GetHeight();
        cellDimensions = LevelGrid.Instance.GetCellSize();
        startingPos = LevelGrid.Instance.GetStartPos();

        gridSystem = new GridSystem<PathNode>(gridDimensions, cellDimensions, startingPos, 
                                (GridSystem<PathNode> g,GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);   
    }

}
