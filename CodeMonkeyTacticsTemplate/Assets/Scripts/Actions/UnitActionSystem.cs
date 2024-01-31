//========= Sterling Stokes, All rights reserved. ============//
//
// Purpose: Top handles excuation of the selceted unit  
// Notes: This script will be crucial for the rest of the game so we need to modify the execution
//
//
//===========================================================//

using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Grid;


public class UnitActionSystem : MonoBehaviour
{
    private enum SelectableComponents
    {
        Unit, TestObject,
    }
    private Dictionary<SelectableComponents, Type> componentTypeMap = new Dictionary<SelectableComponents, Type>
    {
        { SelectableComponents.Unit, typeof(Unit) },
        { SelectableComponents.TestObject, typeof(TestObject) },
        // Add more mappings for other component types if needed
    };

    public static UnitActionSystem Instance { get; private set; }

    //Events
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
    public event EventHandler OnHeal;

    [SerializeField] private MonoBehaviour selectedObj;
    [SerializeField] private LayerMask selectableLayer;

    //Action handelling
    [SerializeField] private BaseAction selectedAction;
    private bool isBusy;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(Instance);
            return; 
        }
        Instance = this;
    }

    private void Start()
    {
        //find he first unit in the scene that is not enemy
        // Find all Unit objects in the scene
        Unit[] unitsInScene = FindObjectsOfType<Unit>();
        foreach (Unit unit in unitsInScene)
        {
            if (unit.IsEnemyUnit() != true)
                SetSelectedUnit(unit);
            Debug.Log(selectedObj);
            return;
        }
    }

    private void Update()
    {
        if (isBusy)
            return;
        if (!TurnSystem.Instance.GetIsPlayerTurn())
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if(TryHandleUnitSelection())
            return;
        HandleSelectedAction();
    }

    //Input Control For Player Turn 
    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Convert world position to grid position
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.GetPosition());

            if (selectedObj is Unit unit)
            {
                switch (selectedAction)
                {
                    case MoveAction moveAction:
                        if (!moveAction.IsValidActionGridPosition(mouseGridPosition))
                            return;
                        if (!unit.TryToSpendActionPoints(moveAction))
                            return;
                        SetBusy();
                        moveAction.TakeAction(mouseGridPosition, ClearBusy);
                        OnActionStarted?.Invoke(this, EventArgs.Empty);
                        break;
                    case SpinAction spinAction:
                        if (!spinAction.IsValidActionGridPosition(mouseGridPosition))
                            return;
                        if (!unit.TryToSpendActionPoints(spinAction))
                            return;
                        SetBusy();
                        spinAction.TakeAction(ClearBusy);
                        OnActionStarted?.Invoke(this, EventArgs.Empty);
                        break;
                    case ShootAction shootAction:
                        if (!shootAction.IsValidActionGridPosition(mouseGridPosition))
                            return;
                        if (!unit.TryToSpendActionPoints(shootAction))
                            return;
                        SetBusy();
                        shootAction.TakeAction(mouseGridPosition, ClearBusy);
                        OnActionStarted?.Invoke(this, EventArgs.Empty);
                        break;

                }
            }
       
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, selectableLayer))
            {
                MonoBehaviour[] comps = hit.transform.GetComponents<MonoBehaviour>();
                MonoBehaviour selectedComponent = GetSelectedComponent(comps);
                if (selectedComponent != null)
                {
                    switch (selectedComponent)
                    {
                        case Unit unit:
                            // Cannot select if Unit is already selected 
                            if (unit == selectedObj)
                                return false;
                            Debug.Log(selectedComponent);
                            // Is enemy Unit
                            if (unit.IsEnemyUnit())
                                return false;
                            // Select Unit
                            SetSelectedUnit(unit);
                            return true;
                        case TestObject testObject:
                            if (testObject == selectedObj)
                                return false;
                            SetSelectedObj(testObject);
                            ; // Handle the case where the component is of type TestObject
                            return true;
                    }
                }
            }
        }
        return false;
    }
    private MonoBehaviour GetSelectedComponent(MonoBehaviour[] components)
    {
        foreach (var component in components)
        {
            foreach (var mapping in componentTypeMap)
            {
                if (mapping.Value.IsInstanceOfType(component))
                {
                    return component;
                }
            }
        }
        return null;
    }
    
    
    #region Action State Handelling
    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    #endregion

    #region Setter Functions
    private void SetSelectedUnit(Unit unit)
    {
        selectedObj = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        //Null Check
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetSelectedObj(MonoBehaviour behaviour)
    {
        selectedObj = behaviour ;
        Debug.Log(selectedObj);
    }
    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
       
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion
    #region Getter Functions
    public Unit GetSelectedUnit()
    {
        return (Unit)selectedObj;
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    #endregion
}
