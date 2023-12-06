using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 2;

    public static event EventHandler OnAnyActionPointChanged; 


    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = MAX_ACTION_POINTS;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();

    }

    private void Start()
    {
        //Delegates 
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;


        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        //Check if Unit change Position
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    #region Getter Functions
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction() 
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition; 
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }
    public int GetActionPoints()
    {
        return actionPoints;
    }
    #endregion

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        actionPoints = MAX_ACTION_POINTS;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool TryToSpendActionPoints(BaseAction action)
    {
        if(CanSpendActionPoints(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }
        return false;
    }

    public bool CanSpendActionPoints(BaseAction action)
    {
        return actionPoints >= action.GetActionPointsCost();
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }


}
