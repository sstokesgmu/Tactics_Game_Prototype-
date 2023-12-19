using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    private int turnNumber = 1;
    public bool isPlayerTurn = true;
    [SerializeField] private int numberOfEnemiesToSpawn;

    //Events
    public event EventHandler OnTurnChanged;
    public event EventHandler<int> OnNoMoreEnemies;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Turn System in the scene!");
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    public void NextTurn()
    {
        if (UnitManager.Instance.GetEnemyUnitList().Count <= 0)
        {
            //If the player deafeats all the enemies then we 
            // reward them by giving them a extra turn
            isPlayerTurn = true;
            OnNoMoreEnemies?.Invoke(this, numberOfEnemiesToSpawn);
            numberOfEnemiesToSpawn += 1;
        }
        else
        {
            isPlayerTurn = !isPlayerTurn;
            if (isPlayerTurn)
                turnNumber++;
        }

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
 
    }


    #region Getter Functions
    public int GetCurrentTurnNumber()
    {
        return turnNumber;
    }
    public bool GetIsPlayerTurn()
    {
        return isPlayerTurn;
    }
    #endregion
}
