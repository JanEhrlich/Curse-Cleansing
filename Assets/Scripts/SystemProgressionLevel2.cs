using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemProgressionLevel2 : MonoBehaviour
{
    SystemEvent systemEvent;
    SystemSpawn systemSpawn;
    ComponentScene componentScene;
    GameObject enemyClose;


    //tmpvariable
    int enemyWasSpawned1 = 0;
    int enemyWasSpawned2 = 0;
    int enemyWasSpawned3 = 0;
    float spawnTimeBetween = 0.5f;
    float nextEnemySpawnTime1 = 0;
    float nextEnemySpawnTime2 = 0;
    bool[] enemySpawns = { false, false};

    // Start is called before the first frame update
    void Start()
    {
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = GameObject.Find("GameLogic").GetComponent<SystemSpawn>();
        componentScene = systemEvent.currentState;
        systemEvent.AddActionTrigger(SetFirstEnemySpawn, 0);
        systemEvent.AddActionTrigger(SetSecondEnemySpawn, 1);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }

    void SetFirstEnemySpawn()
    {
        enemySpawns[0] = true;
    }

    void SetSecondEnemySpawn(){
        enemySpawns[1] = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(componentScene);
        if (enemySpawns[0] == true && enemyWasSpawned1 < 3 && nextEnemySpawnTime1 < Time.time)
        {
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(0).transform);
            enemyClose.GetComponent<SystemEnemyClose>().enemyType = SystemEnemyClose.EnemyType.ZOMBIE;
            enemyClose.GetComponent<SystemEnemyClose>().followRange = 20f;
            enemyClose.GetComponent<SystemEnemyClose>().speedMultiplier= 2.5f;
            componentScene.spawnedEnemies.Add(enemyClose);
            enemyWasSpawned1++;
            nextEnemySpawnTime1 = Time.time + spawnTimeBetween;
        }

        if (enemySpawns[0] == true && enemyWasSpawned3 < 1)
        {
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(2).transform);
            //enemyClose.GetComponent<SystemEnemyClose>().enemyType = SystemEnemyClose.EnemyType.ZOMBIE;
            enemyClose.GetComponent<SystemEnemyClose>().followRange = 20f;
            componentScene.spawnedEnemies.Add(enemyClose);
            enemyWasSpawned3++;
            //nextEnemySpawnTime1 = Time.time + spawnTimeBetween;
        }

        if (enemySpawns[1] == true && enemyWasSpawned2 < 3 && nextEnemySpawnTime2 < Time.time)
        {
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(1).transform);
            //enemyClose.GetComponent<SystemEnemyClose>().enemyType = SystemEnemyClose.EnemyType.ZOMBIE;
            enemyClose.GetComponent<SystemEnemyClose>().followRange = 20f;
            enemyClose.GetComponent<SystemEnemyClose>().speedMultiplier = 1f;
            componentScene.spawnedEnemies.Add(enemyClose);
            enemyWasSpawned2++;
            nextEnemySpawnTime2 =Time.time + spawnTimeBetween*3f;
        }
        spawnTimeBetween = Random.Range(0.5f, 1f);
    }
}
