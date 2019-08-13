using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMainCharacterAction
{
    /*
    public bool hasKraken = false;
    public bool hasBat = false;
    */

    public bool hasKraken = true;                   //TODO set to false, just for testing
    public bool hasBat = true;                      //TODO set to false, just for testing

    public bool hasGhost = false;
    public bool hasWolf = false;

    public bool isKraken = false;
    public bool isBat = false;
    public bool isGhost = false;
    public bool isWolf = false;

    public float timeUnTillNormal = 0;
    public float durationTransformationKrake = 3f;
    public float durationTransformationBat = 5f;
    public float durationTransformationWolf;
    public float durationTransformationGhost;

    public Vector2 colliderStandSize;               //Size of the standing collider
    public Vector2 colliderStandOffset;             //Offset of the standing collider
    public Vector2 colliderKrakenSize;              //Size of the Kraken collider
    public Vector2 colliderKrakenOffset;            //Offset of the Kraken collider
    public Vector2 colliderBatSize;                 //Size of the Bat collider
    public Vector2 colliderBatOffset;               //Offset of the Bat collider

    public float krakenJumpPercentage = 0.6f;       //Jump force reduction while being a kraken
    public float batJumpPercentage = 0.3f;          //Jump force reduction while being a bat
    public float gravityPercentageBat = 0.7f;       //falling slower while being a bat
    public float gravityPercentageHALFBat = 0.3f;   //falling slower while being a HALF-bat

    public float krakenSizePercentage = 0.3f;       //ration of kraken to player
    public float batSizePercentage = 0.3f;          //ration of bat to player

    public float krakenSpeedPercentage = 0.4f;	    //Speed reduction while being a kraken
    public float batSpeedPercentage = 0.7f;         //Speed reduction while being a bat

    public bool isGliding = false;                  //Is the player gliding?
    public bool isHeadBlocked;                      //can the player transform back?
    public float headClearance = 1.5f;		        //Space needed above the player's head

    public bool isFootBlocked;                      //can the player transform back?
    public float foodClearance = 1.5f;		        //Space needed above the player's head

    public bool hasDoubleJump = true;               //Variable to hold jump duration
}
