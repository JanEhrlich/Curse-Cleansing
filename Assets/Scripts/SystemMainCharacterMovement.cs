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

    Vector2 movement;  //Tmp Variables used for Calculations

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterState.MainCharacter = mainCharacterGameObject;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;

        //add button functions
        componentInput.AddJumpButtonPressFunction(MidAirMovement);
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
        //Start by assuming the player isn't on the ground and the head isn't blocked
        componentMainCharacterState.isOnGround = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-componentMainCharacterState.footOffset, 0f), Vector2.down, componentMainCharacterState.groundDistance, componentMainCharacterState.layerMask);
        RaycastHit2D rightCheck = Raycast(new Vector2(componentMainCharacterState.footOffset, 0f), Vector2.down, componentMainCharacterState.groundDistance, componentMainCharacterState.layerMask);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
        {
            componentMainCharacterState.isOnGround = true;
            componentMainCharacterState.hasDoubleJump = true;
        }
    }

    void GroundMovement()
    {
        //Calculate the desired velocity based on inputs
        float xVelocity = componentMainCharacterState.speed * componentInput.getCurrentHorizontalJoystickPosition();

        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        //If the player is on the ground, extend the coyote time window
        if (componentMainCharacterState.isOnGround)
            componentMainCharacterState.coyoteTime = Time.time + componentMainCharacterState.coyoteDuration;
    }
   
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, int layermask = 0)
    {
        //Record the player's position
        Vector2 pos = mainCharacterGameObject.transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layermask);

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


    void MidAirMovement()
    {
        
        //If the jump key is pressed  AND
        //the player is on the ground or within the coyote time window...
        if (componentMainCharacterState.isOnGround || componentMainCharacterState.coyoteTime > Time.time)
        {
            Debug.Log("Single jump! "+componentMainCharacterState.isOnGround);
            //remove y-velocity, so that one can not use edges to boost
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            //...The player is no longer on the groud and is jumping...
            componentMainCharacterState.isOnGround = false;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, componentMainCharacterState.jumpForce), ForceMode2D.Impulse);

        }
        else if(componentMainCharacterAction.hasBat && componentMainCharacterState.hasDoubleJump)
        {
            Debug.Log("Double jump!" + componentMainCharacterState.isOnGround);
            //remove y-velocity, so that one can not use edges to boost
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

            //now we are double jumping
            componentMainCharacterState.hasDoubleJump = false;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, componentMainCharacterState.jumpForce), ForceMode2D.Impulse);
        }
        //If player is falling to fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < componentMainCharacterState.maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, componentMainCharacterState.maxFallSpeed);
    }
}
