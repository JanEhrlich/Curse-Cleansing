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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Attack"))
        {
            doorClose.SetActive(false);
            doorOpen.SetActive(true);
            doorLight.SetActive(true);
            switchClose.SetActive(false);
            switchOpen.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
