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
    protected GameObject gameLogic;
    protected GameObject mainCharacterGameObject;
    protected SystemGameMaster systemGameMaster;
    protected Rigidbody2D rigidBody;
    protected BoxCollider2D collider2d;
    protected ComponentEnemyAction componentEnemyAction;
    protected ComponentEnemyState componentEnemyState;
    protected ComponentMainCharacterState componentMainCharacterState;
    protected SystemMainCharacterMovement mainCharacterMovement;
    protected virtual void Start()
    {
        componentEnemyState = new ComponentEnemyState(); //GetComponent<ComponentEnemyState>();
        componentEnemyAction = new ComponentEnemyAction(); //GetComponent<ComponentEnemyAction>();
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        mainCharacterMovement = gameLogic.GetComponent<SystemMainCharacterMovement>();
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        rigidBody = GetComponent<Rigidbody2D>();
        collider2d = GetComponentInChildren<BoxCollider2D>();
        componentEnemyState.enemyHeight = collider2d.size.y;
        componentEnemyState.originalXScale = transform.localScale.x;

        //Sets Layermask of enemy
        componentEnemyState.layerMask = systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Enemy"), true);
        //TODO check if this is really the best solution for camera bounds
        componentEnemyState.layerMask &= systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Camera"), true);

        RegisterEnemy();

    }

    void RegisterEnemy()
    {
        systemGameMaster.RegisterNewEnemy(gameObject);
    }

    /*
     * let the enemy get hit
     */
    public virtual void ReceiveDamage(int damage, int direction)
    {
        componentEnemyState.health -= damage;
        Debug.Log("Was hit: " + componentEnemyState.health + " Time:"+Time.time); //TEST
        if (componentEnemyState.health <= 0)
        {
            HandleDieEnemy();
        }

        //direction is the direction where the hit was coming from, so we need to bounce the other directino: - direction
        WasHitKnockBack(-direction);
    }

    /*
     * Handle the death of the enemy
     */
    protected virtual void HandleDieEnemy()
    {
        Destroy(gameObject);
    }

    /*
     * knocks back the enemy
     */
    protected void WasHitKnockBack(int knockBackdirection)
    {

        if (componentEnemyAction.timeUntillKnockBackEnd < Time.time)
        {
            componentEnemyAction.timeUntillKnockBackEnd = Time.time + ComponentEnemyAction.knockBackTime;
            rigidBody.velocity = Vector2.zero;
            rigidBody.velocity = new Vector2(knockBackdirection*ComponentEnemyAction.knowBackPowerHorizontal,ComponentEnemyAction.knockBackPowerUp);
        }
    }

    /*
     * damage and knock back the player, if he runs into the enemy
     */
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //throw the enemy back, but not as hard, therefore 1/2
            rigidBody.velocity = new Vector2((mainCharacterGameObject.transform.position.x <= transform.position.x ? 1 : -1) * ComponentEnemyAction.knowBackPowerHorizontal/2, ComponentEnemyAction.knockBackPowerUp/2 );
            componentEnemyAction.timeUntillKnockBackEnd = Time.time + ComponentEnemyAction.knockBackTime/2;

            //throw the player back
            gameLogic.GetComponent<SystemMainCharacterMovement>().ReceiveDamage(componentEnemyState.damage, mainCharacterGameObject.transform.position.x <= transform.position.x ? -1 : 1);
        }
    }

}
