using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script will be crucial for the rest of the game so we need to modify the execution
public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayer; 

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
                selectedUnit = unit;
                return true;
            }
       }
        return false;
    }






}
