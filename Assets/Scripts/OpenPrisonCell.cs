using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPrisonCell : MonoBehaviour
{
    public GameObject door;
    public GameObject key;
    public GameObject notification;

    GameObject gameLogic;
    SystemGameMaster systemGameMaster;

    private bool pickedUp;
    private bool keyRemoved;
    private float waitTimer = 1;

    public void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        float i = Time.time;
    }

    public void Update()
    {
        if (pickedUp)
        {
            if (!keyRemoved)
            {
                systemGameMaster.mainCharacterGameObject.GetComponent<Animator>().Play("Player_Kraken_Skill");
                systemGameMaster.ComponentMainCharacterAction.currentKrakenCounter += ComponentMainCharacterAction.costKrakenAbility;
                Destroy(key);
                keyRemoved = true;
            }

            if (waitTimer <= 0)
            {
                Destroy(door);
                Destroy(notification);
                Destroy(this.gameObject);
            }

            waitTimer -= Time.deltaTime;
            //TODO: Play Cutscene when obtained & change progression state
        }
    }

    public void KeyObtained(){
        pickedUp = true;
    }
}
