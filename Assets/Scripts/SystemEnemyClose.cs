using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemEnemyClose : SystemEnemy
{
    public bool drawDebugRaycasts = true;	//Should the environment checks be visualized


    //Tmp Variables used for Calculations
    Vector2 movement;
    Vector3 tmp_scale;
    float tmp_xVelocity;
    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;
    float tmp_direction;
    bool leftEdge = false;
    bool rightEdge = false;

    /*
     * The Close Combat Enemies. Pirates and holy Order distinction in the ComponentEnemyState
     * TODO:
     *  -Overwrite Attack
     *  -Overwrite some Movement
     *  -Interprete "AI" instructions and 
     *  -Attack Logic
     */
    void Start()
    {
        base.Start();
    }
    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatedSpeedAndJumpForce();

        GroundedCheck();

        SimpleMovement();
    }

    private void UpdatedSpeedAndJumpForce()
    {
        componentEnemyState.currentSpeed = ComponentEnemyState.speed * componentEnemyState.speedMultiplier;
        componentEnemyState.currentJumpForce = ComponentEnemyState.jumpForce * componentEnemyState.jumpForceMultiplier;
    }

    /*
     * just run from left to right and vice versa, do not interact with player
     */
    private void SimpleMovement()
    {
        if (leftEdge && !rightEdge || rightEdge && !leftEdge) 
        {
             FlipCharacterDirection();
        }
        //TODO fix the minus for componentEnemyState
        tmp_xVelocity = -componentEnemyState.currentSpeed * componentEnemyState.direction;
        rigidBody.velocity = new Vector2(tmp_xVelocity, rigidBody.velocity.y);
        

        //Tracking of some State
        componentEnemyState.currentVelocity = rigidBody.velocity;
        componentEnemyState.isMoving = rigidBody.velocity.x > 0.1f || rigidBody.velocity.y > 0.1f || rigidBody.velocity.x < -0.1f || rigidBody.velocity.y < -0.1f;
    }

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
     * Flips the dierection of the Gameobject and the State in the Component
     */
    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        componentEnemyState.direction *= -1;
        //componentEnemyAction.attackPositionOffset.x *= -1;
       // tmp_scale = transform.localScale;
       // tmp_scale.x = componentEnemyState.originalXScale * componentEnemyState.direction;

        //Apply the new scale
        //transform.localScale = tmp_scale;
    }
}
