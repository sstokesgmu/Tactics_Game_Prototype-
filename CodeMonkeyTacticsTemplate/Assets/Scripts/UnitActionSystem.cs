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
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayer;

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

    private void Update()
    {
        //For Testing will refactor into a function
        if (Input.GetMouseButtonDown(0))
        {
            if(TryHandleUnitSelection()) return;
            selectedUnit.Move(MouseToWorld.GetPosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayer))
       {
            if(hit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
       }
        return false;
    }

    #region Setter Functions
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        //Null Check
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }
    #endregion
    #region Getter Functions
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    #endregion
}
