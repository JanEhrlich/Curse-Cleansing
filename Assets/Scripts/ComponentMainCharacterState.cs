using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Information about the Player
 */

public class ComponentMainCharacterState
{
    public GameObject MainCharacter { get; set; }

    public float speed = 8f;                //Player speed
    public float crouchSpeedDivisor = 3f;   //Speed reduction when crouching
    public float coyoteDuration = .05f;     //How long the player can jump after falling
    public float maxFallSpeed = -25f;		//Max speed player can fall


    public float jumpForce = 6.3f;			//Initial force of jump

    public float footOffset = .4f;          //X Offset of feet raycast
    public float groundDistance = .2f;      //Distance player is considered to be on the ground

    public bool isOnGround;                 //Is the player on the ground?

    public float jumpTime;                         //Variable to hold jump duration
    public float coyoteTime;                       //Variable to hold coyote duration
    public float playerHeight;						//Height of the player

}
