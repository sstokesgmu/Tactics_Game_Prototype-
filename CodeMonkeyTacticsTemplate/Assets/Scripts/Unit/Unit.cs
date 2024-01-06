using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 8;
    private const int MAX_ACTION_POINTSENEMY = 4;

    public static event EventHandler OnAnyActionPointChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitKilled;

    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;



    private int actionPoints = MAX_ACTION_POINTS;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        //Delegates 
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        //Check if Unit change Position
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;


            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }


    #region Getter Functions
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction action in baseActionArray)
        {
            if (action is T)
            {
                return (T)action;
            }
        }
        return null;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }
    public int GetMaxActionPoints()
    {
        return MAX_ACTION_POINTS;
    }
    public int GetActionPoints()
    {
        return actionPoints;
    }
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
    #endregion

    #region Event Functions
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemyUnit() && !TurnSystem.Instance.GetIsPlayerTurn()) ||
           (!IsEnemyUnit() && TurnSystem.Instance.GetIsPlayerTurn()))
        {
            actionPoints = MAX_ACTION_POINTS;
            OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitKilled?.Invoke(this, EventArgs.Empty);
    }
    #endregion


    public bool TryToSpendActionPoints(BaseAction action)
    {
        if (CanSpendActionPoints(action))
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

    public bool IsEnemyUnit()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public void Heal(int amount)
    {
        

    }
}
