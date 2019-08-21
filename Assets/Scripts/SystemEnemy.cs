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
    public GameObject GameLogic;


    //handles
    GameObject mainCharacterGameObject;
    SystemGameMaster systemGameMaster;
    Rigidbody2D rigidBody;
    BoxCollider2D collider2d;
    ComponentEnemyAction componentEnemyAction;
    ComponentEnemyState componentEnemyState;
    void Start()
    {
        systemGameMaster = GameLogic.GetComponent<SystemGameMaster>();
        componentEnemyState = GetComponent<ComponentEnemyState>();
        componentEnemyAction = GetComponent<ComponentEnemyAction>();
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();


        //Sets Layermask of enemy
        componentEnemyState.layerMask = systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Enemy"));

        RegisterEnemy();

    }

    void RegisterEnemy()
    {
        systemGameMaster.RegisterNewEnemy(gameObject);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }


    /*
     * let the enemy get hit
     */
    public void ReceiveDamage(int damage)
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
    private void HandleDieEnemy()
    {
        Destroy(gameObject);
    }
}
