using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemProgressionLevel3 : MonoBehaviour
{
    SystemEvent systemEvent;
    SystemSpawn systemSpawn;
    GameObject gameLogic;
    SystemGameMaster gameMaster;


    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemEvent = GameObject.Find("Events").GetComponent<SystemEvent>();
        systemSpawn = gameLogic.GetComponent<SystemSpawn>();
        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPlayerHasBat()
    {
        Debug.Log("called");
        gameMaster.ComponentMainCharacterAction.hasBat = true;
    }

}
