using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPause : MonoBehaviour
{
    public static bool isPaused = false;

    SystemGameMaster gameMaster;
    ComponentInput componentInput;
    GameObject pauseScreen;
    void Start()
    {
        gameMaster = GameObject.Find("GameLogic").GetComponent<SystemGameMaster>();
        componentInput = gameMaster.ComponentInput;
        pauseScreen = gameObject.transform.GetChild(0).gameObject;
        isPaused = false;
        componentInput.AddPauseButtonPressFunction(flipPause);
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    void flipPause()
    {
        isPaused = !isPaused;
        //Debug.Log("PAUSE"+isPaused);
        if (isPaused)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
