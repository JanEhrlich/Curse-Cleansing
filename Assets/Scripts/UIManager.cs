﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Link to the GameMaster
    GameObject gameLogic;
    SystemGameMaster systemGameMaster;
    ComponentMainCharacterState state;
    ComponentMainCharacterAction action;

    //Set if you want to display the remaining Time to Transform back
    public bool showTimeWhileTransformed = true;

    //The Max amount of health. Don't use a greater Value than 8 because there are no more grafics for this.
    public int maxHealth = 8;

    //Link to the UI images
    public List<GameObject> heartImagesFull;
    public List<GameObject> heartImagesEmpty;

    public Image batImage;
    public Image krakenImage;

    public Image batRingWhite;
    public Image krakenRingWhite;

    public Image batRingTransparent;
    public Image krakenRingTransparent;

    public TextMeshProUGUI krakenLockTimer;
    public TextMeshProUGUI batLockTimer;

    //Tmp Values for Calculations
    private int lastHealth;

    private bool isPlayerTransformed;
    private bool wasPlayerTransformedLastFrame;

    private float currentDecayValueKraken;
    private float currentDecayValueBat;

    // Color Code for disabled State used everywhere else 7B7B7B with 200 Alpha
    public Color disabledColor;
    public Color enabledColor;

    private bool enabledOnceKraken;
    private bool enabledOnceBat;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        state = systemGameMaster.ComponentMainCharacterState;
        action = systemGameMaster.ComponentMainCharacterAction;

        UpdateHeartAmount();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHearts();

        #region CheckIfCurseWasAlreadyUnlocked
        if (!action.hasKraken)
        {
            //disable Kraken UI
            krakenImage.enabled = false;
            krakenRingTransparent.enabled = false;
            krakenRingWhite.enabled = false;
            krakenLockTimer.enabled = false;
        }
        else
        {
            if (!enabledOnceKraken)
            {
                enabledOnceKraken = true;

                krakenImage.enabled = true;
                krakenRingTransparent.enabled = true;
                krakenRingWhite.enabled = true;
            }
        }

        if (!action.hasBat)
        {
            //disable Bat UI
            batImage.enabled = false;
            batRingTransparent.enabled = false;
            batRingWhite.enabled = false;
            batLockTimer.enabled = false;
        }
        else
        {
            if (!enabledOnceBat)
            {
                enabledOnceBat = true;

                batImage.enabled = true;
                batRingTransparent.enabled = true;
                batRingWhite.enabled = true;
            }
        }

        #endregion

        if (action.hasKraken ||action.hasBat || action.hasGhost || action.hasWolf)
        {
            UpdateVariables();
            UpdateCurses();
        }
    }

    private void UpdateHeartAmount()
    {
        for (int i = 0; i < heartImagesFull.Count; ++i)
        {
            if (i >= maxHealth)
            {
                heartImagesFull[i].SetActive(false);
                heartImagesEmpty[i].SetActive(false);
            }
        }
    }

    private void UpdateVariables()
    {
        isPlayerTransformed = action.isBat || action.isKraken || action.isWolf || action.isGhost;

        if (isPlayerTransformed)
        {
            if (action.isKraken)
            {
                currentDecayValueKraken = (action.timeUntillNormal - Time.time) / ComponentMainCharacterAction.durationTransformationKrake;
                currentDecayValueKraken = 1 - currentDecayValueKraken;
                currentDecayValueBat = (action.timeUntillNormal - Time.time) / ComponentMainCharacterAction.durationTransformationKrake;
                currentDecayValueBat = 1 - currentDecayValueBat;
            }
            else
            {
                if (action.isBat)
                {
                    currentDecayValueKraken = (action.timeUntillNormal - Time.time) / ComponentMainCharacterAction.durationTransformationBat;
                    currentDecayValueKraken = 1 - currentDecayValueKraken;
                    currentDecayValueBat = (action.timeUntillNormal - Time.time) / ComponentMainCharacterAction.durationTransformationBat;
                    currentDecayValueBat = 1 - currentDecayValueBat;
                }
            }

            if (currentDecayValueKraken > 1)
            {
                currentDecayValueKraken = 1;
                currentDecayValueBat = 1;
            }
        }
        else
        {
            currentDecayValueKraken = 1;
            currentDecayValueBat = 1;
        }
    }

    private void UpdateHearts()
    {
        if (lastHealth == state.health)
            return;

        if (state.health > maxHealth)
            return;

        for (int i = 0; i < heartImagesFull.Count; ++i)
        {
            if (i < state.health)
            {
                heartImagesFull[i].SetActive(true);
            }
            else
            {
                heartImagesFull[i].SetActive(false);
            }
        }

        lastHealth = state.health;
    }

    private void UpdateCurses()
    {
        CheckedTransformed();
        if (isPlayerTransformed)
        {
            setCurrentValueOfRing(currentDecayValueKraken, krakenRingTransparent);
            setCurrentValueOfRing(currentDecayValueBat, batRingTransparent);
            setTimerText((action.timeUntillNormal - Time.time), krakenLockTimer);
            setTimerText((action.timeUntillNormal - Time.time), batLockTimer);
        }
        else
        {
            setCurrentValueOfRing(action.currentKrakenCounter, krakenRingWhite);
            setCurrentValueOfRing(action.currentBatCounter, batRingWhite);
        }

    }

    private void CheckedTransformed()
    {
        if (wasPlayerTransformedLastFrame != isPlayerTransformed)
        {
            if (isPlayerTransformed)
            {
                if (action.hasBat)
                {
                    batImage.enabled = true;
                    batImage.color = disabledColor;

                    batRingTransparent.enabled = true;
                    batRingTransparent.fillAmount = 0;

                    batRingWhite.fillAmount = 0;
                    batRingWhite.enabled = false;

                    batLockTimer.enabled = true;
                }
                else
                {
                    batImage.enabled = false;
                    batRingTransparent.enabled = false;
                    batRingWhite.enabled = false;
                    batLockTimer.enabled = false;
                }

                if (action.hasKraken)
                {
                    krakenImage.enabled = true;
                    krakenImage.color = disabledColor;

                    krakenRingTransparent.enabled = true;
                    krakenRingTransparent.fillAmount = 0;

                    krakenRingWhite.fillAmount = 0;
                    krakenRingWhite.enabled = false;

                    krakenLockTimer.enabled = true;
                }
                else
                {
                    krakenImage.enabled = false;
                    krakenRingTransparent.enabled = false;
                    krakenRingWhite.enabled = false;
                    krakenLockTimer.enabled = false;
                }
            }
            else
            {
                if (action.hasBat)
                {
                    batImage.enabled = true;
                    batImage.color = enabledColor;

                    batRingTransparent.enabled = true;
                    batRingTransparent.fillAmount = 1;

                    batRingWhite.enabled = true;

                    batLockTimer.enabled = false;
                }
                else
                {
                    batImage.enabled = false;
                    batRingTransparent.enabled = false;
                    batRingWhite.enabled = false;
                }

                if (action.hasKraken)
                {
                    krakenImage.enabled = true;
                    krakenImage.color = enabledColor;

                    krakenRingTransparent.enabled = true;
                    krakenRingTransparent.fillAmount = 1;

                    krakenRingWhite.enabled = true;

                    krakenLockTimer.enabled = false;
                }
                else
                {
                    krakenImage.enabled = false;
                    krakenRingTransparent.enabled = false;
                    krakenRingWhite.enabled = false;
                }
            } 
            
        }

        wasPlayerTransformedLastFrame = isPlayerTransformed;
    }

    private void setTimerText(float decayValue, TextMeshProUGUI lockTimerText)
    {
        if (decayValue > 0 && showTimeWhileTransformed)
        {
            lockTimerText.SetText(decayValue.ToString("0.0").Replace(",", "."));
        }
        else
        {
            lockTimerText.SetText("");
        }
    }

    private void setCurrentValueOfRing(float percentage, Image ring)
    {
        if (0 <= percentage && percentage <= 1)
        {
            ring.fillAmount = Mathf.Lerp(ring.fillAmount, percentage, 5 * Time.deltaTime);
        }
    }

    public void resetCurrentValues()
    {
        currentDecayValueBat = 0f;
        currentDecayValueKraken = 0f;
        lastHealth = maxHealth;
        //TODO RESET THE HEALTH

        isPlayerTransformed = false;
        wasPlayerTransformedLastFrame = false;

}
}
