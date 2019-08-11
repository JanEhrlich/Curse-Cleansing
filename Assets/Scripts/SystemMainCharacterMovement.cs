using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    //handles:
    public GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    SystemGameMaster systemGameMaster;
    ComponentInput componentInput;
    ComponentMainCharacterAction componentMainCharacterAction;
    ComponentMainCharacterState componentMainCharacterState;

    //Tmp Variables used for Calculations
    Vector2 movement;
    private float currentSpeed;
    private float currentJumpForce;
    private float currentGravity;

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;
        componentMainCharacterState.layerMask = TransformToLayerMask(mainCharacterGameObject.layer);

        //Record the original x scale of the player
        componentMainCharacterState.originalXScale = mainCharacterGameObject.transform.localScale.x;

        //add button functions
        componentInput.AddJumpButtonPressFunction(MidAirMovement);
    }

    private int TransformToLayerMask(int layer)
    {
        return ~(1<<layer);
    }

    public void Tick()
    {
        
    }
    public void FixedTick()
    {
        PhysicsCheck();
        GroundMovement();
    }

    private void PhysicsCheck()
    {
        currentSpeed = componentMainCharacterState.speed * componentMainCharacterState.speedMultiplier;
        currentJumpForce = componentMainCharacterState.jumpForce * componentMainCharacterState.jumpForceMultiplier;
        //Start by assuming the player isn't on the ground and the head isn't blocked
        componentMainCharacterState.isOnGround = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-componentMainCharacterState.footOffset, 0f), Vector2.down, componentMainCharacterState.groundDistance, componentMainCharacterState.layerMask);
        RaycastHit2D rightCheck = Raycast(new Vector2(componentMainCharacterState.footOffset, 0f), Vector2.down, componentMainCharacterState.groundDistance, componentMainCharacterState.layerMask);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
        {
            componentMainCharacterState.isOnGround = true;
            componentMainCharacterAction.hasDoubleJump = true;
        }
    }

    void GroundMovement()
    {
        //Calculate the desired velocity based on inputs
        float xVelocity = currentSpeed * componentInput.getCurrentHorizontalJoystickPosition();

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * componentMainCharacterState.direction < 0f)
            FlipCharacterDirection();

        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        //If the player is on the ground, extend the coyote time window
        if (componentMainCharacterState.isOnGround)
            componentMainCharacterState.coyoteTime = Time.time + componentMainCharacterState.coyoteDuration;
    }
   
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, int layerMask)
    {
        //Record the player's position
        Vector2 pos = mainCharacterGameObject.transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layerMask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }

    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        componentMainCharacterState.direction *= -1;

        //Record the current scale
        Vector3 scale = mainCharacterGameObject.transform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = componentMainCharacterState.originalXScale * componentMainCharacterState.direction;

        //Apply the new scale
        mainCharacterGameObject.transform.localScale = scale;
    }

    void MidAirMovement()
    {
        
        //If the jump key is pressed  AND
        //the player is on the ground or within the coyote time window...
        if (componentMainCharacterState.isOnGround || componentMainCharacterState.coyoteTime > Time.time )
        {
            //remove y-velocity, so that one can not use edges to boost
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            //...The player is no longer on the groud and is jumping...
            componentMainCharacterState.isOnGround = false;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, currentJumpForce), ForceMode2D.Impulse);

        }
        else if(componentMainCharacterAction.hasBat && componentMainCharacterAction.hasDoubleJump || componentMainCharacterAction.isBat)
        {
            //remove y-velocity, so that one can not use edges to boost
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            //now we are double jumping
            componentMainCharacterAction.hasDoubleJump = false;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, currentJumpForce), ForceMode2D.Impulse);
        }
        //If player is falling to fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < componentMainCharacterState.maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, componentMainCharacterState.maxFallSpeed);
    }
}
