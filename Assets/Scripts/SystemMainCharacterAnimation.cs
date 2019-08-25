using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class handles the animations of the main character
 * 
 * handled by Game-Master-System
 * 
 * TODO:
 *  -make animations on events (attack)
 */

public class SystemMainCharacterAnimation : MonoBehaviour
{
    GameObject mainCharacterGameObject;
    SystemGameMaster systemGameMaster;
    ComponentMainCharacterAction actions;
    ComponentMainCharacterState states;

    //Animator used for all Animations of Main Character
    Animator anim;

    #region AnimatorParameterStrings
    string isIdeling = "isIdeling";
    string isRunning = "isRunning";
    string isAirborn = "isAirborn";
    string isAttacking = "isAttacking";
    string isUsingKrakenSkill = "isUsingKrakenSkill";
    string isUsingGhostSkill = "isUsingGhostSkill";
    string isUsingBatDoubleJump = "isUsingBatDoubleJump";
    string isUsingBatSkill = "isUsingBatSkill";
    string isBat = "isBat";
    string isBatFlapping = "isBatFlapping";
    string isKraken = "isKraken";
    string isWolf = "isWolf";
    string isWolfRunning = "isWolfRunning";
    string isWolfDamageTaken = "isWolfDamageTaken";
    string isTransformed = "isTransformed";
    #endregion

    //Set this Vaiable to the length of the attack Animation Clip
    float attackAnimationDuration = 0.3333333333333333F;

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();

        states = systemGameMaster.ComponentMainCharacterState;
        actions = systemGameMaster.ComponentMainCharacterAction;

        anim = mainCharacterGameObject.GetComponent<Animator>();
    }

    public void Tick()
    {
        /*
         * Further TODOS:
         * -Check Falling and Rising state in Airborn state depending on physical movement direction (up or down)
         */
        #region Finished
        anim.SetBool(isIdeling, checkIdel());
        anim.SetBool(isRunning, checkRunning());
        anim.SetBool(isAirborn, checkAirborn());
        anim.SetBool(isUsingBatDoubleJump, checkUsingBatDoubleJump());
        anim.SetBool(isBat, checkBat());
        anim.SetBool(isBatFlapping, checkBatFlapping());
        anim.SetBool(isKraken, checkKraken());
        anim.SetBool(isTransformed, checkTransformed());
        anim.SetBool(isAttacking, checkAttacking());
        #endregion

        //TODO
        #region TODO
        anim.SetBool(isUsingKrakenSkill, checkUsingKrakenSkill());
        anim.SetBool(isUsingBatSkill, checkUsingBatSkill());
        #endregion

        #region TODO_MAYHAVE
        anim.SetBool(isUsingGhostSkill, checkUsingGhostSkill());
        anim.SetBool(isWolf, checkWolf());
        anim.SetBool(isWolfRunning, checkWolfRunning());
        anim.SetBool(isWolfDamageTaken, checkWolfDamageTaken());
        #endregion
    }

    private bool checkIdel()
    {
        return states.isOnGround && !states.isMoving;
    }

    private bool checkRunning()
    {
        return states.isOnGround && states.isMoving;
    }

    private bool checkAirborn()
    {
        return !states.isOnGround && states.isMoving;
    }

    private bool checkAttacking()
    {
        anim.SetFloat("attackSpeed", attackAnimationDuration/actions.waitingTime);
        return actions.attackImpulse;
    }

    private bool checkUsingKrakenSkill()
    {
        return false;
    }

    private bool checkUsingGhostSkill()
    {
        return false;
    }

    private bool checkUsingBatDoubleJump()
    {
        return !states.isOnGround && states.isMoving && actions.hasBat && !actions.hasDoubleJump && actions.batFlapDoubleJumpImpulse;
    }

    private bool checkUsingBatSkill()
    {
        return false;
    }

    private bool checkBat()
    {
        return actions.isBat && !actions.batFlapImpulse;
    }

    private bool checkBatFlapping()
    {
        return actions.isBat && actions.batFlapImpulse;
    }

    private bool checkKraken()
    {
        return actions.isKraken;
    }

    private bool checkWolf()
    {
        return false;
    }

    private bool checkWolfRunning()
    {
        return false;
    }

    private bool checkWolfDamageTaken()
    {
        return false;
    }

    private bool checkTransformed()
    {
        return checkBat() || checkKraken() || checkWolf();
    }
}      