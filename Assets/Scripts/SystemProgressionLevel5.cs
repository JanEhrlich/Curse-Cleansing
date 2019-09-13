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
    GameObject enemyRange;

    public GameObject afterFight;


    //tmpvariable
    int maximalNumberOfSpawningEnemies = 10;
    int maximalNumberOfSimultaniusSpawnedEnemies = 5;

    int[] maxNumberSpawn = { 3,3,3,3,3};



    float spawnTimeBetween = 0.5f;
    float nextEnemySpawnTime = 0f;

    int randomSpawn = 0;
    int modulo = 0;
    bool attack = false;
    bool startFight = false;
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = gameLogic.GetComponent<SystemSpawn>();
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        componentScene = systemEvent.currentState;
        randomSpawn = Mathf.RoundToInt(Random.Range(0,(float)(systemEvent.enemySpawns.Length-1)));

        //gameLogic.GetComponent<SystemMainCharacterMovement>().allowAttack = false;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));

        if (RespawnState.lastRespawn == 1)
        {
            StartFight();
        }
    }



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

        if (/*componentScene.enemySpawns[0]*/ attack == true && maximalNumberOfSimultaniusSpawnedEnemies > gameMaster.enemys.Count && maximalNumberOfSpawningEnemies > 0 && nextEnemySpawnTime < Time.time)
        {
            while (maxNumberSpawn[randomSpawn] <= 0)
            {
                //do this for modulo, since %-operator is just reminder, not complete modulo
                modulo = (randomSpawn - 1) % systemEvent.enemySpawns.Length;
                randomSpawn = modulo < 0 ? modulo + systemEvent.enemySpawns.Length : modulo;
            }
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(randomSpawn).transform);
            maxNumberSpawn[randomSpawn]--;
            componentScene.spawnedEnemies.Add(enemyClose);
            maximalNumberOfSpawningEnemies--;
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            randomSpawn = Mathf.RoundToInt(Random.Range(0, (float)(systemEvent.enemySpawns.Length-1f)));
        }
      
        if (maximalNumberOfSpawningEnemies <=0 && gameMaster.enemys.Count <= 0 && nextEnemySpawnTime < Time.time)
        {
            finished = true;
            AfterFight();
        }
    }


    /*
     * call after the fight
     */
    void AfterFight()
    {
        afterFight.SetActive(true);
        AudioManager.StartLevel5_1Audio();
    }

}
