using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public bool playersSpawned = false;


    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There's more than one Unit Manager! " + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitKilled += Unit_OnAnyUnitKilled; 
    }


    private void Update()
    {



        if (GetFriendlyUnitList().Count <= 0 && playersSpawned != false)
            Debug.LogError("The Enemy Defeated all the enemies restart the scene");
           // SceneManager.LoadScene("GameOver");
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        Debug.Log(unit + " spawned");

        unitList.Add(unit);
        if (unit.IsEnemyUnit())
            enemyUnitList.Add(unit);
        else
        {
            friendlyUnitList.Add(unit);
            playersSpawned = true;
        }
            
    }
    
    private void Unit_OnAnyUnitKilled(object sender, EventArgs e)
    {

        Unit unit = sender as Unit;

        Debug.Log(unit + " died");

        unitList.Remove(unit);
        if (unit.IsEnemyUnit())
            enemyUnitList.Remove(unit);
        else
            friendlyUnitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public bool GetPlayersSpawned()
    {
        return playersSpawned;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

}
