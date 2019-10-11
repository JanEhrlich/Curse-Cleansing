using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemProgressionLevel7 : MonoBehaviour
{
    SystemEvent systemEvent;
    SystemSpawn systemSpawn;
    GameObject gameLogic;
    SystemGameMaster gameMaster;
    ComponentScene componentScene;
    GameObject enemyRange;
    GameObject boss;

    //tmpvariable
    int maximalNumberOfSpawningEnemies = 0;
    int maximalNumberOfSimultaniusSpawnedEnemies = 5;




    float spawnTimeBetween = 0.5f;
    float nextEnemySpawnTime = 0f;

    int modulo = 0;
    bool attack = false;
    bool startFight = false;
    bool finished = false;
    bool didSpawn = false;
    bool spawnBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = gameLogic.GetComponent<SystemSpawn>();
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        componentScene = systemEvent.currentState;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        if (RespawnState.lastRespawn != 1)
        {
            StartFight();
        }
    }


    //TODO maybe go direct to bossfigth
    public void StartFight()
    {
        gameLogic.GetComponent<SystemMainCharacterMovement>().allowAttack = true;
        SpawnStartEnemies();
        startFight = true;
        //Debug.Log(gameMaster.enemys.Count);
        RespawnState.lastRespawn = 1;
    }

    void SpawnStartEnemies()
    {
        //Start enemies:
        foreach (var pos in systemEvent.startEnemies)
        {
            enemyRange = systemSpawn.InstantiateEnemyPirateRange(pos.transform);
        }
        //some random dude is spawning, do not do that
        nextEnemySpawnTime = Time.time + 5f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (finished) return;

        if (startFight && !attack && gameMaster.enemys.Count == 0 && nextEnemySpawnTime < Time.time) attack = true;

        if(attack == true && !spawnBoss)
        {
            boss = systemSpawn.InstantiateEnemyBoss(systemEvent.getEnemySpawn(5).transform);
            componentScene.spawnedEnemies.Add(boss);
            spawnBoss = true;
        }


        if (attack == true && maximalNumberOfSimultaniusSpawnedEnemies > gameMaster.enemys.Count && maximalNumberOfSpawningEnemies > 0 && nextEnemySpawnTime < Time.time)
        {
            switch (maximalNumberOfSpawningEnemies)
            {
                case 5:
                    enemyRange = systemSpawn.InstantiateEnemyPirateRange(systemEvent.getEnemySpawn(0).transform);
                    didSpawn = true;
                    break;
                case 4:
                    enemyRange = systemSpawn.InstantiateEnemyPirateRange(systemEvent.getEnemySpawn(1).transform);
                    didSpawn = true;
                    break;
                case 3:
                    enemyRange = systemSpawn.InstantiateEnemyPirateRange(systemEvent.getEnemySpawn(2).transform);
                    didSpawn = true;
                    break;
                case 2:
                    enemyRange = systemSpawn.InstantiateEnemyPirateRange(systemEvent.getEnemySpawn(4).transform);
                    didSpawn = true;
                    break;
                case 1:
                    enemyRange = systemSpawn.InstantiateEnemyPirateRange(systemEvent.getEnemySpawn(3).transform);
                    didSpawn = true;
                    break;
                default:
                    didSpawn = false;
                    break;
            }
            if (didSpawn)
            {
                componentScene.spawnedEnemies.Add(enemyRange);
                maximalNumberOfSpawningEnemies--;
                nextEnemySpawnTime = Time.time + spawnTimeBetween;
                didSpawn = false;
            }
        }

        if (maximalNumberOfSpawningEnemies <= 0 && gameMaster.enemys.Count <= 0 && nextEnemySpawnTime < Time.time && spawnBoss)
        {
            finished = true;
        }
    }

}
