using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Notification_Controlle_Switcher : MonoBehaviour
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

    //Variables needed for calculations
    private bool callOnEnterOnce = true;
    private bool callOnInteractionOnce = true;
    private bool isInside = false;
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

        if (collision.gameObject.layer == playerLayer) {
            isInside = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            isInside = false;
        }

        DeactivateAll();
    }

    private void DeactivateAll()
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
    }

    public void CallOnEnter()
    {
        if (callOnEnter != null && callOnEnterOnce)
        {
            callOnEnter.Invoke();
            callOnEnterOnce = false;
        }

    }

    public void CallOnInteraction()
    {
        if (isInside)
        {
            Debug.Log("jo");
        }

        if (callOnInteraction != null && isInside)
        {
            callOnInteraction.Invoke();
            callOnInteractionOnce = false;
        }

    }
}
