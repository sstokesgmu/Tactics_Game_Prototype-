using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
   [SerializeField] private Unit unit;
    [SerializeField] private Spawner spawner;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Called");
     
            //GridSystemVisual.Instance.HideAllGridPositions();
            //GridSystemVisual.Instance.ShowAllGridPositions(
            //    unit.GetMoveAction().GetValidActionGridPositionList());

        }
    }

}
