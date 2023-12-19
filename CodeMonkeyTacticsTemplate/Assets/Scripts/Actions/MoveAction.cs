using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction
{ 
    [SerializeField] private int maxHorMoveDistance = 4;
    [SerializeField] private int maxVertMoveDistance = 1;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

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
            }
            else
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
               
            //Rotate Towards 
            float rotationSpeed = 7f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
        }
     
    }

    public override void TakeAction(GridPosition position, Action onActionComplete)
    {

        ActionStart(onActionComplete);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(position);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }
    
    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();


        // Check if the unit is null or has been destroyed
        if (unit == null)
        {
            // Handle the case where the unit is null or destroyed
            // For example, you might return an empty list or log a warning
            Debug.LogWarning("Unit is null or has been destroyed.");
            return validGridPositionList;
        }

        //Note we should fix this to remove going through two pass throughs it doesn't work on initialization

        GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(unit.transform.position);

        //GridPosition unitGridPosition = unit.GetGridPosition();

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


    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
       int targetCountAtGridPosition =
            unit.GetShootAction().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
