using System;
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
    public GameObject mainmainCharacterGameObject;
    public ComponentCameraState ComponentCameraState { get; } = new ComponentCameraState();
    public ComponentCutscene ComponentCutscene { get; } = new ComponentCutscene();
    public ComponentEnemyAction ComponentEnemyAction { get; } = new ComponentEnemyAction();
    public ComponentEnemyState ComponentEnemyState { get; } = new ComponentEnemyState();
    public ComponentGameState ComponentGameState { get; } = new ComponentGameState();
    public ComponentInput ComponentInput { get; } = new ComponentInput();
    public ComponentMainCharacterAction ComponentMainCharacterAction { get; } = new ComponentMainCharacterAction();
    public ComponentMainCharacterState ComponentMainCharacterState { get; } = new ComponentMainCharacterState();
    public ComponentProgression ComponentProgression { get; } = new ComponentProgression();
    public ComponentScene ComponentScene { get; } = new ComponentScene();
    public ComponentSpawn ComponentSpawn { get; } = new ComponentSpawn();
    public SystemUtility SystemUtility { get; } = new SystemUtility();

    private SystemInput systemInput;
    private SystemMainCharacterMovement systemMainCharacterMovement;
    private SystemMainCharacterMovementTransformed systemMainCharacterMovementTransformed;
    private SystemMainCharacterAnimation systemMainCharacterAnimation;

    void Start()
    {
        InitComponents();
        InitSystems();
    }

    void Update()
    {
        systemInput.Tick();
        systemMainCharacterAnimation.Tick();
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
        systemMainCharacterAnimation.Init(this);
    }

    public GameObject getMainCharacterGameobject()
    {
        return mainmainCharacterGameObject;
    }
}
