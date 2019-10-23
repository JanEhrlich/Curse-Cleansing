﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public int nextLevelNumber;

    //Link to the GameMaster
    GameObject gameLogic;
    SystemGameMaster systemGameMaster;
    private int playerLayer;

    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        playerLayer = systemGameMaster.mainCharacterGameObject.layer;
    }

    void Update()
    {
        //Reload Level
        if (Input.GetKeyDown("t"))
        {
            LoadSceneByNumber(nextLevelNumber - 1);
        }

        //Load next Level
        if (Input.GetKeyDown("z"))
        {
            if (nextLevelNumber < 8)
            {
                LoadSceneByNumber(nextLevelNumber);
            }
        }

        //Load previous Level
        if (Input.GetKeyDown("r"))
        {
            if (nextLevelNumber > 1)
            {
                LoadSceneByNumber(nextLevelNumber-2);
            }
        }
    }

    public void LoadSceneByNumber(int num)
    {
        SceneManager.LoadScene(num);
    }

    public void LoadEscapeLevel()
    {
        LoadSceneByNumber(1);
    }
    public void LoadHideoutLevel()
    {
        LoadSceneByNumber(2);
    }

    public void LoadRoadToTavernLevel()
    {
        LoadSceneByNumber(3);
    }

    public void LoadTavernLevel()
    {
        LoadSceneByNumber(4);
    }

    public void LoadHideoutAfterTavernLevel()
    {
        LoadSceneByNumber(5);
    }

    public void LoadHarborLevel()
    {
        LoadSceneByNumber(6);
    }

    public void LoadShipLevel()
    {
        LoadSceneByNumber(7);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            LoadSceneByNumber(nextLevelNumber);
        }
    }
}
