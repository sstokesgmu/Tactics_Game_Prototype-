using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }


    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridvisualTypeMaterialList;

    private GridSystemVisualSingle[,,] gridSystemVisualArray;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        

    }

    private void Start()
    {
        gridSystemVisualArray = new GridSystemVisualSingle[
                                     LevelGrid.Instance.GetWidth(),
                                     LevelGrid.Instance.GetLength(),
                                     LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                for (int y = 0; y < LevelGrid.Instance.GetHeight(); y++)
                {
                    GridPosition gridPosition = new GridPosition(x, y, z);

                    Transform gridSystemVisualSingleTransform =
                                Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                    gridSystemVisualArray[x, z, y] = gridSystemVisualSingleTransform.GetComponentInChildren<GridSystemVisualSingle>();
                    if (gridSystemVisualArray[x, z, y] == null)
                        Debug.Log("null");
                }
            }
        }
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnUnitMovedGridPosition += LevelGrid_OnUnitMovedGridPosition;
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                for (int y = 0; y < LevelGrid.Instance.GetHeight(); y++)
                {
                    gridSystemVisualArray[x, z, y].Hide();
                }
            }
        }
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

    public void ShowAllGridPositions(List<GridPosition> gridPositionList, GridVisualType  gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualArray[gridPosition.x, gridPosition.z, gridPosition.y].Show(
                GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    public void UpdateGridVisual()
    {
        HideAllGridPositions();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        GridVisualType gridVisualType;
        switch(selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), 
                    shootAction.GetActionRange(), GridVisualType.RedSoft);
                break;
        }
        ShowAllGridPositions(
           selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridvisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find Grid Visual Type material for GridVisualType  " + gridVisualType);
        return null;
    }
}
