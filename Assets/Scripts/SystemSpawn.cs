using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * thic class is used for spawning enemies, and other spawnabel object
 * TODO:
 *  -spawn enemies
 */

public class SystemSpawn : MonoBehaviour
{

    GameObject enemyClosePrefab;
    GameObject enemyClose;
    GameObject flyinSkullPrefab;
    GameObject flyingSkull;
    GameObject enemyRangePrefab;
    GameObject enemyRange;


    void Start()
    {
        enemyClosePrefab = Resources.Load("EnemyOrderClose") as GameObject;
        enemyRangePrefab = Resources.Load("EnemyPirateRange") as GameObject;
        flyinSkullPrefab = Resources.Load("FlyingSkull") as GameObject;
    }

    public GameObject InstantiateEnemyOrderClose(Transform transform, int direction = 1)
    {
        enemyClose = Instantiate(enemyClosePrefab, transform);
        //enemyClose.GetComponent<SystemEnemyClose>().FlipCharacterDirection(direction);
        return enemyClose;
    }


    public GameObject InstantiateEnemyPirateRange(Transform transform, int direction = 1)
    {
        enemyRange = Instantiate(enemyRangePrefab, transform);
        //enemyRange.GetComponent<SystemEnemyRange>().FlipCharacterDirection(direction);
        return enemyRange;
    }

    public GameObject InstatiateFlyingSkull(Transform transform, int direction = 1)
    {
        flyingSkull = Instantiate(flyinSkullPrefab, transform);
        //flyingSkull.GetComponent<SystemEnemyFlyingSkull>().UpdateDirection( direction < 0? SystemEnemyFlyingSkull.Direction.LEFT: SystemEnemyFlyingSkull.Direction.RIGHT);
        return flyingSkull;
    }


}
