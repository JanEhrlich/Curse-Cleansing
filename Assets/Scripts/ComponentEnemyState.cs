using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentEnemyState : MonoBehaviour
{
    /*
     * Hardcoded Variables which shape the games behaviour
     */
    #region StaticVariables
    public static float speed = 3f;                 //Enemy speed

    public static float jumpForce = 12f;            //Initial force of jump
    public static float groundDistance = .2f;       //Distance from player to ground which is considered as grounded

    public static float footOffsetLeft = .47f;           //X Offset of left foot raycast
    public static float footOffsetRight = .3f;           //X Offset of right foot raycast


    public int health = 100;
    public int damage = 10;
    #endregion


    /*
     * Variables which will be set on Startup and never changed later
     */
    #region SetOnInitVariables
    public float normalGravity;                     //normal gravity of the World
    public float enemyHeight;                      //Height of the enemy
    public int layerMask;                           //do not hit enemy with raycast for ground checks etc.
    public int hitLayerMask;                        //hit everything except the enemy and the camera for attacking
    #endregion


    /*
     * Dynamic Variables which will change very often depending on the game state and player's actions
     */
    #region DynamictVariables
    public float speedMultiplier = 1f;              //adjusts speed based on form and state (normal,kraken,bat,...)
    public float jumpForceMultiplier = 1f;          //adjusts jump height based on form and state (normal,kraken,bat,...)

    public float currentSpeed;                      //current speed of the enemy
    public float currentJumpForce;                  //current jumpForce of the enemy

    public bool isMoving;                           //Is the enemy currently moving horizontally or vertically
    public Vector2 currentVelocity;                   //current velocity of the enemy's rigidbody

    public bool hasJump;                            //Can the enemy intentionally jump once
    public bool isOnGround;                         //Is the enemy on the ground?

    public float originalXScale;                    //Original scale on X axis
    public int direction = 1;                       //Direction enemy is facing


    #endregion
}
