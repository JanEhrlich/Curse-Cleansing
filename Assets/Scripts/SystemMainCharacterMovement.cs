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
    public GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    SystemGameMaster systemGameMaster;

    ComponentInput componentInput;
    ComponentMainCharacterState componentMainCharacterState;

    //Tmp Variables used for Calculations
    Vector2 movement;

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterState.MainCharacter = mainCharacterGameObject;
    }

    public void Tick()
    {
        
    }

    public void FixedTick()
    {
        //Use the two store floats to create a new Vector2 variable movement.
         movement= componentInput.getCurrentJoystickDirection();

        //Call the AddForce function of our Rigidbody2D ridgetBody2D supplying movement multiplied by speed to move our player.
        rigidBody.AddForce(movement * componentMainCharacterState.Speed);

        //mainCharacterGameObject.transform.Translate(movement*0.2f);
    }
}
