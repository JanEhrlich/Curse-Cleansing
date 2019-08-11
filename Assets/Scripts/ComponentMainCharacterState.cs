using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Information about the Player
 */

public class ComponentMainCharacterState
{

    public float speed = 6f;                //Player speed
    public float coyoteDuration = .05f;     //How long the player can jump after falling
    public float maxFallSpeed = -25f;       //Max speed player can fall
    public float speedMultiplier = 1f;      //adjust speed
    public float normalGravity;
    public float jumpForceMultiplier = 1f;  //adjust jump height


    public float jumpForce = 12f;           //Initial force of jump

    public float footOffset = .4f;          //X Offset of feet raycast
    public float groundDistance = .2f;      //Distance player is considered to be on the ground

    public bool isOnGround;                 //Is the player on the ground?
    public float coyoteTime;                       //Variable to hold coyote duration
    public float playerHeight;					   //Height of the player

    public int direction = 1;						//Direction player is facing

    public int layerMask; //do not hit player with raycast

}