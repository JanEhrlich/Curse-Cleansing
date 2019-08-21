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
    //drag an drop in inspector
    GameObject gameLogic;


    //handles
    protected GameObject mainCharacterGameObject;
    protected SystemGameMaster systemGameMaster;
    protected Rigidbody2D rigidBody;
    protected BoxCollider2D collider2d;
    protected ComponentEnemyAction componentEnemyAction;
    protected ComponentEnemyState componentEnemyState;
    protected void Start()
    {
        componentEnemyState = GetComponent<ComponentEnemyState>();
        componentEnemyAction = GetComponent<ComponentEnemyAction>();
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        rigidBody = GetComponent<Rigidbody2D>();
        collider2d = GetComponentInChildren<BoxCollider2D>();
        componentEnemyState.enemyHeight = collider2d.size.y;

        //Sets Layermask of enemy
        componentEnemyState.layerMask = systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Enemy"), true);
        //TODO fix that the camera is player layer, not good, messes with raycasts this
        componentEnemyState.layerMask &= systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Player"), true);

        RegisterEnemy();

    }

    void RegisterEnemy()
    {
        systemGameMaster.RegisterNewEnemy(gameObject);
    }

    /*
     * let the enemy get hit
     */
    public virtual void ReceiveDamage(int damage)
    {
        componentEnemyState.health -= damage;
        Debug.Log("Was hit: " + componentEnemyState.health); //TEST
        if (componentEnemyState.health <= 0)
        {
            HandleDieEnemy();
        }
    }

    /*
     * Handle the death of the enemy
     */
    protected virtual void HandleDieEnemy()
    {
        Destroy(gameObject);
    }

}
