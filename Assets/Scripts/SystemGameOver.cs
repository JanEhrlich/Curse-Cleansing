using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SystemGameOver : MonoBehaviour
{
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
        gameMaster = GameObject.Find("GameLogic").GetComponent<SystemGameMaster>();
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        ui = GameObject.Find("UI").GetComponent<UIManager>();
        componentMainCharacterState = gameMaster.ComponentMainCharacterState;
        gameOverScreen = gameObject.transform.GetChild(0).gameObject;
        dead = false;

        ResetStates();
    }

    private void Update()
    {




        if (!dead && componentMainCharacterState.health <= 0)
        {
            timeUntilRestart = Time.realtimeSinceStartup+timeForGameOverScreen;
            dead = true;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
            
        if(dead && timeUntilRestart < Time.realtimeSinceStartup)
        {

            gameOverScreen.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //Start();
            //ResetStates();
        }
    }


    void ResetStates()
    {
        //componentMainCharacterState = new ComponentMainCharacterState(); //TODO does this work as intendet????????
        systemEvent.RespawnPlayer();
    }
}
