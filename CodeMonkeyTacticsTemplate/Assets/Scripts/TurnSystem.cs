using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    private int turnNumber = 1;

    //Events
    public event EventHandler OnTurnChanged; 

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
        turnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    #region Getter Functions
    public int GetCurrentTurnNumber()
    {
        return turnNumber;
    }
    #endregion
}
