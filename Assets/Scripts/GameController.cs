using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonComponent<GameController> {

    [SerializeField] float aiActCooldown = 1f;
    [SerializeField] float enemySpawnCooldown = 1f;
    [SerializeField] Transform spawnPoints;
    [SerializeField] Enemy enemyPrefabs;

    private float lastAITurnTime = 0;

    public Action OnAIAction = () => { };


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (lastAITurnTime <= Time.time)
        {
            AIsAct();
            lastAITurnTime = Time.time + aiActCooldown;
        }

        if (enemySpawnCooldown <= Time.time)
        {
            

            lastAITurnTime = Time.time + aiActCooldown;
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
