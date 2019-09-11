using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
     * The Close Combat Enemies. Pirates and holy Order distinction in the ComponentEnemyState
     * TODO:
     *  -Overwrite Attack
     *  -Overwrite some Movement
     *  -Interprete "AI" instructions and 
     *  -Attack Logic
     */
public class SystemEnemyClose : SystemEnemy
{
    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized
    public enum EnemyType{SIMPLE,ZOMBIE};
    //define the movement of this enemy
    public EnemyType enemyType = EnemyType.SIMPLE;

    //Tmp Variables used for Calculations
    Vector2 movement;
    Vector3 tmp_scale;
    float tmp_xVelocity;
    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;
    float tmp_direction;
    bool leftEdge = false;
    bool rightEdge = false;
    bool leftWall = false;
    bool rightWall = false;
    Collider2D[] toDamageColliders = new Collider2D[10];
    int numberOfOverlaps = 0;
    Vector2 attackDirection;
    int lastDirectionOfZombie = 0;
    float attackLength = 2f;

    private void Start()
    {
        base.Start();
        //do not change direction if the player is hit
        componentEnemyState.layerMask &= componentEnemyState.layerMask &= systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Player"), true);
        componentEnemyAction.timeToAttack = 0.5f;
        componentEnemyAction.attackBoxNormal = new Vector2(attackLength, attackLength);
        componentEnemyAction.isAttacking = false;
        componentEnemyAction.followRange = 5f;
        componentEnemyAction.attackRange = 3f;
        //TODO check - for the sprite direction
        componentEnemyAction.attackPositionOffset = new Vector3(-1f,0,0f);
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatedSpeedAndJumpForce();

        TrackPlayerMovement();

        GroundedCheck();

        WallCheck();

        switch (enemyType)
        {
            case EnemyType.SIMPLE:
                SimpleMovement();
                break;
            case EnemyType.ZOMBIE:
                ZombieMovement();
                break;
            default:
                break;
        }
        Attack(); 
    }

    /*
     * simply update the current speed and jump force of the enemy
     */
    private void UpdatedSpeedAndJumpForce()
    {
        componentEnemyState.currentSpeed = ComponentEnemyState.speed * componentEnemyState.speedMultiplier;
        componentEnemyState.currentJumpForce = ComponentEnemyState.jumpForce * componentEnemyState.jumpForceMultiplier;
    }


    #region movement
    /*
     * check if enemy is on the ground use two ray burst, for checking left and right foot
     */
    private void GroundedCheck()
    {
        //Cast rays for the left and right foot
        leftCheck = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down * 0.49F * componentEnemyState.enemyHeight,
            new Vector2(- ComponentEnemyState.footOffsetLeft, 0f), Vector2.down,
            ComponentEnemyState.groundDistance*2, componentEnemyState.layerMask, drawDebugRaycasts);

        rightCheck = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down * 0.49F * componentEnemyState.enemyHeight,
            new Vector2(ComponentEnemyState.footOffsetRight, 0f), Vector2.down,
            ComponentEnemyState.groundDistance*2, componentEnemyState.layerMask, drawDebugRaycasts);

        //If either ray hit the ground, the player is on the ground and doubleJump gets enabled
        componentEnemyState.isOnGround = leftCheck || rightCheck;
        leftEdge = !leftCheck;
        rightEdge = !rightCheck;

        if (componentEnemyState.isOnGround)
        {
            componentEnemyState.hasJump = true;
        }
    }

    /*
     * check if enemy is abount to hit a wall, left or right
     */
    private void WallCheck()
    {
        //Cast rays for the left and right foot
        leftCheck = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down * 0.2F * componentEnemyState.enemyHeight,
            new Vector2(-ComponentEnemyState.footOffsetLeft, 0f), Vector2.left,
            ComponentEnemyState.groundDistance * 2, componentEnemyState.layerMask, drawDebugRaycasts);

        rightCheck = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down * 0.2F * componentEnemyState.enemyHeight,
            new Vector2(ComponentEnemyState.footOffsetRight, 0f), Vector2.right,
            ComponentEnemyState.groundDistance * 2, componentEnemyState.layerMask, drawDebugRaycasts);

        leftWall = leftCheck;
        rightWall = rightCheck;

    }

    /*
     * Trackplayermovent checks where the payer is, and if he is close enough tho attack
     */
     void TrackPlayerMovement()
    {
        componentEnemyAction.distanceToMainCharacter = Vector2.Distance(mainCharacterGameObject.transform.position, transform.position);
    }

    /*
    * just run from left to right and vice versa, do not interact with player
    */
    private void SimpleMovement()
    {
        //dont move if knockedback
        if (componentEnemyAction.timeUntillKnockBackEnd >= Time.time) return;

        if (leftEdge && !rightEdge || leftWall)
        {
            FlipCharacterDirection(-1);
        }
        else if (rightEdge && !leftEdge || rightWall)
        {
            FlipCharacterDirection(1);
        }

        //TODO fix the minus for componentEnemyState
        tmp_xVelocity = -componentEnemyState.currentSpeed * componentEnemyState.direction;
        rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);


        //Tracking of some State
        componentEnemyState.currentVelocity = rigidBody.velocity;
        componentEnemyState.isMoving = rigidBody.velocity.x > 0.1f || rigidBody.velocity.y > 0.1f || rigidBody.velocity.x < -0.1f || rigidBody.velocity.y < -0.1f;
    }

    /*
    * follow the player if he comes close enough
    * TODO implement
    */
    private void ZombieMovement()
    {
        //dont move if knockedback, check edges
        if (componentEnemyAction.timeUntillKnockBackEnd >= Time.time) return;

        if (leftEdge && !rightEdge || leftWall)
        {
            FlipCharacterDirection(-1);
        }
        else if (rightEdge && !leftEdge || rightWall)
        {
            FlipCharacterDirection(1);
        }

        //check for the player position and flip if needed
        if (componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange)
        {
            //maintain last direction when it was not following the player
            lastDirectionOfZombie = lastDirectionOfZombie == 0 ? componentEnemyState.direction : lastDirectionOfZombie;
            if (mainCharacterGameObject.transform.position.x < transform.position.x)
            {
                FlipCharacterDirection(1);
            }
            else
            {
                FlipCharacterDirection(-1);
            }
        }else if(lastDirectionOfZombie != 0 && componentEnemyState.direction != lastDirectionOfZombie){
            FlipCharacterDirection(lastDirectionOfZombie);
            lastDirectionOfZombie = 0;
        }

        //TODO fix the minus for componentEnemyState
        tmp_xVelocity = -componentEnemyState.currentSpeed * componentEnemyState.direction;
        rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);


        //Tracking of some State
        componentEnemyState.currentVelocity = rigidBody.velocity;
        componentEnemyState.isMoving = rigidBody.velocity.x > 0.1f || rigidBody.velocity.y > 0.1f || rigidBody.velocity.x < -0.1f || rigidBody.velocity.y < -0.1f;
    }

    #endregion
    
    #region attack
    /*
     * Attack the main character
     */
    void Attack()
    {
        Debug.Log(componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.attackRange);
        Debug.Log(componentEnemyAction.distanceToMainCharacter);

        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.attackRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            Debug.Log(componentEnemyState.direction != (transform.position.x < mainCharacterGameObject.transform.position.x ? 1 : -1));
            Debug.Log(componentEnemyState.direction);
            Debug.Log((transform.position.x < mainCharacterGameObject.transform.position.x ? 1 : -1));
        }
        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.attackRange && componentEnemyAction.timeForNextAttack < Time.time && componentEnemyState.direction !=  (transform.position.x < mainCharacterGameObject.transform.position.x ? 1 : -1))
        {
            Debug.Log("Enemy Attack");
            //Debug.Log(componentEnemyState.direction);
            //Debug.Log(componentMainCharacterState.direction);
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;
            //delay the attackdirection of the enemy
            attackDirection = new Vector2(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y);

            numberOfOverlaps = Physics2D.OverlapBoxNonAlloc(transform.position + componentEnemyAction.attackPositionOffset, componentEnemyAction.attackBoxNormal, 0, toDamageColliders, systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Player")));

            ApplyDamage(numberOfOverlaps);

            ResetTempArrays();
        }

        if (componentEnemyAction.isAttacking && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.isAttacking = false;
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
        }
    }

    /*
     * applies damage to the player
     */
     void ApplyDamage(int numberOfOverlaps)
    {
        if (numberOfOverlaps == 0) return;

        mainCharacterMovement.ReceiveDamage(componentEnemyState.damage, transform.position.x < mainCharacterGameObject.transform.position.x ? 1 : -1);
    }
    #endregion
    
    #region hit
    /*
     * let the enemy get hit
     */
    override public void ReceiveDamage(int damage, int direction)
    {
        if (direction != componentEnemyState.direction)
        {
            Debug.Log("Was Block Time:" + Time.time); //TEST
        }
        else
        {
            componentEnemyState.health -= damage;
            Debug.Log("Was hit: " + componentEnemyState.health + " Time:" + Time.time); //TEST
            if (componentEnemyState.health <= 0)
            {
                HandleDieEnemy();
            }

        }
        //direction is the direction where the hit was coming from, so we need to bounce the other direction: - direction
        WasHitKnockBack(-direction);
    }

    #endregion

    /*
     * Flips the dierection of the Gameobject and the State in the Component
     */
    public void FlipCharacterDirection(int newDirection)
    {
        //Turn the character by flipping the direction
        componentEnemyState.direction = newDirection;
        //TODO check - for the sprite direction
        componentEnemyAction.attackPositionOffset.x = -newDirection;
        tmp_scale = transform.localScale;
        tmp_scale.x = componentEnemyState.originalXScale * componentEnemyState.direction;

        //Apply the new scale
        transform.localScale = tmp_scale;
    }

    /*
     * reset toDamageCollider and colliders array
     */
    void ResetTempArrays()
    {
        for (int i = 0; i < toDamageColliders.Length; i++)
        {
            toDamageColliders[i] = null;
        }
    }


    /*
     * just for debug purposes, draws the hitting area of the player
     */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //The next line is causing an Error when not in Play mode
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position, componentEnemyAction.attackRange);
            Gizmos.DrawWireCube(transform.position + componentEnemyAction.attackPositionOffset, new Vector3(componentEnemyAction.attackBoxNormal.x, componentEnemyAction.attackBoxNormal.y, 1f));
        }
    }
}

