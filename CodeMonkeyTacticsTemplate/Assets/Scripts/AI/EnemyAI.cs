using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn, 
        Busy
    }

    private State state; 
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.GetIsPlayerTurn())
            return;
        if(UnitManager.Instance.GetFriendlyUnitList().Count <= 0)
        {
            TurnSystem.Instance.NextTurn();
            return;
        }
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer < 0)
                {

                    state = State.Busy;
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        state = State.Busy;
                    else
                    {
                        Debug.Log("called");
                        TurnSystem.Instance.NextTurn();
                    }
                        
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = .5f;
        state = State.TakingTurn;
    }



    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.GetIsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeEnemyAIAction( Action onEnemyAIActionComplete)
    {
        foreach (Unit enemy  in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemy, onEnemyAIActionComplete))
                return true;
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if(!enemyUnit.CanSpendActionPoints(baseAction))
            {
                //Eenmy cannot afford this action
                continue;
            }

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TryToSpendActionPoints(bestBaseAction))
        {
            Debug.Log(bestEnemyAIAction);
            switch (bestBaseAction)
            {
                
                case MoveAction moveAction:
                    moveAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
                    return true;
                case SpinAction spinAction:
                    spinAction.TakeAction(onEnemyAIActionComplete);
                    return true;
                case ShootAction shootAction:
                    shootAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
                    return true;
                default: return false;  
            }
        }
        else
            return false;
    }
}
