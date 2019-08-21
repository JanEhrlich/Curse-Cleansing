using System;
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
    Transform mainCharacterTransform;

    //Variable used to process jumping and gliding
    bool receivedJumpFlag;
    bool holdJumpButton;

    //Variable used for attacking
    bool receivedAttackFlag;

    //Used to avoid setting jupedOnce back to false because Jump couldn't build up distance to make GroundedCheck false
    bool skipNextFrame;

    //Tmp Variables used for Calculations
    Vector2 movement;
    Vector3 tmp_scale;
    float tmp_xVelocity;
    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;
    float tmp_direction;

    private float attackLength = 2f;

    Collider2D[] enemyToDamageColliders;
    GameObject[] enemys;

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        mainCharacterTransform = mainCharacterGameObject.transform;

        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;

        InitPlayerAttackRanges();

        //Sets Layermask of mainCharacter
        componentMainCharacterState.layerMask = gameMaster.SystemUtility.TransformToLayerMask(mainCharacterGameObject.layer,true);

        //Record the original x scale of the player
        componentMainCharacterState.originalXScale = mainCharacterGameObject.transform.localScale.x;

        //add button functions
        componentInput.AddJumpButtonPressFunction(ReceiveJumpPressInput);
        componentInput.AddJumpButtonCancelFunction(ReceiveJumpCancelInput);
        componentInput.AddAttackButtonPressFunction(ReceiveAttackPressInput);
    }

    void InitPlayerAttackRanges()
    {
        componentMainCharacterAction.attackBoxNormal = new Vector2(attackLength, attackLength);
        componentMainCharacterAction.attackBoxKraken = new Vector2(attackLength, attackLength * 2.5f);
        componentMainCharacterAction.attackBoxBat = new Vector2(attackLength, attackLength * 2f);

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

        if (skipNextFrame)
        {
            skipNextFrame = false;            
        }
        else
        {
            GroundedCheck();
        }


        HorizontalMovement();
        VerticalLooking();

        if (receivedJumpFlag)
        {
            HandelJumpInstruction();
            receivedJumpFlag = false;
        }

        if (receivedAttackFlag)
        {
            HandleAttackInstruction();
            
        }

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
        if (componentInput.getCurrentVerticalJoystickPosition() == 0)
        {
            componentMainCharacterAction.attackPositionOffset = componentMainCharacterAction.attackPositionHorizontalOffset;
            componentMainCharacterAction.attackPositionOffset.x *= componentMainCharacterState.direction;
            return;
        }

        componentMainCharacterAction.attackPositionOffset = componentMainCharacterAction.attackPositionVerticalOffset;
        if (componentInput.getCurrentVerticalJoystickPosition() < 0)
        {
            componentMainCharacterAction.attackPositionOffset.y *= -1;
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
            if (componentMainCharacterState.hasJump && (componentMainCharacterState.isOnGround || Time.time < componentMainCharacterState.coyoteTime))
            {
                Jump();
                skipNextFrame = true;
                componentMainCharacterState.isOnGround = false;
                componentMainCharacterState.hasJump = false;

                if (componentMainCharacterAction.isBat)
                {
                    componentMainCharacterAction.batFlapImpulse = true;
                }               
            }
            else
            {
                if ((componentMainCharacterAction.hasBat && componentMainCharacterAction.hasDoubleJump) || componentMainCharacterAction.isBat)
                {
                    Jump();
                    if (componentMainCharacterAction.isBat)
                    {
                        componentMainCharacterAction.batFlapImpulse = true;
                    }
                    else
                    {
                        componentMainCharacterAction.hasDoubleJump = false;
                        componentMainCharacterAction.batFlapDoubleJumpImpulse = true;
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
     * Resets Variables in the Component which only need to be set fro one Frame to trigger other events like Animations
     */
    private void ResetImpulses()
    {
        componentMainCharacterAction.batFlapImpulse = false;
        componentMainCharacterAction.batFlapDoubleJumpImpulse = false;
    }
    #endregion

    #region handleAttack
    /*
     * perform the attack
     */
    void HandleAttackInstruction()
    {
        //do not attack to often
        if (componentMainCharacterAction.timeUntillNextAttack <= 0)
        {
        Debug.Log("DidAttack");
            //compute overlapping colliders
            enemyToDamageColliders = Physics2D.OverlapBoxAll(mainCharacterTransform.position + componentMainCharacterAction.attackPositionOffset, componentMainCharacterAction.attackBoxNormal, 0, systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Enemy")));

            //transform them into GameObjects
            enemys = enemyToDamageColliders.Select(enemy => enemy.gameObject).ToArray<GameObject>();

            ApplyDamageToAllEnemys(enemys, componentMainCharacterState.damage);

            componentMainCharacterAction.timeUntillNextAttack = componentMainCharacterAction.waitingTime;
            receivedAttackFlag = false;
        }
        else
        {
            componentMainCharacterAction.timeUntillNextAttack -= Time.deltaTime;
        }
    }

    /*
     * apply the damage to all enemys hit
     */
    void ApplyDamageToAllEnemys(GameObject[] enemys, int damage)
    {
        foreach (var enemy in enemys)
        {
            enemy.GetComponentInParent<SystemEnemy>().ReceiveDamage(damage);
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
     * just for debug purposes, draws the hitting area of the player
     */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mainCharacterTransform.position + componentMainCharacterAction.attackPositionOffset, new Vector3(componentMainCharacterAction.attackBoxNormal.x, componentMainCharacterAction.attackBoxNormal.y, 1f));
    }
    #endregion


}
