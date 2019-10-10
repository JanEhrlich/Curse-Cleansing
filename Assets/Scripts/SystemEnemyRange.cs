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
    GameObject arm;
    //set attack range multiplier in inspector
    public float attackrangeMultiplier = 3f;

    //Tmp Variables used for Calculations
    Quaternion defaultPositionArm;
    Quaternion armRotation;
    Vector3 tmp_scale;
    float tmp_direction;
    RaycastHit2D attackHit;
    Vector3 attackDirection;
    GameObject bullet;
    Quaternion rotation;
    float rotZ;
    float attackdelay = 0.5f;
    float timeAfterAttackdelay = 0f;
    bool afterAttackDelay = false;

    float rotX = 0f;
    float rotY = 0f;

    void Awake()
    {
        base.Awake();
        componentEnemyState.currentSpeed = 0;
        componentEnemyAction.followRange *= attackrangeMultiplier;
        rangeAttackMisslePrefab = Resources.Load("Bullet") as GameObject;
        arm = gameObject.transform.GetChild(0).gameObject;
        defaultPositionArm = arm.transform.rotation;
        componentEnemyAction.isAttacking = false;
        componentEnemyAction.timeForNextAttack = Time.time + 3f;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        TrackPlayerMovement();

        if(allowAttack)
            Attack();

        //set arm in default position 
        if(!allowAttack || componentEnemyAction.distanceToMainCharacter > componentEnemyAction.followRange){
            rotZ = 270f;
            rotation = Quaternion.Euler(0f, 0f, rotZ);
            armRotation = Quaternion.Euler(rotX, rotY, rotZ);
            arm.transform.rotation = armRotation;
        }
    }

    /*
    * Trackplayermovent checks where the payer is, and if he is close enough to attack
    */
    void TrackPlayerMovement()
    {
        //Arm following the player
        attackDirection = new Vector3(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y, 0f);
        attackDirection.Normalize();


        componentEnemyAction.distanceToMainCharacter = Vector2.Distance(mainCharacterGameObject.transform.position, transform.position);
        attackHit = systemGameMaster.SystemUtility.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero, attackDirection, componentEnemyAction.followRange, componentEnemyState.layerMask, debugRayCasts);
        if(attackHit != null && attackHit.transform != null && attackHit.transform.gameObject != null ) allowAttack = attackHit.transform.gameObject.layer == LayerMask.NameToLayer("Player");
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
        if (componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange)
        {
            //rotate the bullet
            //roate only around z, this math-formula give us the angle between the maincaracter and the enemy
            rotZ = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, rotZ);
            armRotation = Quaternion.Euler(rotX, rotY, rotZ);
            arm.transform.rotation = armRotation;

        }


        if (!afterAttackDelay && !componentEnemyAction.isAttacking &&  componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange &&componentEnemyAction.timeForNextAttack < Time.time)
        {
            afterAttackDelay = true;
            timeAfterAttackdelay = Time.time + attackdelay;
        }
        if(afterAttackDelay && timeAfterAttackdelay <= Time.time &&  componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange)
        {
            afterAttackDelay = false;
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;

            //TODO make timings
            gameObject.GetComponent<Animator>().Play("Shoot");
            gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Arm_Shoot");

            //create the bullet
            bullet = Instantiate(rangeAttackMisslePrefab, transform.position+ 2f* attackDirection.normalized + Vector3.up * 0.4f, rotation);
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
    public void FlipCharacterDirection(int newDirection)
    {
        //Turn the character by flipping the direction
        componentEnemyState.direction = newDirection;
        //TODO let enemy attack
        componentEnemyAction.attackPositionOffset.x = newDirection;
        tmp_scale = transform.localScale;
        rotX = newDirection == 1? 0 : 180;
        rotY = newDirection == 1 ? 0 : 180;
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
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position, componentEnemyAction.followRange);
        }
    }

}
