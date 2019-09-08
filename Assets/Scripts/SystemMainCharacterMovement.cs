﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/*
 * this class implements the main character movement
 * 
 * handled by Game-Master-System
 * 
 * TODO:
 *  -left, right, jumps
 *  -check bounds, walls, ceilings, etc.
 *  -is able to move?
 *  -attacking, check if attack is finished
 */

public class SystemMainCharacterMovement : MonoBehaviour
{
    public bool drawDebugRaycasts = true;	//Should the environment checks be visualized

    //handles
    GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    SystemGameMaster systemGameMaster;
    ComponentInput componentInput;
    ComponentMainCharacterAction componentMainCharacterAction;
    ComponentMainCharacterState componentMainCharacterState;
    ComponentKrakenMarker componentKrakenMarker;
    Transform mainCharacterTransform;
    SystemUtility utility;
    PolygonCollider2D attackArea;               // used if collider attack is used

    //Variable used to process jumping and gliding
    bool receivedJumpFlag;
    bool holdJumpButton;

    //Variable used for attacking
    bool receivedAttackFlag = false;

    //Variable used for kraken ability
    bool receivedKrakenFlag = false;

    //Used to avoid setting jupedOnce back to false because Jump couldn't build up distance to make GroundedCheck false
    bool skipNextFrame;

    //Tmp Variables used for Calculations
    Vector2 movement;
    Vector3 tmp_scale;
    float tmp_xVelocity;
    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;
    Vector2 tmp_direction;

    private float attackLength = 2f;

    int numberOfOverlaps = 0;
    //should be equally long
    Collider2D[] enemyToDamageColliders = new Collider2D[10];

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        mainCharacterTransform = mainCharacterGameObject.transform;

        //Get Components
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;
        utility = systemGameMaster.SystemUtility;
        componentKrakenMarker = systemGameMaster.ComponentKrakenMarker;

        //attackArea = mainCharacterGameObject.GetComponentInChildren<PolygonCollider2D>();     // used if collider attack is used
        //attackArea.enabled = false;                                                           // used if collider attack is used

        InitPlayerAttackRanges();

        //Sets Layermask of mainCharacter
        componentMainCharacterState.layerMask = gameMaster.SystemUtility.TransformToLayerMask(mainCharacterGameObject.layer,true);
        componentMainCharacterState.layerMask &= gameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Camera"), true);

        //Record the original x scale of the player
        componentMainCharacterState.originalXScale = mainCharacterGameObject.transform.localScale.x;
        componentMainCharacterState.isAttacking = false;
        componentMainCharacterState.isGliding = false;
        componentMainCharacterState.normalGravity = rigidBody.gravityScale;

        //add button functions
        componentInput.AddJumpButtonPressFunction(ReceiveJumpPressInput);
        componentInput.AddJumpButtonCancelFunction(ReceiveJumpCancelInput);
        componentInput.AddAttackButtonPressFunction(ReceiveAttackPressInput);
        componentInput.AddTentacleButtonPressFunction(ReceiveKrakenPressInput);

        //Set normal Attack duration
        componentMainCharacterAction.waitingTime = ComponentMainCharacterAction.durationAttackNormal;
    }

    void InitPlayerAttackRanges()
    {
        componentMainCharacterAction.attackBoxNormal = new Vector2(attackLength, attackLength);
        componentMainCharacterAction.attackBoxKraken = new Vector2(attackLength * 3f, attackLength);
        componentMainCharacterAction.attackBoxBat = new Vector2(attackLength * 2f, attackLength);

        componentMainCharacterAction.currentAttackBox = componentMainCharacterAction.attackBoxNormal;

        componentMainCharacterAction.attackPositionHorizontalOffset = new Vector3(1f, 0f, 0f);
        componentMainCharacterAction.attackPositionOffset = componentMainCharacterAction.attackPositionHorizontalOffset;
        componentMainCharacterAction.attackPositionVerticalOffset = new Vector3(0f, 1f, 0f);
    }

    public void Tick()
    {
        
    }

    public void FixedTick()
    {
        ResetImpulses();
        UpdatedSpeedAndJumpForce();
        UpdateAttackRange();

        if (skipNextFrame)
        {
            skipNextFrame = false;            
        }
        else
        {
            GroundedCheck();
        }

        //dont move if knockedback, but let move after knock back is finished
        //TODO maybe make own vairable for this
        if (componentMainCharacterAction.timeUntillKnockBackEnd-1.15f >= Time.time) return;

        HorizontalMovement();
        VerticalLooking();

        if (receivedJumpFlag)
        {
            HandelJumpInstruction();
            receivedJumpFlag = false;
        }

        HandleGliding();
        
        HandleAttackInstruction();  
        
        CheckTimers();
        #region checkKrakenAbility

        if (receivedKrakenFlag)
        {
            HandleKrakenInstruction();
        }
        receivedKrakenFlag = false;

        if (componentMainCharacterAction.isUsingKrakenPull)
        {
            MoveToKrakenMarker();
        }

        #endregion

        CapFallingSpeed();
    }

    /*
    * Calculates the current speed and jumpForce based on set speedMultiplier and jumpForceMultiplier.
    */
    private void UpdatedSpeedAndJumpForce()
    {
        componentMainCharacterState.currentSpeed = ComponentMainCharacterState.speed * componentMainCharacterState.speedMultiplier;
        componentMainCharacterState.currentJumpForce = ComponentMainCharacterState.jumpForce * componentMainCharacterState.jumpForceMultiplier;
    }

    void UpdateAttackRange()
    {
        if (receivedKrakenFlag)
        {
            componentMainCharacterAction.currentAttackBox = componentMainCharacterAction.attackBoxKraken;
        }
        else
        {
            componentMainCharacterAction.currentAttackBox = componentMainCharacterAction.attackBoxNormal;
        }
    }

    private void CheckTimers()
    {
        //make player not move through enemys, as the damage protection is running out
        if (componentMainCharacterAction.timeUntillKnockBackEnd < Time.time)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
    }


    #region handleMoving

    /*
    * Checks if Player is grounded using 2 raycasts
    */
    private void GroundedCheck()
    {
        //Cast rays for the left and right foot
        leftCheck = systemGameMaster.SystemUtility.Raycast(mainCharacterGameObject.transform.position + Vector3.down * 0.49F * componentMainCharacterState.playerHeight, 
            new Vector2(-ComponentMainCharacterState.footOffsetLeft, 0f), Vector2.down, 
            ComponentMainCharacterState.groundDistance, componentMainCharacterState.layerMask, drawDebugRaycasts);

        rightCheck = systemGameMaster.SystemUtility.Raycast(mainCharacterGameObject.transform.position + Vector3.down * 0.49F * componentMainCharacterState.playerHeight,
            new Vector2(ComponentMainCharacterState.footOffsetRight, 0f), Vector2.down, 
            ComponentMainCharacterState.groundDistance, componentMainCharacterState.layerMask, drawDebugRaycasts);

        //If either ray hit the ground, the player is on the ground and doubleJump gets enabled
        componentMainCharacterState.isOnGround = leftCheck || rightCheck;


        if (componentMainCharacterState.isOnGround)
        {
            componentMainCharacterState.hasJump = true;
            componentMainCharacterAction.hasDoubleJump = true;
        }
    }

    /*
     * Handels the horizontal movement based on inputs
     */
    void HorizontalMovement()
    {
        tmp_xVelocity = componentMainCharacterState.currentSpeed * componentInput.getCurrentHorizontalJoystickPosition();
        rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);

        //Tracking of some State
        componentMainCharacterState.currentVelocity = rigidBody.velocity;
        componentMainCharacterState.isMoving = rigidBody.velocity.x > 0.1f || rigidBody.velocity.y > 0.1f || rigidBody.velocity.x < -0.1f || rigidBody.velocity.y < -0.1f;

        //If the sign of the velocity and direction don't match, flip the character
        if (tmp_xVelocity * componentMainCharacterState.direction < 0f)
            FlipCharacterDirection();

        //If the player is on the ground, extend the coyote time window
        if (componentMainCharacterState.isOnGround)
            componentMainCharacterState.coyoteTime = Time.time + ComponentMainCharacterState.coyoteDuration;
    }

    /*
     * Handles changing the hitbox if looking up or down
     */
    void VerticalLooking()
    {
        if (componentInput.getCurrentJoystickAxis().y == 0)
        {
            componentMainCharacterAction.attackPositionOffset = componentMainCharacterAction.attackPositionHorizontalOffset;
            componentMainCharacterAction.attackPositionOffset.x *= componentMainCharacterState.direction;
        }
        else
        {
            componentMainCharacterAction.attackPositionOffset = componentMainCharacterAction.attackPositionVerticalOffset;
            componentMainCharacterAction.currentAttackBox = new Vector2(componentMainCharacterAction.currentAttackBox.y, componentMainCharacterAction.currentAttackBox.x);
            if (componentInput.getCurrentJoystickAxis().y < 0)
            {
                componentMainCharacterAction.attackPositionOffset.y *= -1;
            }
        }

        if (receivedKrakenFlag)
        {
            componentMainCharacterAction.attackPositionOffset *= ComponentMainCharacterAction.krakenAttackRangeMultiplier;
        }
    }

    /*
     * Flips the dierection of the Gameobject and the State in the Component
     */
    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        componentMainCharacterState.direction *= -1;
        componentMainCharacterAction.attackPositionOffset.x *= -1; 
        tmp_scale = mainCharacterGameObject.transform.localScale;
        tmp_scale.x = componentMainCharacterState.originalXScale * componentMainCharacterState.direction;

        //Apply the new scale
        mainCharacterGameObject.transform.localScale = tmp_scale;
    }
    #endregion

    #region handleJump
    /*
     * sets Jump flag to signal received Jump Instruction on Update
     */
    void ReceiveJumpPressInput()
    {
        receivedJumpFlag = true;
        holdJumpButton = true;
    }

    /*
     * Removes Jump is beeing hold Flag
     */
    void ReceiveJumpCancelInput()
    {
        holdJumpButton = false;
    }

    /*
     * Handels Jump Instructions
     */
    void HandelJumpInstruction()
    {
        if (isJumpPossible())
        {           
            if ((componentMainCharacterState.hasJump && (componentMainCharacterState.isOnGround || Time.time < componentMainCharacterState.coyoteTime))
                || componentMainCharacterState.hasJump && componentMainCharacterAction.isHangingOnMarker)
            {
                if (componentMainCharacterAction.isHangingOnMarker)
                {
                    componentMainCharacterAction.isHangingOnMarker = false;
                    rigidBody.bodyType = RigidbodyType2D.Dynamic;
                }

                Jump();
                skipNextFrame = true;
                componentMainCharacterState.isOnGround = false;
                componentMainCharacterState.hasJump = false;

                if (componentMainCharacterAction.isBat)
                {
                    componentMainCharacterAction.batFlapImpulse = true;
                    //TODO change to bat flap audio if found good sound
                    AudioManager.PlayJumpAudio();
                }
                else
                {
                    AudioManager.PlayJumpAudio();
                }
            }
            else
            {
                if (componentMainCharacterAction.isKraken || componentMainCharacterAction.isWolf || componentMainCharacterAction.isGhost)
                    return;

                if ((componentMainCharacterAction.hasBat && componentMainCharacterAction.hasDoubleJump) || componentMainCharacterAction.isBat)
                {
                    Jump();
                    if (componentMainCharacterAction.isBat)
                    {
                        componentMainCharacterAction.batFlapImpulse = true;
                        //TODO change to bat flap audio if found good sound
                        AudioManager.PlayJumpAudio();
                    }
                    else
                    {
                        componentMainCharacterAction.hasDoubleJump = false;
                        componentMainCharacterAction.batFlapDoubleJumpImpulse = true;
                        increaseCurseCounterBat();
                        //TODO change to BIG bat flap audio if found good sound
                        AudioManager.PlayJumpAudio();
                    }
                }
            }
        }
    }

    /*
     * Applies force to the character to actually jump
     */
    void Jump()
    {
        //remove y-velocity, so that one can not use edges to boost
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

        componentMainCharacterState.isOnGround = false;
        rigidBody.AddForce(new Vector2(0f, componentMainCharacterState.currentJumpForce), ForceMode2D.Impulse);
    }

    /*
     * Caps falling speed If player is falling to fast, reduce the Y velocity to the max
     */
    void CapFallingSpeed()
    {
        if (rigidBody.velocity.y < ComponentMainCharacterState.maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, ComponentMainCharacterState.maxFallSpeed);
    }

    /*
     * TODO
     * Checks all possible states of components to determine whether Jump is currently possible
     * e.g. no jump during attack, no jump during tentacle attack, etc.
     */
    bool isJumpPossible()
    {
        return true;
    }

    /*
     * glide if someone is falling, fether the falling
     */
    void HandleGliding()
    {
        if (!componentMainCharacterAction.hasBat) return;

        if (holdJumpButton &&!componentMainCharacterState.isGliding &&rigidBody.velocity.y < 0)
        {
            StartGlide();
        }
        else if(componentMainCharacterState.isGliding && (!holdJumpButton || componentMainCharacterState.isOnGround))
        {
            EndGlide();
        }
    }

    void StartGlide()
    {
        rigidBody.gravityScale = componentMainCharacterState.normalGravity * ComponentMainCharacterAction.gravityPercentageHALFBat;
        componentMainCharacterState.speedMultiplier = 0.6f;

        rigidBody.velocity = Vector2.zero;//new Vector2(rigidBody.velocity.x * 0.6f,rigidBody.velocity.y*0.3f);
        componentMainCharacterState.isGliding = true;
    }

    void EndGlide()
    {
        rigidBody.gravityScale = componentMainCharacterState.normalGravity;
        componentMainCharacterState.speedMultiplier = 1f;
        componentMainCharacterState.isGliding = false;
        holdJumpButton = false;
    }

    #endregion

    #region handleKraken

    private void HandleKrakenInstruction()
    {
        //If ability was not unlocked ==> do nothing
        if (componentMainCharacterAction.hasKraken == false)
        {
            return;
        }

        //If no marker is in range ==> do nothing (TODO do krakenAttack)
        if (componentKrakenMarker.closestMarkerInRange == null)
        {
            HandleKrakenAttack();
            return;
        }

        //If player is currently attackign ==> do nothing
        if (componentMainCharacterState.isAttacking)
        {
            return;
        }

        //If player is transformed in something other than kraken ==> do nothing
        if (componentMainCharacterAction.isBat || componentMainCharacterAction.isGhost || componentMainCharacterAction.isWolf)
        {
            return;
        }

        //If player is transformed in kraken ==> TODO make something usefull
        if (componentMainCharacterAction.isKraken)
        {
            return;
        }
        else
        {
            AudioManager.PlayKrakenSquishSFXAudio();
            componentMainCharacterAction.krakenImpulse = true;
            componentMainCharacterAction.isUsingKrakenPull = true;
            increaseCurseCounterKraken();
            Vector2.MoveTowards(mainCharacterTransform.position, componentKrakenMarker.closestMarkerInRange.transform.position,componentKrakenMarker.distanceThreshold);
        }    
    }

    private void MoveToKrakenMarker()
    {
        tmp_direction = (Vector2)(componentKrakenMarker.closestMarkerInRange.transform.position - mainCharacterTransform.position);

        //Checking player distance to the krakenMarker
        if (tmp_direction.sqrMagnitude < ComponentMainCharacterAction.krakePullThresholdDistance * ComponentMainCharacterAction.krakePullThresholdDistance)
        {
            if (!componentMainCharacterAction.isKraken)
            {
                rigidBody.velocity = Vector2.zero;
                rigidBody.bodyType = RigidbodyType2D.Static;
                componentMainCharacterAction.isUsingKrakenPull = false;
                componentMainCharacterAction.isHangingOnMarker = true;
                componentMainCharacterAction.hasDoubleJump = true;
                componentMainCharacterState.hasJump = true;
            }
            else
            {
                componentMainCharacterAction.isUsingKrakenPull = false;
                componentMainCharacterAction.isHangingOnMarker = false;
            }
        }
        else
        {
            rigidBody.MovePosition((Vector2)mainCharacterTransform.position + tmp_direction * ComponentMainCharacterAction.krakenPullSpeed * Time.deltaTime);
        }  
    }

    void ReceiveKrakenPressInput()
    {
        receivedKrakenFlag = true;
    }


    void HandleKrakenAttack()
    {
        if (componentMainCharacterAction.timeUntillNextAttack < Time.time)
        {
            Debug.Log("DidAttack");
            componentMainCharacterState.isAttacking = true;
            numberOfOverlaps = Physics2D.OverlapBoxNonAlloc(mainCharacterTransform.position + componentMainCharacterAction.attackPositionOffset, componentMainCharacterAction.currentAttackBox, 0, enemyToDamageColliders, systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Enemy")));

            ApplyDamageToAllEnemys(enemyToDamageColliders, componentMainCharacterState.damage);

            ResetTempArrays();

            componentMainCharacterAction.timeUntillNextAttack = Time.time + componentMainCharacterAction.waitingTime;

            //trigger Animation
            //TODO make right animation
            componentMainCharacterAction.attackImpulse = true;
            //AudioManager.PlaySwordAttackAudio();
        }
        if (componentMainCharacterAction.timeUntillNextAttack < Time.time)
        {
            componentMainCharacterState.isAttacking = false;
        }
    }
    #endregion

    /*
     * Resets Variables in the Component which only need to be set fro one Frame to trigger other events like Animations
     */
    private void ResetImpulses()
    {
        componentMainCharacterAction.batFlapImpulse = false;
        componentMainCharacterAction.batFlapDoubleJumpImpulse = false;
        componentMainCharacterAction.attackImpulse = false;
        componentMainCharacterAction.krakenImpulse = false;
    }

    #region handleAttack
    /*
     * perform the attack
     */
    void HandleAttackInstruction()
    {
        if (receivedAttackFlag && componentMainCharacterAction.timeUntillNextAttack < Time.time)
        {
            Debug.Log("DidAttack");
            componentMainCharacterState.isAttacking = true;
            numberOfOverlaps = Physics2D.OverlapBoxNonAlloc(mainCharacterTransform.position + componentMainCharacterAction.attackPositionOffset, componentMainCharacterAction.currentAttackBox, 0, enemyToDamageColliders, systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Enemy")));

            ApplyDamageToAllEnemys(enemyToDamageColliders, componentMainCharacterState.damage);

            ResetTempArrays();

            componentMainCharacterAction.timeUntillNextAttack = Time.time + componentMainCharacterAction.waitingTime;
            receivedAttackFlag = false;

            //trigger Animation
            componentMainCharacterAction.attackImpulse = true;
            AudioManager.PlaySwordAttackAudio();
        }

        if (componentMainCharacterAction.timeUntillNextAttack < Time.time)
        {
            componentMainCharacterState.isAttacking = false;
        }

    }

    /*
     * reset enemyToCollider and enemys array
     */
    void ResetTempArrays()
    {
        for (int i = 0; i < enemyToDamageColliders.Length; i++)
        {
            enemyToDamageColliders[i] = null;
        }

    }


    /*
     * apply the damage to all enemys hit
     */
    void ApplyDamageToAllEnemys(Collider2D[] enemys, int damage)
    {
        foreach (var enemy in enemys)
        {
            if (enemy == null) break;
            // arguments damage, and direction -1 if we hit from the left and 1 if we hit from th right
            enemy.GetComponentInParent<SystemEnemy>().ReceiveDamage(damage, mainCharacterGameObject.transform.position.x <= enemy.transform.position.x ? -1 : 1);
        }
    }

    /*
     * set attack flag, if attacak button is pressed
     */
    void ReceiveAttackPressInput()
    {
        receivedAttackFlag = true;
    }
    
    
    /*
     *  --------------------------TEST----------------------------
     *  used for attacking with a collider
     */
    public void AttackWithCollider(Collider2D collision)
    {
        int damage = componentMainCharacterState.damage;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

        collision.GetComponentInParent<SystemEnemy>().ReceiveDamage(damage, mainCharacterGameObject.transform.position.x <= collision.transform.position.x ? -1 : 1);
    }

    /*
     * -----------------------------------------------------------------------------
     */

    #endregion

    #region handleHit

    /*
     * handle the bullet hit
     */
     public void BulletHit(Collision2D collision, int damage)
    {
        Destroy(collision.gameObject);

        ReceiveDamage(damage,collision.gameObject.GetComponent<SystemBullet>().getAttackDirection().x > 0? 1 : -1);
    }


    /*
     * handles the case if the player receives damage
     */
     public void ReceiveDamage(int  damage, int direction)
    {
        //do not receive damage while knockbacked
        if (componentMainCharacterAction.timeUntillKnockBackEnd >= Time.time) return;

        //Debug.Log("time till knockback: "+componentMainCharacterAction.timeUntillKnockBackEnd);

        AudioManager.PlayHurtAudio();
        componentMainCharacterState.health -= damage;

        if (componentMainCharacterState.isGliding) EndGlide();

        Debug.Log("Was hit: " + componentMainCharacterState.health + " Time:" + Time.time); //TEST
        if (componentMainCharacterState.health <= 0)
        {
            AudioManager.PlayDeathAudio();
            AudioManager.PlayDeathSFXAudio();
            HandleDiePlayer();
        }

        //make player move while he can not receive damage
        Debug.Log("GOD");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        //direction is the direction where the collision is originated
        WasHitKnockBack(direction);
    }

     /*
     * knocks back the player
     */
    private void WasHitKnockBack(int knockBackdirection)
    {

        if (componentMainCharacterAction.timeUntillKnockBackEnd < Time.time)
        {
            componentMainCharacterAction.timeUntillKnockBackEnd = Time.time + ComponentMainCharacterAction.knockBackTime;
            rigidBody.velocity = Vector2.zero;
            rigidBody.velocity = new Vector2(knockBackdirection * ComponentMainCharacterAction.knowBackPowerHorizontal, ComponentMainCharacterAction.knockBackPowerUp);
        }
    }

    private void HandleDiePlayer()
    {
        //TODO Implement dieing
    }
    #endregion

    #region handleTransformation
    /*
     * for every use of a special ability, add energy to the transformation bar
     */
     void AddEnergy(int amount)
    {

    }

    /*
     * decreas the energy over time
     */
    void DecreaseEneryOverTime()
    {

    }


    #endregion

    /*
     * just for debug purposes, draws the hitting area of the player
     */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //The next line is causing an Error when not in Play mode
        Gizmos.DrawWireCube(mainCharacterTransform.position + componentMainCharacterAction.attackPositionOffset, new Vector3(componentMainCharacterAction.currentAttackBox.x, componentMainCharacterAction.currentAttackBox.y, 1f));
    }

    #region IncreaseCurseCounters
    private void increaseCurseCounterKraken()
    {
        componentMainCharacterAction.currentKrakenCounter += ComponentMainCharacterAction.costKrakenAbility;
        
        if (componentMainCharacterAction.currentKrakenCounter > 1)
        {
            componentMainCharacterAction.currentKrakenCounter = 1;
        }
    }

    private void increaseCurseCounterBat()
    {
        componentMainCharacterAction.currentBatCounter += ComponentMainCharacterAction.costBatAbility;

        if (componentMainCharacterAction.currentBatCounter > 1)
        {
            componentMainCharacterAction.currentBatCounter = 1;
        }
    }
    #endregion
}