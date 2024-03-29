﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class is the main entry point,
 * also used to handle resume after pausing a game
 * 
 * TODO:
 *  -handle all logic
 *  -call all classes
 */

public class SystemGameMaster : MonoBehaviour
{
    public GameObject mainCharacterGameObject;
    //components for every enemy, update list on enemy spawn and enemy destroy
    public List<GameObject> enemys = new List<GameObject>();
    public ComponentCameraState ComponentCameraState { get; } = new ComponentCameraState();
    public ComponentCutscene ComponentCutscene { get; } = new ComponentCutscene();
    public ComponentGameState ComponentGameState { get; } = new ComponentGameState();
    public ComponentInput ComponentInput { get; } = new ComponentInput();
    public ComponentMainCharacterAction ComponentMainCharacterAction { get; set; } = new ComponentMainCharacterAction();
    public ComponentMainCharacterState ComponentMainCharacterState { get; set; } = new ComponentMainCharacterState();
    public ComponentProgression ComponentProgression { get; } = new ComponentProgression();
    public ComponentScene ComponentScene { get; } = new ComponentScene();
    public ComponentSpawn ComponentSpawn { get; } = new ComponentSpawn();
    public ComponentKrakenMarker ComponentKrakenMarker { get; } = new ComponentKrakenMarker();
    public SystemUtility SystemUtility { get; } = new SystemUtility();

    private SystemInput systemInput;
    private SystemMainCharacterMovement systemMainCharacterMovement;
    private SystemMainCharacterMovementTransformed systemMainCharacterMovementTransformed;
    private SystemMainCharacterAnimation systemMainCharacterAnimation;
    private SystemMainCharacterAnimationFinal systemMainCharacterAnimationFinal;
    private SystemKrakenMarker systemKrakenMarker;
    private SystemProgression systemProgression;
    private GameObject systemEventObject;

    void Awake()
    {
        InitComponents();
        InitSystems();
    }

    void Update()
    {
        systemInput.Tick();
        systemMainCharacterMovementTransformed.Tick();

        if (systemMainCharacterAnimation) {
            systemMainCharacterAnimation.Tick();
        }
        else
        {
            systemMainCharacterAnimationFinal.Tick();
        }

        systemKrakenMarker.Tick();
        systemProgression.Tick();
    }

    void FixedUpdate()
    {
        systemMainCharacterMovement.FixedTick();
        systemMainCharacterMovementTransformed.FixedTick();
    }

    private void InitComponents()
    {
        //TODO Initialisations which are needed can be put in here
    }

    private void InitSystems()
    {
        systemInput = GetComponent<SystemInput>();
        systemInput.Init(this);

        systemMainCharacterMovement = GetComponent<SystemMainCharacterMovement>();
        systemMainCharacterMovement.Init(this);

        systemMainCharacterMovementTransformed = GetComponent<SystemMainCharacterMovementTransformed>();
        systemMainCharacterMovementTransformed.Init(this);

        systemMainCharacterAnimation = GetComponent<SystemMainCharacterAnimation>();
        systemMainCharacterAnimationFinal = GetComponent<SystemMainCharacterAnimationFinal>();

        if (systemMainCharacterAnimation)
        {
            systemMainCharacterAnimation.Init(this);
        }
        else
        {
            systemMainCharacterAnimationFinal.Init(this);
        }

        systemKrakenMarker = GetComponent<SystemKrakenMarker>();
        systemKrakenMarker.Init(this);

        systemProgression = GetComponent<SystemProgression>();
        systemProgression.Init(this);

        systemEventObject = GameObject.Find("Events");
    }

    /*
     * Call this function from the enemy to register as new
     */
    public void RegisterNewEnemy(GameObject enemy)
    {
        enemys.Add(enemy);
    }

    public void RegisterNewKrakenMarker(GameObject marker)
    {
        systemKrakenMarker.AddMarker(marker);
    }

    public void DeregisterKrakenMarker(GameObject marker)
    {
        systemKrakenMarker.RemoveMarker(marker);
    }

    public void EnemyWasKilled(GameObject enemy)
    {
        if (!enemys.Remove(enemy))
        {
            //TODO what then?
            Debug.Log("Enemy not in systemGameMaster enemyList");
        }
    }

    public GameObject getMainCharacterGameobject()
    {
        return mainCharacterGameObject;
    }

    public GameObject getSystemEvents()
    {
        return systemEventObject;
    }

}
