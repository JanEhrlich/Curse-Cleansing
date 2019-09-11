using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPrisonExit : MonoBehaviour
{
    public GameObject doorClose;

    public GameObject doorOpen;
    public GameObject doorLight;
    public GameObject switchClose;
    public GameObject switchOpen;

    private bool hasSword = false;
    private bool inside = false;

    public void Update()
    {     
        if (Input.GetButtonDown("Attack") && hasSword && inside)
        {
            doorClose.SetActive(false);
            doorOpen.SetActive(true);
            doorLight.SetActive(true);
            switchClose.SetActive(false);
            switchOpen.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    public void UnlockedSword()
    {
        hasSword = true;
    }

    public void Enter()
    {
        inside = true;
    }

    public void Leave()
    {
        inside = false;
    }
}
