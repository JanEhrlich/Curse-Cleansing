using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * this class tracks the progression of the scenes
 * 
 * handled by Game-Master-System
 *  
 * TODO:
 *  -track the progression inside a level
 */

public class SystemProgression : MonoBehaviour
{
    /*
     * IMPORTANT @Jannis
     * This Whole Script is just a mockup it will be reworked entirely. Now it's just build for testing the level music system.
     */
    SystemGameMaster gameMaster;
    string currentLevel;
    Scene scene;

    //Tmp Variables
    string tmp_level;
    int levelNumber;

    public void Init(SystemGameMaster gm)
    {
        gameMaster = gm;
    }

    public void Tick()
    {
        scene = SceneManager.GetActiveScene();

        tmp_level = scene.name.Substring(0, 6);       

        if (tmp_level != currentLevel)
        {
            currentLevel = tmp_level;
            levelNumber = int.Parse(currentLevel.Substring(5, 1));
            PlayLevelMusic(levelNumber);
        }           
    }

    private void PlayLevelMusic(int level)
    {
        switch (level)
        {
            case 1:
                AudioManager.StartLevel1Audio();
                break;
            case 2:
                AudioManager.StartLevel2Audio();
                break;
            case 3:
                AudioManager.StartLevel3Audio();
                break;
            case 4:
                AudioManager.StartLevel4Audio();
                break;
            case 5:
                if(RespawnState.lastRespawn == 0)
                    AudioManager.StartLevel5_1Audio();
                break;
            default:
                AudioManager.StartLevel6Audio();
                break;
        }
    }
}
