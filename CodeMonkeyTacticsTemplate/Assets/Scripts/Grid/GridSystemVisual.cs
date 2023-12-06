using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;

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
    }

    private void Update()
    {
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

    public void ShowAllGridPositions(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualArray[gridPosition.x, gridPosition.z, gridPosition.y].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();
            BaseAction SelectedAction = UnitActionSystem.Instance.GetSelectedAction();
        ShowAllGridPositions(
            SelectedAction.GetValidActionGridPositionList());
    }
}
