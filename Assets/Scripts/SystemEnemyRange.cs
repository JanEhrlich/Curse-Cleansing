using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Range Combat Enemies. Pirates and holy Order distinction in the ComponentEnemyState
 * TODO:
 *  -Overwrite Attack
 *  -Overwrite some Movement, because the can not jump
 *  -Handle Reload/Magic Cooldown in Component State
 *  -Interprete "AI" instructions and 
 *  -Attack Logic
 */
public class SystemEnemyRange : SystemEnemy
{
    bool debugRayCasts = true;

    //Tmp Variables used for Calculations
    Vector3 tmp_scale;
    float tmp_direction;
    RaycastHit2D attackHit;
    bool isAttacking = false;
    Vector2 attackDirection;
    void Start()
    {
        base.Start();
        componentEnemyState.currentSpeed = 0;
        componentEnemyAction.followRange = 15f;
    }
    void Update()
    {
    }

    void FixedUpdate()
    {
        TrackPlayerMovement();

        Attack();
    }

 /*
 * Trackplayermovent checks where the payer is, and if he is close enough tho attack
 */
    void TrackPlayerMovement()
    {
        componentEnemyAction.distanceToMainCharacter = Vector3.Distance(mainCharacterGameObject.transform.position, transform.position);
        systemGameMaster.SystemUtility.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero, attackDirection, 10f, componentEnemyState.layerMask, debugRayCasts);
        if (componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange)
        {
            if (mainCharacterGameObject.transform.position.x < transform.position.x)
            {
                FlipCharacterDirection(-1);

            }
            else
            {
                FlipCharacterDirection(1);
            }
        }


    }

    /*
     * Attack the main character
     */
     void Attack()
    {
        if (!isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            isAttacking = true;
            //delay the attackdirection of the enemy
            attackDirection = new Vector2(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y);
        }

        if(isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            attackHit = systemGameMaster.SystemUtility.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero, attackDirection,Mathf.Infinity,componentEnemyState.layerMask, debugRayCasts);
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            isAttacking = false;
            Debug.Log(attackHit.collider.gameObject.layer);

            //TODO fix the dirty check for layer, into something better
            if (attackHit && attackHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                mainCharacterMovement.ReceiveDamage(componentEnemyState.damage, transform.position.x < mainCharacterGameObject.transform.position.x? 1: -1);
            }
        }

    }


    /*
    * Flips the dierection of the Gameobject and the State in the Component
    */
    void FlipCharacterDirection(int newDirection)
    {
        //Turn the character by flipping the direction
        componentEnemyState.direction = newDirection;
        //TODO let enemy attack
        componentEnemyAction.attackPositionOffset.x = newDirection;
        tmp_scale = transform.localScale;
        tmp_scale.x = componentEnemyState.originalXScale * componentEnemyState.direction;

        //Apply the new scale
        transform.localScale = tmp_scale;
    }


    /*
     * just for debug purposes, draws the hitting area of the player
     */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //The next line is causing an Error when not in Play mode
        Gizmos.DrawWireSphere(transform.position,componentEnemyAction.followRange);
    }

}
