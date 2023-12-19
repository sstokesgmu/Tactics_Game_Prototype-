using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    

    private float totalSpinAmount = 0f;


    public override string GetActionName()
    {
        return "Spin";
    }


    private void Update()
    {
        if(isActive)
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount >= 360f)
            {
                ActionComplete();
            }
        }
    }

    public override void TakeAction(Action onActionComplete)
    {
        //Set Delegate
        ActionStart(onActionComplete);
        totalSpinAmount = 0f;

        
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        { unitGridPosition };

    }

    public override int GetActionPointsCost()
    {
        return 3;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
