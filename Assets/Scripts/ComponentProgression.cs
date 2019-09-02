using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentProgression
{

    /*
     * A list of Progression Points in the game
     * It should turn to true from top to buttom
     */

    //Level 1 Prison
    public bool level1_PlayedCutsceneOpening = false;
    public bool level1_TalkedToPrisoner = false;
    public bool level1_PlayedCutsceneReceiveCurse = false;
    public bool level1_PickedUpKey = false;
    public bool level1_PlayedCutsceneOpenDoor = false;
    public bool level1_JumpedPreFirstEnemy = false;
    public bool level1_PreSpikes = false;
    public bool level1_LeapOfFaith = false;
    public bool level1_PickedUpSword = false;
    public bool level1_Finished = false;

    //Level 2 Escape From Town
    public bool level2_RunTillRoadBlocked = false;
    public bool level2_InteractedWithMysteryMan = false;
    public bool level2_PlayedCutsceneToTheHideout = false;
    public bool level2_Finished = false;

    //Level 3 In the Hideout
    public bool level3_TalkedToMysteryMan = false;
    public bool level3_Finished = false;

    //Level 4 On the way to the Tavern
    public bool level4_Finished = false;

    //Level 5 On the way to the Tavern
    public bool level5_ExploredTavern = false;
    public bool level5_TalkedToBarkeeper = false;
    public bool level5_FinishedFightVersusPirates = false;
    public bool level5_FinishedFightVersusOrder = false;
    public bool level5_PlayedCutsceneReceiveCurseFromBarkeeper = false;
    public bool level5_Finished = false;

    //Level 3_2 In the Hideout again
    public bool level3_2_TalkedToMysteryMan = false;
    public bool level3_2_Finished = false;

    //Level 6 In the Habor, Sneaking into th ship
    public bool level6_Finished = false;

    //Level 7 In the Habor, Sneaking into th ship
    public bool level7_FailStealingFromCaptain = false;
    public bool level7_FinishedFightVersusPirateCrew = false;
    public bool level7_PlayedCutsceneAppereanceOfTheCaptain = false;
    public bool level7_FinishedBossFight = false;
    public bool level7_PlayedCutsceneReceivingGhostCurse = false;
    public bool level7_Finished = false;
}
