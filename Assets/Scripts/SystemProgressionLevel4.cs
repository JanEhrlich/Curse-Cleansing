using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemProgressionLevel4 : MonoBehaviour
{
    SystemEvent systemEvent;
    SystemSpawn systemSpawn;
    GameObject gameLogic;
    SystemGameMaster gameMaster;
    ComponentScene componentScene;
    GameObject flyingSkull;


    //tmpvariable
    int enemySpawn1 = 3;
    int enemySpawn2 = 4;

    int enemySpawn3 = 4;

    int enemySpawn4 = 5;
    int enemySpawn5 = 6;



    float spawnTimeBetween = 0.7f;
    float nextEnemySpawnTime = 0f;

    bool alternate = true;
    bool[] enemySpawns = { false, false, false, false, false };

    Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = gameLogic.GetComponent<SystemSpawn>();
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        componentScene = systemEvent.currentState;
        systemEvent.AddActionTrigger(SetFirstEnemySpawn, 0);
        systemEvent.AddActionTrigger(SetSecondEnemySpawn,1);
        nextEnemySpawnTime = 1f;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }

    void SetFirstEnemySpawn()
    {
        enemySpawns[0] = true;
        enemySpawns[1] = true;
        enemySpawns[2] = true;
    }

    void SetSecondEnemySpawn()
    {
        enemySpawns[3] = true;
        enemySpawns[4] = true;
    }




    // Update is called once per frame
    void FixedUpdate()
    {
        offset = new Vector3(Random.Range(-1f,1f), Random.Range(-2f, 2f),1f);
        if (alternate && enemySpawn1 > 0 && nextEnemySpawnTime < Time.time)
        {
            flyingSkull = systemSpawn.InstatiateFlyingSkull(systemEvent.getEnemySpawn(0).transform, offset);
            flyingSkull.GetComponent<SystemEnemyFlyingSkull>().flyingDirection = SystemEnemyFlyingSkull.Direction.LEFT;
            componentScene.spawnedEnemies.Add(flyingSkull);
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            enemySpawn1--;
            alternate = !alternate;
        }

        if (!alternate && enemySpawn2 > 0 && nextEnemySpawnTime < Time.time)
        {
            flyingSkull = systemSpawn.InstatiateFlyingSkull(systemEvent.getEnemySpawn(1).transform, offset);
            componentScene.spawnedEnemies.Add(flyingSkull);
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            enemySpawn2--;
            alternate = !alternate;
        }

        if (enemySpawns[2] == true && enemySpawn3 > 0 && nextEnemySpawnTime < Time.time)
        {
            flyingSkull = systemSpawn.InstatiateFlyingSkull(systemEvent.getEnemySpawn(2).transform, offset);
            flyingSkull.GetComponent<SystemEnemyFlyingSkull>().flyingDirection = SystemEnemyFlyingSkull.Direction.LEFT;
            componentScene.spawnedEnemies.Add(flyingSkull);
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            enemySpawn3--;
        }

        if (enemySpawns[3] == true && enemySpawn4 > 0 && nextEnemySpawnTime < Time.time)
        {
            flyingSkull = systemSpawn.InstatiateFlyingSkull(systemEvent.getEnemySpawn(3).transform, offset);
            flyingSkull.GetComponent<SystemEnemyFlyingSkull>().flyingDirection = SystemEnemyFlyingSkull.Direction.LEFT;
            componentScene.spawnedEnemies.Add(flyingSkull);
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            enemySpawn4--;
        }

        if (enemySpawns[4] == true && enemySpawn5 > 0 && nextEnemySpawnTime < Time.time)
        {
            flyingSkull = systemSpawn.InstatiateFlyingSkull(systemEvent.getEnemySpawn(4).transform, offset);
            flyingSkull.GetComponent<SystemEnemyFlyingSkull>().flyingDirection = SystemEnemyFlyingSkull.Direction.LEFT;
            componentScene.spawnedEnemies.Add(flyingSkull);
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            enemySpawn5--;
        }

    }
}
