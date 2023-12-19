//========= Sterling Stokes, All rights reserved. ============//
//
// Purpose: To show that the unit is selected  
// Notes: 
//
//
//===========================================================//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    //use Awake for internal reference 
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    //use Start for external reference
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
       
        UpdateVisual(); 
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
            meshRenderer.enabled = true;
        else
            meshRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -=
            UnitActionSystem_OnSelectedUnitChanged;
    }
}


