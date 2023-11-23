using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem gridSystem;
    private void Start()
    {
        gridSystem = new GridSystem(10, 2, 10 , 2, this.transform.position);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        Debug.Log(new GridPosition(5, 7, 1));
    }

    private void Update()
    {
       // Debug.Log(gridSystem.GetGridPosition(MouseToWorld.GetPosition()));
    }
}
