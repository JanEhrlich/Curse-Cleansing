using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Notifications : MonoBehaviour
{
    //Link to the GameMaster
    GameObject gameLogic;
    SystemGameMaster systemGameMaster;
    ComponentInput inputComp;

    public GameObject xboxNotification;
    public GameObject switchNotification;
    public GameObject text;

    public UnityEvent callOnEnter;
    public UnityEvent callOnInteraction;
    public UnityEvent callOnExit;

    //Variables needed for calculations
    private bool isInside = false;
    private bool interactable = false;
    private int playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        systemGameMaster = gameLogic.GetComponent<SystemGameMaster>();
        inputComp = systemGameMaster.ComponentInput;
        playerLayer = systemGameMaster.mainCharacterGameObject.layer;

        inputComp.AddInteractButtonPressFunction(CallOnInteraction);
        DeactivateAll();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inputComp.getCurrentControllerType() == ComponentInput.ControllerType.Xbox || switchNotification == null)
        {
            xboxNotification.SetActive(true);
        }
        else
        {
            switchNotification.SetActive(true);
        }

        if (text != null)
        {
            text.SetActive(true);
        }

        CallOnEnter();
        interactable = true;

        if (collision.gameObject.layer == playerLayer) {
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            isInside = false;
        }

        CallOnExit();

        DeactivateAll();
    }

    public void DeactivateAll()
    {
        xboxNotification.SetActive(false);

        if (switchNotification != null)
        {
            switchNotification.SetActive(false);
        }

        if (text != null)
        {
            text.SetActive(false);
        }

        interactable = false;
    }

    public void CallOnEnter()
    {
        if (callOnEnter != null)
        {
            callOnEnter.Invoke();
        }

    }

    public void CallOnInteraction()
    {

        if (callOnInteraction != null && isInside && interactable)
        {
            callOnInteraction.Invoke();
        }

    }

    public void CallOnExit()
    {
        if (callOnExit != null)
        {
            callOnExit.Invoke();
        }

    }
}
