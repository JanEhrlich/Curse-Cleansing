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

    public GameObject InstantiateEnemyOrderClose(Transform transform)
    {
        enemyClose = Instantiate(enemyClosePrefab, transform);
        return enemyClose;
    }
    public GameObject InstantiateEnemyOrderClose(Transform transform, Vector3 offset)
    {
        enemyClose = Instantiate(enemyClosePrefab, transform.position+offset,transform.rotation);
        return enemyClose;
    }
    

    public GameObject InstantiateEnemyPirateRange(Transform transform)
    {
        enemyRange = Instantiate(enemyRangePrefab, transform);
        return enemyRange;
    }
    public GameObject InstantiateEnemyPirateRange(Transform transform, Vector3 offset)
    {
        enemyRange = Instantiate(enemyRangePrefab, transform.position + offset, transform.rotation);
        return enemyRange;
    }

    public GameObject InstatiateFlyingSkull(Transform transform)
    {
        flyingSkull = Instantiate(flyinSkullPrefab, transform);

        return flyingSkull;
    }
    public GameObject InstatiateFlyingSkull(Transform transform, Vector3 offset)
    {
        flyingSkull = Instantiate(flyinSkullPrefab, transform.position + offset, transform.rotation);
        return flyingSkull;
    }


}
