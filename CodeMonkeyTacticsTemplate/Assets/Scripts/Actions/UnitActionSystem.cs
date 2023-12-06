//========= Sterling Stokes, All rights reserved. ============//
//
// Purpose: Top handles excuation of the selceted unit  
// Notes: This script will be crucial for the rest of the game so we need to modify the execution
//
//
//===========================================================//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    //Events
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayer;

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
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if(TryHandleUnitSelection())
            return;

        HandleSelectedAction();
    }

    //Input Control 
    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Convert world position to grid position
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseToWorld.GetPosition());
            switch (selectedAction)
            {
                case MoveAction moveAction:
                    if (!moveAction.IsValidActionGridPosition(mouseGridPosition))
                        return;
                    if (!selectedUnit.TryToSpendActionPoints(moveAction))
                        return;
                    SetBusy();
                    moveAction.TakeAction(mouseGridPosition, ClearBusy);
                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                    break;
                case SpinAction spinAction:
                    if (!spinAction.IsValidActionGridPosition(mouseGridPosition))
                        return;
                    if (!selectedUnit.TryToSpendActionPoints(spinAction))
                        return;
                    SetBusy();
                    spinAction.TakeAction(ClearBusy);
                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                    break;                   
            }
        }
    }


    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayer))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
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
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction()); 
        //Null Check
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
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
        return selectedUnit;
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    #endregion
}
