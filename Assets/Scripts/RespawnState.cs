using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnState : MonoBehaviour
{
    public static GameObject current;
    public static int currentIndex = 0;
    public static SystemGameMaster gameMaster;
    public static GameObject gameLogic;

    public static int lastRespawn = 0;

    private void Awake()
    {
        if (currentIndex != SceneManager.GetActiveScene().buildIndex)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            //Debug.Log("DID SWITCH");
            SwitchScene();
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        current = gameObject;
        currentIndex = SceneManager.GetActiveScene().buildIndex;

    }


    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
    }


    /*
     * call after switching scene, or respawning
     */
    public static void UpdatePointer()
    {
        gameLogic = GameObject.Find("GameLogic");
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
    }


    public static void SwitchScene()
    {
        lastRespawn = 0;
    }
}
