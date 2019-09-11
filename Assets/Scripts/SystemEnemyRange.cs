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

    //handles
    GameObject rangeAttackMisslePrefab;

    //set attack range multiplier in inspector
    public float attackrangeMultiplier = 3f;

    //Tmp Variables used for Calculations
    Vector3 tmp_scale;
    float tmp_direction;
    RaycastHit2D attackHit;
    Vector3 attackDirection;
    GameObject bullet;
    Quaternion rotation;
    float rotZ;

    void Start()
    {
        base.Start();
        componentEnemyState.currentSpeed = 0;
        componentEnemyAction.followRange *= attackrangeMultiplier;
        rangeAttackMisslePrefab = Resources.Load("Bullet") as GameObject;
        componentEnemyAction.isAttacking = false;
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
    * Trackplayermovent checks where the payer is, and if he is close enough to attack
    */
    void TrackPlayerMovement()
    {
        componentEnemyAction.distanceToMainCharacter = Vector2.Distance(mainCharacterGameObject.transform.position, transform.position);
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

        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;
            
            attackDirection = new Vector3(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y,0f);
            attackDirection.Normalize();

            //rotate the bullet
            //roate only around z, this math-formula give us the angle between the maincaracter and the enemy
            rotZ = Mathf.Atan2(attackDirection.y,attackDirection.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, rotZ);

            //create the bullet
            bullet = Instantiate(rangeAttackMisslePrefab, transform.position+ 2f* attackDirection.normalized, rotation);
            bullet.GetComponent<SystemBullet>().SetDirection(attackDirection.normalized);
        }

        if(componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            componentEnemyAction.isAttacking = false;
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
