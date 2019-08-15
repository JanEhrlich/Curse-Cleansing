using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Information about the Player
 */
public class ComponentMainCharacterState
{

    /*
     * Hardcoded Variables which shape the games behaviour
     */
    #region StaticVariables
    public static float speed = 6f;                 //Player speed
    public static float coyoteDuration = .05f;      //How long the player can jump after falling
    public static float maxFallSpeed = -25f;        //Max speed player can fall

    public static float jumpForce = 12f;            //Initial force of jump
    public static float groundDistance = .2f;       //Distance from player to ground which is considered as grounded

    public static float footOffsetLeft = .47f;           //X Offset of left foot raycast
    public static float footOffsetRight = .3f;           //X Offset of right foot raycast
    #endregion


    /*
     * Variables which will be set on Startup and never changed later
     */
    #region SetOnInitVariables
    public float normalGravity;                     //normal gravity of the World
    public float playerHeight;                      //Height of the player
    #endregion


    /*
     * Dynamic Variables which will change very often depending on the game state and player's actions
     */
    #region DynamictVariables
    public float speedMultiplier = 1f;              //adjusts speed based on form and state (normal,kraken,bat,...)
    public float jumpForceMultiplier = 1f;          //adjusts jump height based on form and state (normal,kraken,bat,...)

    public float currentSpeed;                      //current speed of the Player
    public float currentJumpForce;                  //current jumpForce of the Player

    public bool isMoving;                           //Is the player currently moving horizontally or vertically
    public Vector2 currentVelocity;                   //current velocity of the Player's rigidbody

    public bool hasJump;                            //Can the player intentionally jump once. Used to prevent edge case "doubleJump" during coyoteTime
    public bool isOnGround;                         //Is the player on the ground?
    public float coyoteTime;                        //Variable to hold coyote duration

    public float originalXScale;                    //Original scale on X axis
    public int direction = 1;                       //Direction player is facing

    public int layerMask;                           //do not hit player with raycast
    #endregion
}