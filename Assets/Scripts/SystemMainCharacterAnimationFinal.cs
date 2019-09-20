using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMainCharacterAnimationFinal : MonoBehaviour
{
    GameObject mainCharacterGameObject;
    SystemGameMaster systemGameMaster;
    Rigidbody2D rb;
    ComponentMainCharacterAction actions;
    ComponentMainCharacterState states;

    //Animator used for all Animations of Main Character
    Animator anim;

    /*
     * Animator.StringToHash is more efficient then passing strings
     */
    #region ClipNames   
    //Animator Parameters
    int isIdeling = Animator.StringToHash("isIdeling");
    int isRunning = Animator.StringToHash("isRunning");
    int isAirborn = Animator.StringToHash("isAirborn");
    int isRising = Animator.StringToHash("isRising");
    int isFalling = Animator.StringToHash("isFalling");
    int isHangingOnMarker = Animator.StringToHash("isHangingOnMarker");
    int isTurnedAroundDuringHanging = Animator.StringToHash("isTurnedAroundDuringHanging");
    int isGliding = Animator.StringToHash("isGliding");

    //No Weapon
    int jumpStartNoWeapon = Animator.StringToHash("Jump_NoWeapon");
    int jumpLandingtNoWeapon = Animator.StringToHash("Jump_Landing_NoWeapon");

    //With Weapon
    int attack = Animator.StringToHash("Attack");
    int attackDown = Animator.StringToHash("Attack_Down");
    int attackUp = Animator.StringToHash("Attack_Up");
    int jumpStart = Animator.StringToHash("Jump_Start");
    int jumpLanding = Animator.StringToHash("Jump_Landing");
    int attackJump = Animator.StringToHash("Attack_Jump");
    int attackJumpDown = Animator.StringToHash("Attack_Jump_Down");
    int attackJumpUp = Animator.StringToHash("Attack_Jump_Up");
    int batDoubleJump = Animator.StringToHash("Bat_Double_Jump");
    int batGlidingStart = Animator.StringToHash("Bat_Gliding_Start");
    int krakenAttack = Animator.StringToHash("Kraken_Attack");
    int krakenPullAlternative = Animator.StringToHash("Kraken_Pull_Alternative");
    int krakennPull = Animator.StringToHash("Player_Kraken_Skill");

    //Bat Form 
    int batFlap = Animator.StringToHash("Transformed_Bat_Flap");

    //Kraken Form
    int krakenJumpStart = Animator.StringToHash("Transformed_Kraken_Jump_Start");
    int krakenJumpLanding = Animator.StringToHash("Transformed_Kraken_Jump_Landing");
    #endregion

    //Layers
    int movement_NoWeapon;
    int actions_NoWeapon;
    int movement_WithWeapon;
    int actions_WithWeapon;
    int movement_Transformed_Bat;
    int actions_Transformed_Bat;
    int movement_Transformed_Kraken;
    int actions_Transformed_Kraken;

    //Set these Vaiables to the length of the attack Animation Clip
    float attackAnimationDuration = 0.5F;
    float attackUpAnimationDuration = 0.521F;
    float attackDownAnimationDuration = 0.521F;

    float attackJumpAnimationDuration = 0.417F;
    float attackJumpUpAnimationDuration = 0.417F;
    float attackJumpDownAnimationDuration = 0.417F;

    //Lookign Direction
    bool isLookingUp = false;
    bool isLookingDown = false;
    bool isLookingLeft = false;
    bool isLookingRight = false;

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();

        states = systemGameMaster.ComponentMainCharacterState;
        actions = systemGameMaster.ComponentMainCharacterAction;

        rb = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        anim = mainCharacterGameObject.GetComponent<Animator>();

        movement_NoWeapon = anim.GetLayerIndex("Movement_NoWeapon");
        actions_NoWeapon = anim.GetLayerIndex("Actions_NoWeapon");
        movement_WithWeapon = anim.GetLayerIndex("Movement_WithWeapon");
        actions_WithWeapon = anim.GetLayerIndex("Actions_WithWeapon");
        movement_Transformed_Bat = anim.GetLayerIndex("Movement_Transformed_Bat");
        actions_Transformed_Bat = anim.GetLayerIndex("Actions_Transformed_Bat");
        movement_Transformed_Kraken = anim.GetLayerIndex("Movement_Transformed_Kraken");
        actions_Transformed_Kraken = anim.GetLayerIndex("Actions_Transformed_Kraken");
    }

    public void Tick()
    {
        checkActiveLayer();

        anim.SetBool(isIdeling, checkIdel());
        anim.SetBool(isRunning, checkRunning());
        anim.SetBool(isAirborn, checkAirborn());
        anim.SetBool(isRising, checkRising());
        anim.SetBool(isFalling, checkFalling());
        anim.SetBool(isHangingOnMarker, checkHangingOnMarker());
        anim.SetBool(isTurnedAroundDuringHanging, checkTurnAroundOnMarker());
        anim.SetBool(isGliding, checkGliding());

        //Jump
        checkJumpImpulse();
        checkLandingImpulse();
        checkUsingDoubleJump();
        checkGlidingImpulse();

        //Attack
        checkLookingDirection();
        setAttackSpeed();
        checkAttack();

        //Bat
        checkBatFlapping();

        //Kraken
        checkUsingKrakenPull();
        checkUsingKrakenSkill();
    }

    private bool checkTurnAroundOnMarker()
    {
        return actions.hangingTurnAround;
    }

    private void checkGlidingImpulse()
    {
        if (actions.glidingImpulse)
        {
            anim.Play(batGlidingStart);
        }
    }

    private void checkAttack()
    {
        if (actions.attackImpulse)
        {
            if (anim.GetBool(isAirborn))
            {
                if (isLookingLeft || isLookingRight)
                {
                    anim.Play(attackJump);
                    return;
                }

                if (isLookingUp)
                {
                    anim.Play(attackJumpUp);
                    return;
                }

                if (isLookingDown)
                {
                    anim.Play(attackJumpDown);
                    return;
                }
            }
            else
            {
                if (isLookingLeft || isLookingRight)
                {
                    anim.Play(attack);
                    return;
                }

                if (isLookingUp)
                {
                    anim.Play(attackUp);
                    return;
                }

                if (isLookingDown)
                {
                    anim.Play(attackDown);
                    return;
                }
            }
        }
    }

    private bool checkGliding()
    {
        return states.isGliding;
    }

    private void checkActiveLayer()
    {
        if (!actions.hasSword && !actions.isBat && !actions.isKraken)
        {
            anim.SetLayerWeight(movement_NoWeapon, 1);
            anim.SetLayerWeight(movement_WithWeapon, 0);
            anim.SetLayerWeight(movement_Transformed_Bat, 0);
            anim.SetLayerWeight(movement_Transformed_Kraken, 0);
            return;
        }

        if (actions.hasSword && !actions.isBat && !actions.isKraken)
        {
            anim.SetLayerWeight(movement_NoWeapon, 0);
            anim.SetLayerWeight(movement_WithWeapon, 1);
            anim.SetLayerWeight(movement_Transformed_Bat, 0);
            anim.SetLayerWeight(movement_Transformed_Kraken, 0);
            return;
        }

        if (actions.isBat && !actions.isKraken)
        {
            anim.SetLayerWeight(movement_NoWeapon, 0);
            anim.SetLayerWeight(movement_WithWeapon, 0);
            anim.SetLayerWeight(movement_Transformed_Bat, 1);
            anim.SetLayerWeight(movement_Transformed_Kraken, 0);
            return;
        }

        if (!actions.isBat && actions.isKraken)
        {
            anim.SetLayerWeight(movement_NoWeapon, 0);
            anim.SetLayerWeight(movement_WithWeapon, 0);
            anim.SetLayerWeight(movement_Transformed_Bat, 0);
            anim.SetLayerWeight(movement_Transformed_Kraken, 1);
            return;
        }
    }

    private void checkLandingImpulse()
    {
        if (actions.landingImpulse)
        {
            anim.Play(jumpLandingtNoWeapon);
        }
    }

    private void checkJumpImpulse()
    {
        if (actions.jumpImpulse)
        {
            anim.Play(jumpStartNoWeapon);
        }
    }

    private bool checkFalling()
    {
        return checkAirborn() && rb.velocity.y < 0;
    }

    private bool checkRising()
    {
        return checkAirborn() && rb.velocity.y > 0;
    }

    private bool checkHangingOnMarker()
    {
        return actions.isHangingOnMarker;
    }

    private void checkUsingKrakenPull()
    {
        if (actions.krakenImpulse)
        {
            anim.Play(krakennPull);
            //anim.Play(krakenPullAlternative);
        }
    }

    private void checkLookingDirection()
    {
        if (actions.attackPositionOffset.x > 0)
        {
            isLookingRight = true;
            isLookingLeft = false;
            isLookingUp = false;
            isLookingDown = false;
        }

        if (actions.attackPositionOffset.x < 0)
        {
            isLookingRight = false;
            isLookingLeft = true;
            isLookingUp = false;
            isLookingDown = false;
        }

        if (actions.attackPositionOffset.y > 0)
        {
            isLookingRight = false;
            isLookingLeft = false;
            isLookingUp = true;
            isLookingDown = false;
        }

        if (actions.attackPositionOffset.y < 0)
        {
            isLookingRight = false;
            isLookingLeft = false;
            isLookingUp = false;
            isLookingDown = true;
        }
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

    private void setAttackSpeed()
    {
        if (anim.GetBool(isAirborn))
        {
            if (isLookingUp)
            {
                anim.SetFloat("attackSpeed", attackJumpUpAnimationDuration / actions.waitingTime);
            }

            if (isLookingDown)
            {
                anim.SetFloat("attackSpeed", attackJumpDownAnimationDuration / actions.waitingTime);
            }

            if (isLookingLeft || isLookingRight)
            {
                anim.SetFloat("attackSpeed", attackJumpAnimationDuration / actions.waitingTime);
            }
        }
        else
        {
            if (isLookingUp)
            {
                anim.SetFloat("attackSpeed", attackUpAnimationDuration / actions.waitingTime);
            }

            if (isLookingDown)
            {
                anim.SetFloat("attackSpeed", attackDownAnimationDuration / actions.waitingTime);
            }

            if (isLookingLeft || isLookingRight)
            {
                anim.SetFloat("attackSpeed", attackAnimationDuration / actions.waitingTime);
            }
        }
    }

    private void checkUsingKrakenSkill()
    {
        if (actions.krakenAttackImpulse)
        {
            anim.Play(krakenAttack);
        }
    }

    private void checkUsingDoubleJump()
    {
        if (actions.batFlapDoubleJumpImpulse)
        {
            anim.Play(batDoubleJump);
        }
    }

    private void checkBatFlapping()
    {
        if (actions.batFlapImpulse)
        {
            anim.Play(batFlap);
        }
    }
}