using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonComponent<GameController> {

    [SerializeField] float aiActCooldown = 1f;
    [SerializeField] float enemySpawnCooldown = 1f;
    [SerializeField] Enemy[] enemyPrefabs;
    [SerializeField] Transform enemiesHolder;

    [SerializeField] Transform bottomLeftSpawn;
    [SerializeField] Transform topLeftSpawn;
    [SerializeField] Transform topRightSpawn;
    [SerializeField] Transform bottomRightSpawn;

    private float lastAITurnTime = 0;
    private float lastEnemySpawn = 0;

    public Action OnAIAction = () => { };


    void Start () {
		
	}
	
	void Update () {
        if (lastAITurnTime <= Time.time)
        {
            AIsAct();
            lastAITurnTime = Time.time + aiActCooldown;
        }

        if (lastEnemySpawn <= Time.time)
        {
            //var thisTrySpawnPoints = new List<Transform>(spawnPoints);
            System.Func<Vector2Int, Enemy, bool> trySpawn = (spawnPoint, enemy) =>
            {
                //Vector2 offsetSpawnPoint = spawnPoint.position.xy() - new Vector2(enemy.size.x - 1, enemy.size.y - 1);
                var foundEntities = Inventory.Instance.AtRect<EntityBase>(spawnPoint, enemy.size.x, enemy.size.y, Inventory.ScanDirection.NONE);
                if (foundEntities.Count == 0)
                {
                    var newEnemy = Instantiate(enemy, enemiesHolder) as Enemy;
                    newEnemy.Position = spawnPoint;
                    return true;
                }
                else
                {
                    return false;
                }
            };

            bool spawned = false;
            int numberOfTries = 0;
            var randomEnemy = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length - 1)];
            while (spawned == false)
            {
                Vector2Int offsetSpawn = Vector2Int.zero;
                float rand = UnityEngine.Random.value;
                if (rand < .25f)
                {
                    offsetSpawn = bottomLeftSpawn.position.xy().Int();
                }
                else if (rand < .5f)
                {
                    offsetSpawn = topLeftSpawn.position.xy().Int() - new Vector2Int(0, randomEnemy.size.y - 1);
                }
                else if (rand < .75f)
                {
                    offsetSpawn = topRightSpawn.position.xy().Int() - new Vector2Int(randomEnemy.size.x - 1, randomEnemy.size.y - 1);
                }
                else if (rand < 1f)
                {
                    offsetSpawn = bottomRightSpawn.position.xy().Int() - new Vector2Int(randomEnemy.size.x - 1, 0);
                }
                spawned = trySpawn(offsetSpawn, randomEnemy);
                numberOfTries++;
                if (numberOfTries > 4)
                {
                    break;
                }
            }
            lastEnemySpawn = Time.time + enemySpawnCooldown;
        }
    }

    void AIsAct()
    {
        var entititesList = new List<EntityBase>(FindObjectsOfType<EntityBase>());
        entititesList.Sort();
        foreach (var entity in entititesList)
        {
            var possibleAI = entity as AIEntity;
            if (possibleAI != null)
            {
                possibleAI.Act();
                OnAIAction();
            }
        }

    }
}
