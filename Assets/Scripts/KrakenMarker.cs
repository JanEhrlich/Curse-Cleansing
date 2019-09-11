using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KrakenMarker : MonoBehaviour
{
    public GameObject krakenMarkerActive;
    public GameObject krakenMarkerPassive;

    private bool isActive;

    public bool isInteractionMarker;
    public UnityEvent callOnInteraction;

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

    public void ExecuteInteraction()
    {
        if (callOnInteraction != null && isInteractionMarker)
        {
            callOnInteraction.Invoke();
        }
    }

    private void OnDestroy()
    {
        systemGameMaster.DeregisterKrakenMarker(this.gameObject);
    }
}
