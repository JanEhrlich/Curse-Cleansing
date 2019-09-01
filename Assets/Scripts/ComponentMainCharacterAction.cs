using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMainCharacterAction
{
    /*
     * Hardcoded Variables which shape the games behaviour
     */
    #region StaticVariables
    public static float durationTransformationKrake = 3f;
    public static float durationTransformationBat = 5f;
    public static float durationTransformationWolf;
    public static float durationTransformationGhost;

    public static float krakenJumpPercentage = 0.6f;       //Jump force reduction while being a kraken
    public static float batJumpPercentage = 0.3f;          //Jump force reduction while being a bat
    public static float gravityPercentageBat = 0.3f;       //falling slower while being a bat
    public static float gravityPercentageHALFBat = 0.3f;   //falling slower while being a HALF-bat

    public static float krakenSizePercentage = 0.3f;       //ration of kraken to player
    public static float batSizePercentage = 0.3f;          //ration of bat to player

    public static float krakenSpeedPercentage = 0.4f;	   //Speed reduction while being a kraken
    public static float batSpeedPercentage = 0.7f;         //Speed reduction while being a bat

    public static float headClearance = 1.5f;              //Space needed above the player's head
    public static float foodClearance = 1.5f;              //Space needed under the player's foot

    public static float durationAttackNormal = 0.2f;
    public static float durationAttackKraken = 0.2f;
    public static float durationAttackBat = 0.2f;

    public static float knockBackTime = 1.5f;                 //time untill one can move again
    public static float knockBackPowerUp = 3f;              // velocity upwards on impact 
    public static float knowBackPowerHorizontal = 4f;       //horizontal velocity
    #endregion


    /*
     * Variables which will be set on Startup and never changed later
     */
    #region SetOnInitVariables
    public float footOffset = .4f;                          //X Offset of feet raycast
    public float normalGravity;                             //normal gravity of the World

    public Vector2 colliderStandSize;                       //Size of the standing collider
    public Vector2 colliderStandOffset;                     //Offset of the standing collider
    public Vector2 colliderKrakenSize;                      //Size of the Kraken collider
    public Vector2 colliderKrakenOffset;                    //Offset of the Kraken collider
    public Vector2 colliderBatSize;                         //Size of the Bat collider
    public Vector2 colliderBatOffset;                       //Offset of the Bat collider

    public Vector2 attackBoxNormal;                         //attackBox ist the area of applying dmg
    public Vector2 attackBoxKraken;                         //attackBox ist the area of applying dmg
    public Vector2 attackBoxBat;                            //attackBox ist the area of applying dmg


    public Vector3 attackPositionHorizontalOffset;                  //offset where to start the attack horizontal
    public Vector3 attackPositionVerticalOffset;                    //offset where to start the attack above or below
    #endregion


    /*
     * Variables which will be set during the game progression. They will only changed once during the game.
     */
    #region SetDuringGameProgressionVariables
    public bool hasKraken = true;                           //TODO set to false, just for testing
    public bool hasBat = true;                              //TODO set to false, just for testing
    public bool hasGhost = false;
    public bool hasWolf = false;
    #endregion


    /*
     * Dynamic Variables which will change very often depending on the game state and player's actions
     */
    #region DynamictVariables
    public bool isKraken = false;
    public bool isBat = false;
    public bool isGhost = false;
    public bool isWolf = false;

    public bool isGliding = false;                          //Is the player gliding?
    public bool isHeadBlocked;                              //can the player transform back?
    public bool isFootBlocked;                              //can the player transform back?
    public bool hasDoubleJump = true;                       //Is the player's double jump available

    public Vector3 attackPositionOffset;                    //offset where to start the attack horizontal


    public bool batFlapImpulse = false;                     //Variable will be set to true for one Frame, to trigger bat Flap Animation
    public bool batFlapDoubleJumpImpulse = false;           //Variable will be set to true for one Frame, to trigger bat Flap Animation in Double Jump

    public float timeUntillNormal = 0f;                     //Counter how long it takes until main character transforms back 

    public float waitingTime = 0.3f;                        //how long do i need to wait before attacking again
    public float timeUntillNextAttack = 0f;                 //counter how long to wait 
    public bool attackImpulse = false;                      //Variable will be set to true for one Frame, to trigger Attack Animation

    public float timeUntillKnockBackEnd = 0f;               //current time left untill enemy can move again
    #endregion
}
