using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SystemGameOver : MonoBehaviour
{
    GameObject gameLogic;
    SystemGameMaster gameMaster;
    ComponentMainCharacterState componentMainCharacterState;
    UIManager ui;
    GameObject gameOverScreen;
    SystemEvent systemEvent;

    bool dead = false;
    float timeForGameOverScreen = 3f;
    float timeUntilRestart = 0;


    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        ui = GameObject.Find("UI").GetComponent<UIManager>();
        componentMainCharacterState = gameMaster.ComponentMainCharacterState;
        gameOverScreen = gameObject.transform.GetChild(0).gameObject;
        dead = false;
        systemEvent.playerDied = false;

        HandleRespawn();
    }

    private void Update()
    {




        if (!dead && componentMainCharacterState.health <= 0)
        {
            timeUntilRestart = Time.realtimeSinceStartup+timeForGameOverScreen;
            dead = true;
            systemEvent.playerDied = true;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
            
        if(dead && timeUntilRestart < Time.realtimeSinceStartup)
        {

            gameOverScreen.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    void HandleRespawn()
    {
        //used for respawn
        RespawnState.UpdatePointer();

        if (RespawnState.lastRespawn != 0)
        {
            RespawnState.gameMaster.getMainCharacterGameobject().transform.position = systemEvent.respawnPoints[RespawnState.lastRespawn].transform.position;


            if (RespawnState.currentIndex == 0) RespawnState.gameMaster.GetComponent<SystemMainCharacterMovement>().UnlockKrakenOnRespawn();

            //TODO write "fight" ...
            if (RespawnState.currentIndex == 4)
            {
                GameObject.Find("Pre-Figth").SetActive(false);
                AudioManager.StartLevel5_2Audio();
            }
        }
    }

}
