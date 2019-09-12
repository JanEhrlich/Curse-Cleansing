using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnState : MonoBehaviour
{
    public static GameObject current;
    public static int currentIndex;
    public static SystemGameMaster gameMaster;
    public static GameObject gameLogic;

    public static int lastRespawn = 0;

    private void Awake()
    {
        if (current != null && current != this)
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentIndex != SceneManager.GetActiveScene().buildIndex)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            SwitchScene();
        }
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
