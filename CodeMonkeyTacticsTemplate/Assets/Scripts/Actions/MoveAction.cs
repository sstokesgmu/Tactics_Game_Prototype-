using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction
{ 
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxHorMoveDistance = 4;
    [SerializeField] private int maxVertMoveDistance = 1;

    private Vector3 targetPosition;
 
    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    private void Update()
    {
        if(isActive)
        {
            //Face target direction
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            //Check how close the player is to the target position
            float stoppingDistance = .2f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {

                float moveSpeed = 4f;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;


                //Animator
                unitAnimator.SetBool("isWalking", true);
            }
            else
            {
                unitAnimator.SetBool("isWalking", false);
                isActive = false;
                //Call Delegate
                onActionComplete();
            }
               
            //Rotate Towards 
            float rotationSpeed = 4f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
        }
     
    }

    public override void TakeAction(GridPosition position, Action onActionComplete)
    {
        //Set Delegate
        this.onActionComplete = onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(position);
        isActive = true;
    }
    
    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        
        for (int x = -(maxHorMoveDistance); x <= maxHorMoveDistance; x++)
        {
            for (int z = -(maxHorMoveDistance); z <= maxHorMoveDistance; z++)
            {
                for (int y = -maxHorMoveDistance; y <= maxVertMoveDistance; y++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, y, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    //Position outside of the grid space
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                        continue;
                    // Same Grid position where the unit is already at
                    if (unitGridPosition == testGridPosition)
                        continue;
                    //GridPosition already occupied by another object
                    if(LevelGrid.Instance.HasAnyObjectOnGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    //if the grid position passes all these points then it is a valid position
                    //add to the list of valid grid positions

                    validGridPositionList.Add(testGridPosition);
                   // Debug.Log(testGridPosition);
                }
            }
        }
        return validGridPositionList;
    }
}
