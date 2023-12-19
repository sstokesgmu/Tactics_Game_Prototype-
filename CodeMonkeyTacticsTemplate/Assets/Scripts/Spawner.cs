using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform enemyUnitPrefab;
    [SerializeField] private Transform turrentPrefab;

    private List<Unit> playerUnits;

    //events

    private void Start()
    {
        TurnSystem.Instance.OnNoMoreEnemies += TurnSystem_OnNoMoreEnemies;
    }

    public void GameSide_Spawn(int numberOfEnemiesToSpawn)
    {
        //Get all Available spawn points
        List<Vector3> validWorldPositions = GetSpawnPoints();
        // Check if there are enough spawn points
        if (validWorldPositions.Count < numberOfEnemiesToSpawn)
        {
            Debug.LogWarning("Not enough valid spawn points to spawn all enemies.");
            return;
        }

        // Determine the number of enemies to spawn based on available spawn points
        int enemiesToSpawn = Mathf.Min(numberOfEnemiesToSpawn, validWorldPositions.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Choose a random index from the validWorldPositions list
            int randomIndex = UnityEngine.Random.Range(0, validWorldPositions.Count);
            // Get the randomly selected world position
            Vector3 randomWorldPosition = validWorldPositions[randomIndex];
            // Instantiate enemy at the randomly selected world position
            Transform spawnedEnemy = Instantiate(enemyUnitPrefab, randomWorldPosition, Quaternion.identity);
            // Get unit component and grid position
            Unit unit = spawnedEnemy.GetComponent<Unit>();
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(randomWorldPosition);
            // Add the unit to the grid at the calculated grid position
            LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, unit);
            // Remove the used position to avoid spawning multiple enemies at the same position
            validWorldPositions.RemoveAt(randomIndex);
        }
    }

    private List<Vector3> GetSpawnPoints()
    {
        List<Vector3> validWorldPositionList = new List<Vector3>();
        List<Vector3> worldPositions = new List<Vector3>(); 
        //distance check from the players
        playerUnits = UnitManager.Instance.GetFriendlyUnitList();
        worldPositions = LevelGrid.Instance.GetWorldPositionFromList(LevelGrid.Instance.GetValidGridPositions());

        for (int i = 0; i < worldPositions.Count; i++)
        {
            bool isFarEnough = true;

            for (int j = 0; j < playerUnits.Count; j++)
            {
                // Distance check between the current world position and the player unit
                float distance = Vector3.Distance(worldPositions[i], playerUnits[j].transform.position);
                // Set a distance threshold (adjust as needed)
                float distanceThreshold = 10f;
                // Check if the distance is greater than the threshold
                if (distance < distanceThreshold)
                {
                    isFarEnough = false;
                    break;  // No need to check other player units if one is too close
                }
            }
            if (isFarEnough)
                validWorldPositionList.Add(worldPositions[i]);
        }
        return validWorldPositionList;
    }


    private void SpawnAllies()
    {

    }

    //events
    private void TurnSystem_OnNoMoreEnemies(object sender, int e)
    {
        GameSide_Spawn(e);
    }
    

}
