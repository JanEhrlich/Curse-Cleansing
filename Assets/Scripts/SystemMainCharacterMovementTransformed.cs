﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * this class handles the transformation of the main character
 * 
 * handled by Game-Master-System
 * 
 * TODO:
 *  -only allow one transformation at a time
 *  -handles the maximal time for transformation
 * 
 */

public class SystemMainCharacterMovementTransformed : MonoBehaviour
{

    public bool drawDebugRaycasts = true;	//Should the environment checks be visualized

    //handles:
    public GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    BoxCollider2D collider2d;
    SystemGameMaster systemGameMaster;
    ComponentInput componentInput;
    ComponentMainCharacterAction componentMainCharacterAction;
    ComponentMainCharacterState componentMainCharacterState;
    Animator anim;

    Vector2 movement;  //Tmp Variables used for Calculations

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        collider2d = mainCharacterGameObject.GetComponentInChildren<BoxCollider2D>();
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;
        anim = mainCharacterGameObject.GetComponent<Animator>();
        
        InitPlayerStuff();
        
        //add button functions
        componentInput.AddQuickTransformKrakenButtonPressFunction(transformToKraken);
        componentInput.AddQuickTransformBatButtonPressFunction(transformToBat);
    }

    public void InitPlayerStuff()
    {
        componentMainCharacterState.playerHeight = collider2d.size.y;
        componentMainCharacterState.normalGravity = rigidBody.gravityScale;

        //stand size
        componentMainCharacterAction.colliderStandSize = collider2d.size;
        componentMainCharacterAction.colliderStandOffset = collider2d.offset;
        //kraken size
        componentMainCharacterAction.colliderKrakenSize = new Vector2(componentMainCharacterAction.colliderStandSize.x, componentMainCharacterAction.colliderStandSize.y * componentMainCharacterAction.krakenSizePercentage);
        componentMainCharacterAction.colliderKrakenOffset = new Vector2(componentMainCharacterAction.colliderStandOffset.x, componentMainCharacterAction.colliderStandOffset.y * componentMainCharacterAction.krakenSizePercentage);
        //bat size
        componentMainCharacterAction.colliderBatSize = new Vector2(componentMainCharacterAction.colliderStandSize.x, componentMainCharacterAction.colliderStandSize.y * componentMainCharacterAction.batSizePercentage);
        componentMainCharacterAction.colliderBatOffset = new Vector2(componentMainCharacterAction.colliderStandOffset.x, componentMainCharacterAction.colliderStandOffset.y * componentMainCharacterAction.batSizePercentage);
    }

    private void transformToKraken()
    {
        //do not transform if we do not have the kraken or if we already are a creature
        if (!componentMainCharacterAction.hasKraken || IsAlreadyTransformed()) return;
        componentMainCharacterAction.timeUnTillNormal = Time.time + componentMainCharacterAction.durationTransformationKrake;

        //set kraken size and status
        componentMainCharacterAction.isKraken = true;
        componentMainCharacterState.speedMultiplier = componentMainCharacterAction.krakenSpeedPercentage;
        componentMainCharacterState.jumpForceMultiplier = componentMainCharacterAction.krakenJumpPercentage;
        collider2d.size = componentMainCharacterAction.colliderKrakenSize;
        collider2d.offset = componentMainCharacterAction.colliderKrakenOffset;
        anim.SetBool("isCrouching",true);
    }

    private void transformToBat()
    {
        //do not transform if we do not have the bat or if we already are an creature
        if (!componentMainCharacterAction.hasBat || IsAlreadyTransformed()) return;

        componentMainCharacterAction.timeUnTillNormal = Time.time + componentMainCharacterAction.durationTransformationBat;

        //set kraken size and status
        componentMainCharacterAction.isBat = true;
        componentMainCharacterState.speedMultiplier = componentMainCharacterAction.batSpeedPercentage;
        componentMainCharacterState.jumpForceMultiplier = componentMainCharacterAction.batJumpPercentage;
        rigidBody.gravityScale = componentMainCharacterState.normalGravity * componentMainCharacterAction.gravityPercentageHALFBat;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x,rigidBody.velocity.y * componentMainCharacterAction.gravityPercentageBat);
        collider2d.size = componentMainCharacterAction.colliderBatSize;
        collider2d.offset = componentMainCharacterAction.colliderBatOffset;
        anim.SetBool("isCrouching", true);
    }

    private void transformToNormalCaracter()
    {
        //reset transformation status
        componentMainCharacterAction.isKraken =  false;
        componentMainCharacterAction.isGhost = false;
        componentMainCharacterAction.isBat = false;
        componentMainCharacterAction.isWolf = false;

        //set normal size and status
        componentMainCharacterAction.timeUnTillNormal = 0;
        componentMainCharacterState.speedMultiplier = 1f;
        componentMainCharacterState.jumpForceMultiplier = 1f;
        rigidBody.gravityScale = componentMainCharacterState.normalGravity;
        collider2d.size = componentMainCharacterAction.colliderStandSize;
        collider2d.offset = componentMainCharacterAction.colliderStandOffset;
        anim.SetBool("isCrouching", false);

    }
    private bool IsAlreadyTransformed()
    {
        return componentMainCharacterAction.isKraken || componentMainCharacterAction.isWolf || componentMainCharacterAction.isGhost || componentMainCharacterAction.isBat;
    }
    public void Tick()
    {
        
    }

    public void FixedTick()
    {
        PhysicsCheck();

        if (componentMainCharacterAction.timeUnTillNormal < Time.time && !componentMainCharacterAction.isHeadBlocked)
            transformToNormalCaracter();
        //check that the bat is not so high, that it touches the ceiling
        //TODO can be buggy, dont know if this should work
        if (componentMainCharacterAction.isBat && componentMainCharacterAction.timeUnTillNormal < Time.time && !componentMainCharacterAction.isFootBlocked)
            transformToNormalCaracter();
    }

    void PhysicsCheck()
    {
       componentMainCharacterAction.isHeadBlocked = false;
       componentMainCharacterAction.isFootBlocked = false;

        //Cast the ray to check above the player's head
        RaycastHit2D headCheck = Raycast(new Vector2(0f, collider2d.size.y/2), Vector2.up, componentMainCharacterAction.headClearance, componentMainCharacterState.layerMask);
        //Cast the ray to check below the player's feet
        RaycastHit2D footCheck = Raycast(Vector2.zero, Vector2.down, componentMainCharacterAction.headClearance, componentMainCharacterState.layerMask);

        //If that ray hits, the player's head is blocked
        if (headCheck)
            componentMainCharacterAction.isHeadBlocked = true;
        if (footCheck)
            componentMainCharacterAction.isFootBlocked = true;
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
}
