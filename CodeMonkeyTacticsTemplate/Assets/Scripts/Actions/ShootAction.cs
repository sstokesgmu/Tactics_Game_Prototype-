using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{

    private enum State
    {
        
        Aiming,
        Shooting, 
        Cooloff,
    }

    [SerializeField] private int maxShootDistance = 7;
    [SerializeField] private bool calculateGridAsASphere = true;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    public event EventHandler<OnShootEventArgs> OnShooting;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    public override string GetActionName()
    {
        return "Shoot";
    }

    private void Update()
    {
        if(isActive)
        {
            stateTimer -= Time.deltaTime;

            switch(state)
            {
                case State.Aiming:
                    Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                    float rotationSpeed = 7f;
                    transform.forward =
                        Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                    break;
                case State.Shooting:
                    if (canShootBullet)
                    {
                        Shoot();
                        canShootBullet = false;
                    }
                    break;
                case State.Cooloff:
                    break;
            }

            if (stateTimer <= 0)
                NextState();
        }

    }

    public void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTimer = 0.1f;
                stateTimer = shootingStateTimer;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTimer = 0.1f;
                stateTimer = coolOffStateTimer;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
        //Debug.Log(state);
    }

    public override void TakeAction(GridPosition gridPosition,Action onActionComplete)
    {
        //Set Delegate 
        ActionStart(onActionComplete);

        //find unit at grid position
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        //Setup state timer
        //Debug.Log("Aiming");
        state = State.Aiming;
        float aimingStateTimer = 1f;
        stateTimer = aimingStateTimer;

        canShootBullet = true;
    }


    private void Shoot()
    {
        OnShooting?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit

        });
        targetUnit.Damage(40);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }


    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int x = -(maxShootDistance); x <= maxShootDistance; x++)
        {
            for (int z = -(maxShootDistance); z <= maxShootDistance; z++)
            {
                for (int y = -maxShootDistance; y <= maxShootDistance; y++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, y, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    //Position outside of the grid space
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                        continue;

                    //Here we turn the grid from a box to sphere
                    if(calculateGridAsASphere == true)
                    {
                        int testDistance = Mathf.Abs(x)  + Mathf.Abs(z);
                        if (testDistance > maxShootDistance)
                            continue;
                    }

                    // uncomment code below to see the grid
                    // validGridPositionList.Add(testGridPosition);

                    //Aviod GridPosition already occupied by another object
                    if (!LevelGrid.Instance.HasAnyObjectOnGridPosition(testGridPosition))
                        continue;

                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    //avoid the enemies targeting each other (same team)
                    if (targetUnit.IsEnemyUnit() == unit.IsEnemyUnit())
                        continue;

                    //if the grid position passes all these points then it is a valid position for this action
                    //add to the list of valid grid positions
                    validGridPositionList.Add(testGridPosition);
                }
            }
        }
        return validGridPositionList;
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
    public int GetActionRange()
    {
        return maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
       return GetValidActionGridPositionList(gridPosition).Count;
    }



}
