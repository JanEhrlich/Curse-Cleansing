using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemProgressionLevel1 : MonoBehaviour
{

    SystemEvent systemEvent;
    SystemSpawn systemSpawn;
    ComponentScene componentScene;
    GameObject enemyClose;


    //tmpvariable
    bool enemyWasSpawned1 = false;
    // Start is called before the first frame update
    void Start()
    {
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = GameObject.Find("GameLogic").GetComponent<SystemSpawn>();
        componentScene = systemEvent.currentState;
        systemEvent.AddActionTrigger(SetFirstEnemySpawn,0);
        systemEvent.AddActionTrigger(SetComponentRespawn,0);
    }

    void SetFirstEnemySpawn()
    {
        componentScene.enemySpawns[0] = true;
    }

    void SetComponentRespawn()
    {
        componentScene.respawnpoints[1] = true;
        componentScene.lastRespawnpoint = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (componentScene.enemySpawns[0] == true && !enemyWasSpawned1)
        {
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(0).transform);
            componentScene.spawnedEnemies.Add(enemyClose);
            enemyWasSpawned1 = true;
        }
    }
}
