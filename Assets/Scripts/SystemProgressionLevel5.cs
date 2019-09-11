using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemProgressionLevel5 : MonoBehaviour
{
    SystemEvent systemEvent;
    SystemSpawn systemSpawn;
    GameObject gameLogic;
    SystemGameMaster gameMaster;
    ComponentScene componentScene;
    GameObject enemyClose;


    //tmpvariable
    int maximalNumberOfSpawningEnemies = 15;
    int maximalNumberOfSimultaniusSpawnedEnemies = 5;

    int[] maxNumberSpawn = { 3,3,3,3,3};



    float spawnTimeBetween = 0.5f;
    float nextEnemySpawnTime = 0f;

    int randomSpawn = 0;
    bool attack = false;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = gameLogic.GetComponent<SystemSpawn>();
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        componentScene = systemEvent.currentState;
        randomSpawn = Mathf.RoundToInt(Random.Range(0,(float)componentScene.enemySpawns.Length + 0.0001f));

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!attack && gameMaster.enemys.Count == 0)
        {
            attack = true;
        }

        if (/*componentScene.enemySpawns[0]*/ attack == true && maximalNumberOfSimultaniusSpawnedEnemies > gameMaster.enemys.Count && maximalNumberOfSpawningEnemies > 0 && nextEnemySpawnTime < Time.time)
        {
            while (maxNumberSpawn[randomSpawn] <= 0) randomSpawn = (randomSpawn - 1) % componentScene.enemySpawns.Length;
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(randomSpawn).transform);
            maxNumberSpawn[randomSpawn]--;
            componentScene.spawnedEnemies.Add(enemyClose);
            maximalNumberOfSpawningEnemies--;
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            randomSpawn = Mathf.RoundToInt(Random.Range(0, (float)componentScene.enemySpawns.Length -0.9999f));
        }

    }
}
