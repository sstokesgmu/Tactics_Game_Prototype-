using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    [System.Serializable] public struct GridDimensions {
        public int length, width;
        [Tooltip("The height of the grid. Increase this number to increase the height of the grid.")]
        public int height; }
    [System.Serializable] public struct CellDimensions {
        [Tooltip("The size to a grid cell in a particular axis")]
        public int x, y, z; }
    
    public class LevelGrid : MonoBehaviour {
        public static LevelGrid Instance { get; private set; }
        [Header("Grid Properties")]
        public GridDimensions gridDimensions; 
        public CellDimensions cellDimensions;
        [SerializeField] private Transform gridDebugObjectPrefab;
        [SerializeField] private Transform unitPrefab;
        [SerializeField] private bool useCharacterHeight = false;

        private CapsuleCollider basicCharacter;
        private GridSystem<GridObject> gridSystem;
        public event EventHandler OnUnitMovedGridPosition;
        private void Awake()
        {
            if (Instance != null) { 
                Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
                Destroy(Instance);
                return; }
            Instance = this;
            //We should get the height of the basic character
            if (unitPrefab != null) {
                basicCharacter = unitPrefab.GetComponent<CapsuleCollider>();
                if(useCharacterHeight)
                    cellDimensions.y = GetCharacterHeight(basicCharacter); }
            gridSystem = new GridSystem<GridObject>(gridDimensions, cellDimensions, this.transform.position,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
            gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        }
        private int GetCharacterHeight(CapsuleCollider col) { return Mathf.RoundToInt(col.height + 0.1f); }

        #region Unit Control
            public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) { 
                GridObject gridObject = gridSystem.GetGridObjet(gridPosition); 
                gridObject.AddUnit(unit);
            }
          public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) {
            GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
            return gridObject.GetUnitList();
            }
            public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) { 
                GridObject gridObject = gridSystem.GetGridObjet(gridPosition); 
                gridObject.RemoveUnit(unit);
            }
            #endregion
        #region Grid Control
            public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition) { 
            RemoveUnitAtGridPosition(fromGridPosition, unit); 
            AddUnitAtGridPosition(toGridPosition, unit); 
            OnUnitMovedGridPosition?.Invoke(this, EventArgs.Empty); 
            } 
            public bool HasAnyObjectOnGridPosition(GridPosition gridPosition) {
                GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
                return gridObject.HasAnyUnit(); 
            }
            public int GetWidth() => gridSystem.GetWidth();
            public int GetHeight() => gridSystem.GetHeight();
            public int GetLength() => gridSystem.GetLength();
            public GridDimensions GetGridDimensions() => gridSystem.GetGridDimensions();
            public CellDimensions GetCellSize() => gridSystem.GetCellSize();
            public Vector3 GetStartPos() => gridSystem.GetStartingPos();
            public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
                GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
                return gridObject.GetUnit();
            }
            //Pass through Functions
            public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
            public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition); 
            public List<GridPosition> GetGridPositions(List<Vector3> worldPositions) 
                => gridSystem.GetGridPositionsFromList(worldPositions); 
            public List<Vector3> GetWorldPositionFromList(List<GridPosition> gridPositions)
                => gridSystem.GetWorldPositionsFromList(gridPositions); 
            public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition); 
            public List<GridPosition> GetValidGridPositions() => gridSystem.GetValidGridPositions();
            #endregion
    }
}
