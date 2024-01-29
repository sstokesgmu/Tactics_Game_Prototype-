using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace Grid
{
    public class GridSystem<TGridObject> 
    {
        private readonly GridDimensions gridDimensions;
        private readonly CellDimensions cellDimensions;
        private readonly Vector3 startPos;
        private TGridObject[,,] gridObjectArray;

        public GridSystem(GridDimensions gridDimensions, CellDimensions cellDimensions, Vector3 startPos, 
            Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            gridDimensions.height += 1;
            this.gridDimensions = gridDimensions;
            this.cellDimensions = cellDimensions;
            this.startPos = startPos;
            gridObjectArray = new TGridObject[gridDimensions.width,gridDimensions.height,gridDimensions.length];
            //Loop through the width, length, and height and create a grid position and grid object add the the array location
            for (int x = 0; x < gridDimensions.width; x++) {
                for (int z = 0;  z < gridDimensions.length; z++) {
                    for (int y = 0; y < gridDimensions.height; y++) {
                        GridPosition gridPosition = new GridPosition(x, y, z);
                        gridObjectArray[x,y,z] = createGridObject(this, gridPosition);
                    }
                }
            }
        }
        //Grid Validation
        public bool IsValidGridPosition(GridPosition gridPosition) {
            return gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.y >= 0
                   && gridPosition.x < gridDimensions.width && gridPosition.z < gridDimensions.length && 
                   gridPosition.y < gridDimensions.height; }
        //Get Grid Position
        public TGridObject GetGridObjet(GridPosition gridPosition) {
            return gridObjectArray[gridPosition.x, gridPosition.y, gridPosition.z]; }
        public List<GridPosition> GetValidGridPositions()
        {
            ////what makes a grid position valid
            //List<GridPosition> validGridPositionList = new List<GridPosition>();
            ////1. no object is on the grid position
            //foreach (TGridObject gridObject in gridObjectArray)
            //{
            //    //Does not have a unit on it
            //    if (gridObject.HasAnyUnit())
            //        continue;
            //    validGridPositionList.Add(gridObject.GetGridPosition());
            //}
            return null;
        }
        //Convert Grid Position to World Position with height included
        public Vector3 GetWorldPosition(GridPosition gridPos)
        {
            return new Vector3(
                gridPos.x * cellDimensions.x + startPos.x, 
                gridPos.y * cellDimensions.y + startPos.y, 
                gridPos.z * cellDimensions.z + startPos.z
            );
        }
        // Convert World position to Grid position
        public GridPosition GetGridPosition(Vector3 worldPosition) {
            return new GridPosition(
                Mathf.RoundToInt((worldPosition.x - startPos.x) / cellDimensions.x),
                Mathf.RoundToInt((worldPosition.y - startPos.y) / cellDimensions.y),
                Mathf.RoundToInt((worldPosition.z - startPos.z) / cellDimensions.z)
            );
        }
        //Convert Grid Position to World Position List
        public List<Vector3> GetWorldPositionsFromList(List<GridPosition> gridPosition) {
            List<Vector3> posList = new List<Vector3>();    
            foreach (GridPosition gridPos in gridPosition ) {
                posList.Add(new Vector3(
                    gridPos.x * cellDimensions.x + startPos.x, 
                    gridPos.y * cellDimensions.y + startPos.y, 
                    gridPos.z * cellDimensions.z + startPos.z)); 
            } 
            return posList;
        } 
        // Convert World position to Grid position List
        public List<GridPosition> GetGridPositionsFromList(List<Vector3> worldPositions) {
            List<GridPosition> gridPosList = new List<GridPosition>(); 
            foreach (Vector3 worldPos in worldPositions) {
                gridPosList.Add(new GridPosition(
                    Mathf.RoundToInt((worldPos.x - startPos.x) / cellDimensions.x),
                    Mathf.RoundToInt((worldPos.y - startPos.y) / cellDimensions.y),
                    Mathf.RoundToInt((worldPos.z - startPos.z) / cellDimensions.z)));
            } 
            return gridPosList; 
        }
        public int GetWidth() => gridDimensions.width;
        public int GetLength() => gridDimensions.length;
        public int GetHeight() => gridDimensions.height;
        public GridDimensions GetGridDimensions() => gridDimensions;
        public CellDimensions GetCellSize() => cellDimensions;
        public Vector3 GetStartingPos() => startPos;
        public void CreateDebugObjects(Transform debugPrefab) { 
            for (int x = 0; x < gridDimensions.width; x++) { 
                for (int z = 0; z < gridDimensions.length; z++) { 
                    for (int y = 0; y < gridDimensions.height; y++) 
                    {
                        GridPosition gridPosition = new GridPosition(x, y, z);
                        Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), debugPrefab.rotation);
                        GridDebugObject gridObject = debugTransform.GetComponent<GridDebugObject>();
                        gridObject.SetGridObject(GetGridObjet(gridPosition) as GridObject);
                    }
                }
            }
        }
    }
}
