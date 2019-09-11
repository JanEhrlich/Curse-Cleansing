using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemProgressionLevel1 : MonoBehaviour
{

    SystemEvent systemEvent;
    ComponentScene componentScene;
    GameObject enemyClosePrefab;
    GameObject enemyClose;
    bool wasSaved = true;


    //tmpvariable
    bool enemyWasSpawned1 = false;
    // Start is called before the first frame update
    void Start()
    {
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        enemyClosePrefab = Resources.Load("EnemyOrderClose") as GameObject;
        componentScene = systemEvent.currentState;
        //systemEvent.AddActionTrigger(SaveGameState,0);
        systemEvent.AddActionTrigger(SetFirstEnemySpawn,0);
        systemEvent.AddActionTrigger(SetComponentRespawn,0);
    }


    void SaveGameState()
    {
        if (!componentScene.wasSaved)
        {
            componentScene.wasSaved = true;
        }
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
            enemyClose = Instantiate(enemyClosePrefab, systemEvent.getEnemySpawn(0).transform);
            componentScene.spawnedEnemies.Add(enemyClose);
            enemyWasSpawned1 = true;
        }
    }
}
