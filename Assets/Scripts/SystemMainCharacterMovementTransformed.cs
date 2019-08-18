
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

    //handles
    GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    BoxCollider2D collider2d;
    SystemGameMaster systemGameMaster;
    ComponentInput componentInput;
    ComponentMainCharacterAction componentMainCharacterAction;
    ComponentMainCharacterState componentMainCharacterState;
    //Animator anim;

    //Tmp Variables used for Calculations
    Vector2 movement;
    RaycastHit2D headCheck;
    RaycastHit2D footCheck;

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        collider2d = mainCharacterGameObject.GetComponentInChildren<BoxCollider2D>();
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;
        //anim = mainCharacterGameObject.GetComponent<Animator>();
        
        InitPlayerStuff();
        
        //add button functions
        componentInput.AddQuickTransformKrakenButtonPressFunction(TransformToKraken);
        componentInput.AddQuickTransformBatButtonPressFunction(TransformToBat);
    }

    public void Tick()
    {

    }

    public void FixedTick()
    {
        SpaceCheck();

        if (componentMainCharacterAction.timeUnTillNormal < Time.time && !componentMainCharacterAction.isHeadBlocked)
            TransformToNormalCaracter();

        //check that the bat is not so high, that it touches the ceiling
        //TODO can be buggy, dont know if this should work
        if (componentMainCharacterAction.isBat && componentMainCharacterAction.timeUnTillNormal < Time.time && !componentMainCharacterAction.isFootBlocked)
            TransformToNormalCaracter();
    }

    /*
     * Sets Size Variables of the MainCharacter Objects in the componentMainCharacterAction and componentMainCharacterState
     */
    public void InitPlayerStuff()
    {
        componentMainCharacterState.playerHeight = collider2d.size.y;
        componentMainCharacterState.normalGravity = rigidBody.gravityScale;

        //stand size
        componentMainCharacterAction.colliderStandSize = collider2d.size;
        componentMainCharacterAction.colliderStandOffset = collider2d.offset;
        //kraken size
        componentMainCharacterAction.colliderKrakenSize = new Vector2(componentMainCharacterAction.colliderStandSize.x, componentMainCharacterAction.colliderStandSize.y * ComponentMainCharacterAction.krakenSizePercentage);
        componentMainCharacterAction.colliderKrakenOffset = new Vector2(componentMainCharacterAction.colliderStandOffset.x, componentMainCharacterAction.colliderStandOffset.y * ComponentMainCharacterAction.krakenSizePercentage);
        //bat size
        componentMainCharacterAction.colliderBatSize = new Vector2(componentMainCharacterAction.colliderStandSize.x, componentMainCharacterAction.colliderStandSize.y * ComponentMainCharacterAction.batSizePercentage);
        componentMainCharacterAction.colliderBatOffset = new Vector2(componentMainCharacterAction.colliderStandOffset.x, componentMainCharacterAction.colliderStandOffset.y * ComponentMainCharacterAction.batSizePercentage);
    }

    /*
     * Setting all Variables and changes when fully transform into a curse form
     */
    #region TransformationsIntoAllForms

    private void TransformToKraken()
    {
        //do not transform if we do not have the kraken or if we already are a creature
        if (!componentMainCharacterAction.hasKraken || IsAlreadyTransformed()) return;

        //Handels all Variables
        SetTransformationVariables(ComponentMainCharacterAction.durationTransformationKrake,
            ComponentMainCharacterAction.krakenSpeedPercentage, ComponentMainCharacterAction.krakenJumpPercentage, 
            componentMainCharacterAction.colliderKrakenSize, componentMainCharacterAction.colliderKrakenOffset,true);

        componentMainCharacterAction.isKraken = true;
    }

    private void TransformToBat()
    {
        //do not transform if we do not have the bat or if we already are an creature
        if (!componentMainCharacterAction.hasBat || IsAlreadyTransformed()) return;

        //Handels all Variables
        SetTransformationVariables(ComponentMainCharacterAction.durationTransformationBat,
            ComponentMainCharacterAction.batSpeedPercentage, ComponentMainCharacterAction.batJumpPercentage,
            componentMainCharacterAction.colliderBatSize, componentMainCharacterAction.colliderBatOffset,true);

        componentMainCharacterAction.isBat = true;

        //@Jannis die nächste Zeile verstehe ich nicht. Warum halfBat Values benutzen wenn es hier um die volle Transformation geht?
        rigidBody.gravityScale = componentMainCharacterState.normalGravity * ComponentMainCharacterAction.gravityPercentageHALFBat;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * ComponentMainCharacterAction.gravityPercentageBat);
    }

    private void TransformToNormalCaracter()
    {

        //Handels all Variables
        SetTransformationVariables(0, 1, 1, componentMainCharacterAction.colliderStandSize, componentMainCharacterAction.colliderStandOffset,false);

        //reset transformation status
        componentMainCharacterAction.isKraken = false;
        componentMainCharacterAction.isGhost = false;
        componentMainCharacterAction.isBat = false;
        componentMainCharacterAction.isWolf = false;

        //set normal gravity
        rigidBody.gravityScale = componentMainCharacterState.normalGravity;
    }

    #endregion

    /*
     * Sets the size, speed, duration and other information depending on the Form
     */
    private void SetTransformationVariables(float duration, float speedMultiplier, float jumpForceMultiplier, Vector2 size, Vector2 offset, bool isCrouching)
    {
        componentMainCharacterAction.timeUnTillNormal = Time.time + duration;

        //set form size and statuses
        componentMainCharacterState.speedMultiplier = speedMultiplier;
        componentMainCharacterState.jumpForceMultiplier = jumpForceMultiplier;
        collider2d.size = size;
        collider2d.offset = offset;

        //update playerHeight in ComponentMainCharacterStatus. Used for GroundCheck offset calculations
        componentMainCharacterState.playerHeight = collider2d.size.y;
    }

    private bool IsAlreadyTransformed()
    {
        return componentMainCharacterAction.isKraken || componentMainCharacterAction.isWolf || componentMainCharacterAction.isGhost || componentMainCharacterAction.isBat;
    }

    /*
     * Check if there is enough room to stand up/transform back
     */
    #region FootAndHeadSpaceChecks

    void SpaceCheck()
    {
        HeadSpaceCheck();
        FootSpaceCheck();
    }

    /*
     * Cast a ray to check above the player's head if there is enough room to stand up/transform back
     */
    void HeadSpaceCheck()
    {
        headCheck = systemGameMaster.SystemUtility.Raycast(mainCharacterGameObject.transform.position,
            new Vector2(0f, collider2d.size.y / 2), Vector2.up, ComponentMainCharacterAction.headClearance, componentMainCharacterState.layerMask,drawDebugRaycasts);
        componentMainCharacterAction.isHeadBlocked = headCheck;
    }

    /*
     * Cast a ray to check below the player's feet if there is enough room to stand up/transform back
     */
    void FootSpaceCheck()
    {
        footCheck = systemGameMaster.SystemUtility.Raycast(mainCharacterGameObject.transform.position + Vector3.down * 0.49F * componentMainCharacterState.playerHeight,
            Vector2.zero, Vector2.down, ComponentMainCharacterAction.headClearance, componentMainCharacterState.layerMask,drawDebugRaycasts);
        componentMainCharacterAction.isFootBlocked = footCheck;
    }

    #endregion
}
