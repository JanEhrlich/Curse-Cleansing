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

    public GameObject afterFight;


    //tmpvariable
    int maximalNumberOfSpawningEnemies = 5;
    int maximalNumberOfSimultaniusSpawnedEnemies = 5;

    int[] maxNumberSpawn = { 3,3,3,3,3};



    float spawnTimeBetween = 0.5f;
    float nextEnemySpawnTime = 0f;

    int randomSpawn = 0;
    int modulo = 0;
    bool attack = false;
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = gameLogic.GetComponent<SystemSpawn>();
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        componentScene = systemEvent.currentState;
        randomSpawn = Mathf.RoundToInt(Random.Range(0,(float)componentScene.enemySpawns.Length + 0.0001f));

        foreach (var enemy in gameMaster.enemys)
        {
            enemy.GetComponent<SystemEnemyRange>().allowAttack = false;
        }
        gameLogic.GetComponent<SystemMainCharacterMovement>().allowAttack = false;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }


    public void StartFight()
    {
        gameLogic.GetComponent<SystemMainCharacterMovement>().allowAttack = true;

        foreach (var enemy in gameMaster.enemys)
        {
            enemy.GetComponent<SystemEnemyRange>().allowAttack = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (finished) return;

        if (!attack && gameMaster.enemys.Count == 0) attack = true;


        if (/*componentScene.enemySpawns[0]*/ attack == true && maximalNumberOfSimultaniusSpawnedEnemies > gameMaster.enemys.Count && maximalNumberOfSpawningEnemies > 0 && nextEnemySpawnTime < Time.time)
        {
            while (maxNumberSpawn[randomSpawn] <= 0)
            {
                //do this for modulo, since %-operator is just reminder, not complete modulo
                modulo = (randomSpawn - 1) % componentScene.enemySpawns.Length;
                randomSpawn = modulo < 0 ? modulo + componentScene.enemySpawns.Length : modulo;
            }
            enemyClose = systemSpawn.InstantiateEnemyOrderClose(systemEvent.getEnemySpawn(randomSpawn).transform);
            maxNumberSpawn[randomSpawn]--;
            componentScene.spawnedEnemies.Add(enemyClose);
            maximalNumberOfSpawningEnemies--;
            nextEnemySpawnTime = Time.time + spawnTimeBetween;
            randomSpawn = Mathf.RoundToInt(Random.Range(0, (float)componentScene.enemySpawns.Length -0.9999f));
        }
      
        if (maximalNumberOfSpawningEnemies <=0 && gameMaster.enemys.Count <= 0 && nextEnemySpawnTime < Time.time)
        {
            AfterFight();
            finished = true;
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
