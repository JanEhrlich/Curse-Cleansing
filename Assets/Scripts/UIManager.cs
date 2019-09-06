using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool isPlayerTransformed = false;
    public float currentKrakenLoadValue = 0;
    public float currentBatLoadValue = 0;

    public float curseDecaySpeed;
    public float transformDecayTime;

    public List<GameObject> heartImagesFull;

    public Image batImage;
    public Image krakenImage;

    public Image batRingWhite;
    public Image krakenRingWhite;

    public Image batRingTransparent;
    public Image krakenRingTransparent;

    private int lastHealth;
    public int currentHealth = 8;
    private int maxHealth = 8;
    private bool wasPlayerTransformedLastFrame;

    // Color Code for disabled State used everywhere else 7B7B7B with 200 Alpha
    public Color disabledColor;
    public Color enabledColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            currentKrakenLoadValue += 0.2f;
            currentBatLoadValue += 0.2f;

            if (currentKrakenLoadValue == 1)
            {
                currentKrakenLoadValue = 0;
                currentBatLoadValue = 0;
                isPlayerTransformed = !isPlayerTransformed;
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            currentHealth++;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            currentHealth--;
        }

        updateHearts();
        updateCurses();
    }

    private void updateHearts()
    {
        if (lastHealth == currentHealth)
            return;

        if (currentHealth > maxHealth)
            return;

        for (int i = 0; i < heartImagesFull.Count; ++i)
        {
            if (i < currentHealth)
            {
                heartImagesFull[i].SetActive(true);
            }
            else
            {
                heartImagesFull[i].SetActive(false);
            }
        }

        lastHealth = currentHealth;
    }

    private void updateCurses()
    {
        checkedTransformed();
        if (isPlayerTransformed)
        {
            setCurrentValueOfRing(currentKrakenLoadValue, krakenRingTransparent);
            setCurrentValueOfRing(currentBatLoadValue, batRingTransparent);
        }
        else
        {
            setCurrentValueOfRing(currentKrakenLoadValue, krakenRingWhite);
            setCurrentValueOfRing(currentBatLoadValue, batRingWhite);
        }

    }

    private void checkedTransformed()
    {
        if (wasPlayerTransformedLastFrame != isPlayerTransformed)
        {
            if (isPlayerTransformed)
            {
                batImage.color = disabledColor;
                batRingWhite.fillAmount = 0;
                batRingTransparent.fillAmount = 0;
                batRingWhite.enabled = false;

                krakenImage.color = disabledColor;
                krakenRingWhite.fillAmount = 0;
                krakenRingTransparent.fillAmount = 0;
                krakenRingWhite.enabled = false;
            }
            else
            {
                batImage.color = enabledColor;
                batRingTransparent.fillAmount = 1;
                batRingWhite.enabled = true;

                krakenImage.color = enabledColor;
                krakenRingTransparent.fillAmount = 1;
                krakenRingWhite.enabled = true;
            } 
            
        }

        wasPlayerTransformedLastFrame = isPlayerTransformed;
    }

    private void setCurrentValueOfRing(float percentage, Image ring)
    {
        if (0 <= percentage && percentage <= 1)
        {
            ring.fillAmount = Mathf.Lerp(ring.fillAmount,percentage,5*Time.deltaTime);
        }
    }
}
