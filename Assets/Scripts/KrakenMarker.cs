using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenMarker : MonoBehaviour
{
    public GameObject krakenMarkerActive;
    public GameObject krakenMarkerPassive;

    GameObject gameLogic;
    SystemGameMaster systemGameMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        systemGameMaster.RegisterNewKrakenMarker(this.gameObject);

        MarkAsPassive();
    }

    public void MarkAsPassive()
    {
        isActive = false;
        GameObject go;

        if (this.transform.childCount != 0)
        {
            go = this.transform.GetChild(0).gameObject;
            go.transform.parent = null;
            Destroy(go);
        }

        go = Instantiate(krakenMarkerPassive, transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = this.transform;
    }

    public void MarkAsActive()
    {
        isActive = true;
        GameObject go;

        if (this.transform.childCount != 0)
        {
            go = this.transform.GetChild(0).gameObject;
            go.transform.parent = null;
            Destroy(go);
        }

        go = Instantiate(krakenMarkerActive, transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = this.transform;
    }
}
