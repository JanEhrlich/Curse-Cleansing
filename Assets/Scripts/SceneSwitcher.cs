using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            LoadSceneByNumber(nextLevelNumber);
        }
    }
}
