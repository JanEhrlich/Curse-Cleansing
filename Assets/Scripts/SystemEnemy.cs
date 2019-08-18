using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Base Class of all Enemies. It handels basic Movement.
 * TODO:
 *  -Move Enemy
 *  -Apply Damage to other Entities e.g. Player,Enemy,Projectiles,Objects
 *  -Change ComponentEnemyState isAlive to False if health <=0
 */
public class SystemEnemy : MonoBehaviour
{
    //handles
    GameObject enemyTest;
    GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    BoxCollider2D collider2d;
    SystemGameMaster systemGameMaster;
    ComponentEnemyAction componentEnemyAction;
    ComponentEnemyState componentEnemyState;
    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        enemyTest = systemGameMaster.getEnemyTest();
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        componentEnemyState = systemGameMaster.ComponentEnemyState;
        componentEnemyAction = systemGameMaster.ComponentEnemyAction;


        //Sets Layermask of enemy
        componentEnemyState.layerMask = gameMaster.SystemUtility.TransformToLayerMask(enemyTest.layer);

    }
    public void Tick()
    {
        
    }

    public void FixedTick()
    {
        
    }

    private bool WasHit()
    {

        return false;
    }
}
