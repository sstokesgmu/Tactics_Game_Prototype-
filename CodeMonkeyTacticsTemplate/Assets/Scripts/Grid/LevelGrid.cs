using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [Header("Grid Properties")]
    public int length = 0;
    public int width = 0;
    public int height = 0;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private CapsuleCollider basicCharacter;

    private GridSystem gridSystem;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;

        //We should get the height of the basic character
        //for some reason I have to use an actual in scene object
        int characterHeight = GetCharacterHeight(basicCharacter);
        Vector3 cellsize = new Vector3(2, characterHeight, 2);
        gridSystem = new GridSystem(length, width, height, cellsize, this.transform.position);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }



    private int GetCharacterHeight(CapsuleCollider col)
    {
        return Mathf.FloorToInt(col.bounds.size.y);
    }

    #region Unit Control
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
        gridObject.AddUnit(unit);
    }
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
        return gridObject.GetUnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
        gridObject.RemoveUnit(unit);
    }
    #endregion

    #region Grid Control
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);
    }

    public  bool HasAnyObjectOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjet(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public int GetWidth() => gridSystem.GetWidth();
    public int GetLength() => gridSystem.GetLength();
    public int GetHeight() => gridSystem.GetHeight();


    //Pass through Functions
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    #endregion
}
