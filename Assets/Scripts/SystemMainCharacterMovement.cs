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
    //TODO Initialize Ridgidbody
    // initialize ComponentMainCharacter

    void Tick(SystemGameMaster systemGM)
    {
        
    }

    void FixedTick(SystemGameMaster systemGM)
    {
        ComponentMainCharacterState componentMainCharacterState = systemGM.MainCharacterState;
        Rigidbody2D ridgetBody2D = componentMainCharacterState.mainCharacter.GetComponent<Rigidbody2D>();
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D ridgetBody2D supplying movement multiplied by speed to move our player.
        ridgetBody2D.AddForce(movement * componentMainCharacterState.speed);
    }
}
