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
    private ComponentCameraState componentCameraState;
    private ComponentCutscene componentCutscene;
    private ComponentEnemyAction componentEnemyAction;
    private ComponentEnemyState componentEnemyState;
    private ComponentGameState componentGameState;
    private ComponentInput componentInput;
    private ComponentMainCharacterAction componentMainCharacterAction;
    private ComponentMainCharacterState componentMainCharacterState;
    private ComponentPrograssion componentPrograssion;
    private ComponentScene componentScene;
    private ComponentSpawn componentSpawn;
    void Tick()
    {
        
    }

    void FixedTick()
    {
        
    }


    public ComponentCameraState ComponentCameraState { get => componentCameraState; }
    public ComponentCutscene ComponentCutscene { get => componentCutscene; }
    public ComponentEnemyAction ComponentEnemyAction { get => componentEnemyAction; }
    public ComponentEnemyState ComponentEnemyState { get => componentEnemyState; }
    public ComponentGameState ComponentGameState { get => componentGameState; }
    public ComponentInput ComponentInput { get => componentInput; }
    public ComponentMainCharacterAction ComponentMainCharacterAction { get => componentMainCharacterAction; }
    public ComponentMainCharacterState MainCharacterState { get => componentMainCharacterState;}
    public ComponentPrograssion ComponentPrograssion { get => componentPrograssion;  }
    public ComponentScene ComponentScene { get => componentScene; }
    public ComponentSpawn ComponentSpawn { get => componentSpawn; }
}
