using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Pirate Boss Enemy of the Pirate Ship Level.
 *  -Overwrite Attack
 *  -Overwrite some Movement
 *  -Interprete "AI" instructions and 
 *  -Attack Logic
 */
public class SystemEnemyPirateBoss : SystemEnemy
{

    bool debugRayCasts = true;
    Vector3 debugOffset;
    Vector3 debugAttackbox;

    public enum BossStage { NORMAL, THREESHOT, COMBO, SKULLS, JUMP, RUN};

    //handles
    GameObject rangeAttackMisslePrefab;
    GameObject flyingSkullPrefab;

    //set attack range multiplier in inspector
    public float attackrangeMultiplier = 6f;
    public float maxSpeed = 3f;

    //Tmp Variables used for Calculations
    Vector3 tmp_scale;
    float tmp_direction;
    RaycastHit2D attackHit;
    GameObject bullet;
    GameObject flyingSkull;
    Quaternion rotation;
    Vector2 movement;
    float tmp_xVelocity;
    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;
    RaycastHit2D ground;
    bool leftEdge = false;
    bool rightEdge = false;
    bool leftWall = false;
    bool rightWall = false;

    //use for close compat
    Collider2D[] toDamageColliders = new Collider2D[10];
    int numberOfOverlaps = 0;
    float attackLength = 2f;
    Vector2 attackBoxCombo1 = new Vector2(8f,3f);
    Vector2 attackBoxCombo2 = new Vector2(8f,3f);
    Vector2 attackBoxCombo3 = new Vector2(10f,8f);

    //use for checking which attack next
    BossStage stage = BossStage.NORMAL;

    //use for shooting
    Vector3 attackDirection;
    float rotZ;
    public float spreadRange = 2f;
    const float timeForThreeShot = 0.2f;
    float timeForNextThreeShot = 0f;
    float offsetBullet = 0.2f;
    int numberOFShots = 0;

    //use invulnerability if he was hit 3 times
    float invulnerableTime = 2f;
    float timeUntillvulnerable = 0f;
    int hitcounter = 0;

    private void Start()
    {
        base.Start();
        //do not change direction if the player is hit
        componentEnemyState.layerMask &= componentEnemyState.layerMask & systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Player"), true);
        componentEnemyAction.timeToAttack = 0.5f;
        componentEnemyAction.attackBoxNormal = new Vector2(attackLength, attackLength);
        componentEnemyAction.isAttacking = false;
        componentEnemyState.currentSpeed = 0;
        componentEnemyAction.followRange *= attackrangeMultiplier;
        componentEnemyState.health = 15;

        rangeAttackMisslePrefab = Resources.Load("Bullet") as GameObject;
        flyingSkullPrefab = Resources.Load("FlyingSkull") as GameObject;

        //TODO check - for the sprite direction
        componentEnemyAction.attackPositionOffset = new Vector3(-1f, 0, 0f);
        //componentEnemyState.speedMultiplier = 2f;
    }

    void FixedUpdate()
    {
        UpdatedSpeedAndJumpForce();

        GroundedCheck();

        WallCheck();

        Movement();

        TrackPlayerMovement();


        if(timeUntillvulnerable < Time.time) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,255f);
        }

        ChooseAttack();

        switch (stage)
        {
            case BossStage.NORMAL:
                AttackNormalRange();
                break;
            case BossStage.COMBO:
                ComboAttackClose();
                break;
            case BossStage.SKULLS:
                SummonFlyingSkulls();
                break;
            case BossStage.THREESHOT:
                ThreeShotAttack();
                break;
            case BossStage.JUMP:
                BigJump();
                break;
            case BossStage.RUN:
                FastRun();
                break;                

        }
    }

    /*
     * simply update the current speed and jump force of the enemy
     */
    private void UpdatedSpeedAndJumpForce()
    {
        componentEnemyState.speedMultiplier = componentEnemyAction.distanceToMainCharacter*4 / componentEnemyAction.followRange;
        componentEnemyState.speedMultiplier = componentEnemyState.speedMultiplier < maxSpeed? componentEnemyState.speedMultiplier : maxSpeed;
        componentEnemyState.currentSpeed = ComponentEnemyState.speed * componentEnemyState.speedMultiplier;
        componentEnemyState.currentJumpForce = ComponentEnemyState.jumpForce * componentEnemyState.jumpForceMultiplier;
    }

    //TODO can make a pattern here
    void ChooseAttack()
    {

    }


    void Movement()
    {
        if(!componentEnemyState.isOnGround)
        {
        //TODO maby need the minus for other sprite
        tmp_xVelocity = componentEnemyState.currentSpeed * componentEnemyState.direction;
        rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);
        }


        if(componentEnemyState.isRunning)
        {
        //TODO maby need the minus for other sprite
        //runspeed 
        tmp_xVelocity = 10f * componentEnemyState.direction;
        rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);
        }

        //Tracking of some State
        componentEnemyState.currentVelocity = rigidBody.velocity;
        componentEnemyState.isMoving = rigidBody.velocity.x > 0.1f || rigidBody.velocity.y > 0.1f || rigidBody.velocity.x < -0.1f || rigidBody.velocity.y < -0.1f;
        
    }

    /*
    * Trackplayermovent checks where the payer is, and if he is close enough to attack
    */
    void TrackPlayerMovement()
    {
        componentEnemyAction.distanceToMainCharacter = Vector2.Distance(mainCharacterGameObject.transform.position, transform.position);
        systemGameMaster.SystemUtility.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero, attackDirection, componentEnemyAction.followRange, componentEnemyState.layerMask, debugRayCasts);
        if (componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyState.isOnGround && !componentEnemyState.isRunning)
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
     * check if enemy is abount to hit a wall, left or right
     */
    private void WallCheck()
    {
        //Cast rays for the left and right foot
        leftCheck = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down * 2f,
            new Vector2(-1f, 0f), Vector2.left, 1.5f, componentEnemyState.layerMask, debugRayCasts);

        rightCheck = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down * 2f,
            new Vector2(1f, 0f), Vector2.right, 1.5f, componentEnemyState.layerMask, debugRayCasts);

        leftWall = leftCheck;
        rightWall = rightCheck;

    }


    /*
     * Attack the main character with range
     */
    void AttackNormalRange()
    {

        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;

            attackDirection = new Vector3(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y, 0f);
            attackDirection.Normalize();

            //rotate the bullet
            //roate only around z, this math-formula give us the angle between the maincaracter and the enemy
            rotZ = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, rotZ);

            //create the bullet
            bullet = Instantiate(rangeAttackMisslePrefab, transform.position + 3f * attackDirection.normalized, rotation);
            bullet.GetComponent<SystemBullet>().SetDirection(attackDirection.normalized);
        }

        if (componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            componentEnemyAction.isAttacking = false;
            stage = BossStage.COMBO;
        }

    }

    #region attackClose

    /*
     * Big Swing moderate range, can jump over
     */
    void NormalAttackClose(Vector3 offset, Vector2 attackBox)
    {
        Debug.Log("Enemy Attack");
        //delay the attackdirection of the enemy
        attackDirection = new Vector2(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y);
        debugOffset = offset; //DEBUG
        debugAttackbox = (Vector3)attackBox;
        numberOfOverlaps = Physics2D.OverlapBoxNonAlloc(transform.position + offset, attackBox, 0, toDamageColliders, systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Player")));

        ApplyDamage(numberOfOverlaps);

        ResetTempArrays();
    }

    /*
     * three close range swings, each moves a bit towars the player, last attack with more height, can not jump easily over
     */
    void ComboAttackClose()
    {
        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time || numberOFShots > 0)
        {

            switch (numberOFShots)
            {
                case 0:
                    //important set timeToAttack high enough, so that the whole attack ca be carried out
                    componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack*2;
                    componentEnemyAction.isAttacking = true;
                    NormalAttackClose(componentEnemyAction.attackPositionOffset * 4f -2f*Vector3.up,attackBoxCombo1);
                    timeForNextThreeShot = Time.time + timeForThreeShot*3f;
                    numberOFShots++;
                    break;
                case 1:
                    if (timeForNextThreeShot > Time.time) return;
                    NormalAttackClose(componentEnemyAction.attackPositionOffset * 4f + 2f * Vector3.up, attackBoxCombo2);
                    timeForNextThreeShot = Time.time + timeForThreeShot*3f;
                    numberOFShots++;
                    Debug.Log(numberOFShots);
                    break;
                case 2:
                    if (timeForNextThreeShot > Time.time) return;
                    NormalAttackClose(componentEnemyAction.attackPositionOffset * 4f - 0f * Vector3.up, attackBoxCombo3);
                    numberOFShots = 0;
                    break;
            }


        }

        if (componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            componentEnemyAction.isAttacking = false;
            stage = BossStage.THREESHOT;
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

    /*
     * Range attack, three quick shots 
     */
    void ThreeShotAttack()
    {
        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time || numberOFShots > 0)
        {

            switch (numberOFShots)
            {
                case 0:
                    componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
                    componentEnemyAction.isAttacking = true;
                    ShootBullet(-offsetBullet, -spreadRange);
                    timeForNextThreeShot = Time.time + timeForThreeShot;
                    numberOFShots++;
                    break;
                case 1:
                    if (timeForNextThreeShot > Time.time) return;
                    ShootBullet(0, 0);
                    timeForNextThreeShot = Time.time + timeForThreeShot;
                    numberOFShots++;
                    break;
                case 2:
                    if (timeForNextThreeShot > Time.time) return;
                    ShootBullet(offsetBullet, spreadRange);
                    numberOFShots = 0;
                    break;
            }


        }

        if (componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            componentEnemyAction.isAttacking = false;
            stage = BossStage.SKULLS;
        }
    }

    /*
     * shoot the bullet
     */
     void ShootBullet(float offset, float spread)
    {
        attackDirection = new Vector3(mainCharacterGameObject.transform.position.x - transform.position.x, mainCharacterGameObject.transform.position.y - transform.position.y + spread, 0f);
        attackDirection.Normalize();
        //rotate the bullet
        //roate only around z, this math-formula give us the angle between the maincaracter and the enemy
        rotZ = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0f, 0f, rotZ);
        //create the bullet
        bullet = Instantiate(rangeAttackMisslePrefab, transform.position +Vector3.down *offset + 3f * attackDirection.normalized, rotation);
        bullet.GetComponent<SystemBullet>().SetDirection(attackDirection.normalized);
    }


    /*
     * summon between 2 and 4 flying skulls, direction towards the player
     */
    void SummonFlyingSkulls()
    {
        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time || numberOFShots > 0)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;
            switch (numberOFShots)
            {
                case 0:
                    componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
                    componentEnemyAction.isAttacking = true;
                    SummonSkull(-2f);
                    timeForNextThreeShot = Time.time + timeForThreeShot;
                    numberOFShots++;
                    break;
                case 1:
                    if (timeForNextThreeShot > Time.time) return;
                    SummonSkull(2f);
                    timeForNextThreeShot = Time.time + timeForThreeShot;
                    numberOFShots++;
                    break;
                case 2:
                    if (timeForNextThreeShot > Time.time) return;
                    SummonSkull(0f);
                    numberOFShots = 0;
                    break;
            }
            
            
            
        }
        if (componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            componentEnemyAction.isAttacking = false;
            stage = BossStage.JUMP;
        }
    }

    /*
     * summons a flying skull
     */
     void SummonSkull(float offset = 0f)
    {
        flyingSkull = Instantiate(flyingSkullPrefab, transform.position + Vector3.down * offset + 3f * Vector3.right* (mainCharacterGameObject.transform.position.x <= transform.position.x ? -1 : 1), transform.rotation);
        flyingSkull.GetComponent<SystemEnemyFlyingSkull>().UpdateDirection(mainCharacterGameObject.transform.position.x <= transform.position.x ? SystemEnemyFlyingSkull.Direction.LEFT: SystemEnemyFlyingSkull.Direction.RIGHT);
    }

    /*
     * jump on the player
     */
     void BigJump()
    {
        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time || numberOFShots > 0)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;

            //remove y-velocity, so that one can not use edges to boost
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            componentEnemyState.isOnGround = false;
            rigidBody.AddForce(new Vector2(0f, componentEnemyState.currentJumpForce*1.6f), ForceMode2D.Impulse);
        }
        if (componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks/3f;
            componentEnemyAction.isAttacking = false;
            stage = BossStage.RUN;
        }
    }

        /*
    * Checks if Player is grounded using 2 raycasts
    */
    private void GroundedCheck()
    {
        //Cast rays for the left and right foot
        ground = systemGameMaster.SystemUtility.Raycast(transform.position + Vector3.down, Vector2.zero, Vector2.down, 2f, componentEnemyState.layerMask, debugRayCasts);

        componentEnemyState.isOnGround = ground;
    }

    /*
     * run towards the player
     */
    void FastRun()
    {
        if(leftWall || rightWall)
        {
            componentEnemyState.isRunning = false;
            if(leftWall)
            {
                tmp_xVelocity = 1f;
                rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);

                //Tracking of some State
                componentEnemyState.currentVelocity = rigidBody.velocity;
            }
            else
            {
                tmp_xVelocity = -1f;
                rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);

                //Tracking of some State
                componentEnemyState.currentVelocity = rigidBody.velocity;
            }
        }

        if (!componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time || numberOFShots > 0)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeToAttack;
            componentEnemyAction.isAttacking = true;
            componentEnemyState.isRunning = true;
        }

        if(componentEnemyState.isRunning)
        {
            componentEnemyAction.timeForNextAttack = Time.time +  componentEnemyAction.timeBetweenAttacks/3f;
        }


        if (componentEnemyAction.isAttacking && componentEnemyAction.distanceToMainCharacter <= componentEnemyAction.followRange && componentEnemyAction.timeForNextAttack < Time.time)
        {
            componentEnemyAction.timeForNextAttack = Time.time + componentEnemyAction.timeBetweenAttacks;
            componentEnemyAction.isAttacking = false;
            componentEnemyState.isRunning = false;
            stage = BossStage.NORMAL;
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
        tmp_scale.x = componentEnemyState.originalXScale * componentEnemyState.direction;
        //Apply the new scale
        transform.localScale = tmp_scale;
    }

    #region handleHit

    /*
     * let the enemy get hit
     *  TODO Make invulnerable if hit
     */
    public override void ReceiveDamage(int damage, int direction)
    {
        if (timeUntillvulnerable > Time.time) return;
        componentEnemyState.health -= damage;
        Debug.Log("Was hit: " + componentEnemyState.health + " Time:" + Time.time); //TEST
        if (componentEnemyState.health <= 0)
        {
            HandleDieEnemy();
        }

        hitcounter++;
        Debug.Log("invulnerable:"+hitcounter);
        if (hitcounter > 0 && hitcounter % 3 == 0)
        {
            timeUntillvulnerable = Time.time + invulnerableTime;
            componentEnemyAction.timeForNextAttack = timeUntillvulnerable;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    #endregion
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
        if (Application.isPlaying)
        {
            //Gizmos.DrawWireSphere(transform.position, componentEnemyAction.followRange);
            Gizmos.DrawWireCube(transform.position + debugOffset, debugAttackbox);
        }
    }
}
