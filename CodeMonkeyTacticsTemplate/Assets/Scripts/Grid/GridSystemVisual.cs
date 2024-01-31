using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid;
using Unity.VisualScripting;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        public static GridSystemVisual Instance { get; private set; }
        private GridDimensions gridDimensions;
        [Serializable] public struct GridVisualTypeMaterial {
            public GridVisualType gridVisualType;
            public Material material;
        }
        public enum GridVisualType { White, Blue, Red, RedSoft, Yellow }
        [SerializeField] private Transform gridSystemVisualSinglePrefab;
        [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
        private GridSystemVisualSingle[,,] gridSystemVisualArray; // Holds all the Grid System Vis. Single instances at a gridPosition 

        private void Awake() {
            if(Instance != null) {
                Debug.LogError("There's more than one GridSystemVisual! " + transform + " - " + Instance);
                Destroy(gameObject);
                return; }
            Instance = this;
        }
        private void Start()
        {
            gridDimensions = LevelGrid.Instance.GetGridDimensions();
            gridSystemVisualArray = new GridSystemVisualSingle[gridDimensions.width, gridDimensions.height, gridDimensions.length];
            for (int x = 0; x < gridDimensions.width; x++)
            {
                for (int z = 0; z < gridDimensions.length; z++)
                {
                    for (int y = 0; y < gridDimensions.height; y++)
                    {
                        GridPosition gridPosition = new GridPosition(x, y, z);
                        Transform gridSystemVisualSingleTransform =
                                Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                        gridSystemVisualArray[x, y, z] = gridSystemVisualSingleTransform.GetComponentInChildren<GridSystemVisualSingle>();
                        if (gridSystemVisualArray[x, y, z] == null)
                            Debug.Log("null");
                    }
                }
            }
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnUnitMovedGridPosition += LevelGrid_OnUnitMovedGridPosition;
            UpdateGridVisual();
        }
        public void UpdateGridVisual()
        {
            HideAllGridPositions();
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

            GridVisualType gridVisualType;
            switch(selectedAction)
            {
                default: //Move Action
                    gridVisualType = GridVisualType.White;
                    break;
                case SpinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
                case ShootAction shootAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRange(selectedUnit.GetGridPosition(), 
                        shootAction.GetActionRange(), GridVisualType.RedSoft);
                    break;
            }
            ShowAllGridPositions(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        }
        private void HideAllGridPositions() {
            for (int x = 0; x < gridDimensions.width; x++)
                for (int z = 0; z < gridDimensions.length; z++)
                    for (int y = 0; y < gridDimensions.height; y++) {
                        //Disables the mesh renders of all the grid system visual prefabs in the array
                        gridSystemVisualArray[x, y, z].Hide(); 
                    }
        }
        private void ShowAllGridPositions(List<GridPosition> gridPositionList, GridVisualType  gridVisualType) {
            if (gridVisualTypeMaterialList?.Any() != true) {   
                Debug.LogError("Grid Visual Type Material List is Empty add materials to the list");
                return;
            }
            foreach (GridPosition gridPosition in gridPositionList)
                gridSystemVisualArray[gridPosition.x, gridPosition.y, gridPosition.z].Show(
                    GetGridVisualTypeMaterial(gridVisualType));
        }
        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    for (int y = -range; y <=  range; y++)
                    {
                        //Note:
                        // this code is similar to the shoot and move action
                        GridPosition testGridposition = gridPosition + new GridPosition(x, y, z);
                        //Validate is valid grid position
                        if (!LevelGrid.Instance.IsValidGridPosition(testGridposition)) 
                            continue;
                        int testDistance = Mathf.Abs(x) + Mathf.Abs(z) + Mathf.Abs(y);
                        if (testDistance > range)
                            continue;
                        gridPositionList.Add(testGridposition); 
                    }
                }
            }
            ShowAllGridPositions(gridPositionList, gridVisualType);
        }
        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
                if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
                    return gridVisualTypeMaterial.material;
            Debug.LogError("Could not find Grid Visual Type material for GridVisualType  " + gridVisualType);
            return null;
        }
        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) {
            UpdateGridVisual();
        }
        private void LevelGrid_OnUnitMovedGridPosition(object sender, EventArgs e) {
            UpdateGridVisual();
        }
    }
}
