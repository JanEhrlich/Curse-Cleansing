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
    string isJumping = "isJumping";
    string isAttacking = "isAttacking";
    string isUsingKrakenSkill = "isUsingKrakenSkill";
    string isUsingGhostSkill = "isUsingGhostSkill";
    string isUsingBatDoubleJump = "isUsingBatDoubleJump";
    string isUsingBatSkill = "isUsingBatSkill";
    string isBatIdle = "isBatIdle";
    string isBatFlapping = "isBatFlapping";
    string isKrakenIdle = "isKrakenIdle";
    string isWolfIdle = "isWolfIdle";
    string isWolfRunning = "isWolfRunning";
    string isWolfDamageTaken = "isWolfDamageTaken";
    #endregion

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
        #region Finished
        anim.SetBool(isIdeling, checkIdel());
        anim.SetBool(isRunning, checkRunning());
        anim.SetBool(isJumping, checkJumping());
        anim.SetBool(isUsingBatDoubleJump, checkUsingBatDoubleJump());
        anim.SetBool(isBatIdle, checkBatIdle());
        anim.SetBool(isBatFlapping, checkBatFlapping());
        anim.SetBool(isKrakenIdle, checkKrakenIdle());
        #endregion

        //TODO
        #region TODO
        anim.SetBool(isAttacking, checkAttacking());
        anim.SetBool(isUsingKrakenSkill, checkUsingKrakenSkill());
        anim.SetBool(isUsingBatSkill, checkUsingBatSkill());
        #endregion

        #region TODO_MAYHAVE
        anim.SetBool(isUsingGhostSkill, checkUsingGhostSkill());
        anim.SetBool(isWolfIdle, checkWolfIdle());
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

    private bool checkJumping()
    {
        return !states.isOnGround && states.isMoving;
    }

    private bool checkAttacking()
    {
        return false;
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

    private bool checkBatIdle()
    {
        return actions.isBat && !actions.batFlapImpulse;
    }

    private bool checkBatFlapping()
    {
        return actions.isBat && actions.batFlapImpulse;
    }

    private bool checkKrakenIdle()
    {
        return actions.isKraken;
    }

    private bool checkWolfIdle()
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
}      