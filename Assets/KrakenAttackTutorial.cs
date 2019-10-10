using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenAttackTutorial : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Trap;
    private bool inside = false;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetButtonDown("Tentacle") && inside)
        {
            //doorClose.SetActive(false);
            //doorOpen.SetActive(true);
            //doorLight.SetActive(true);
            Enemy.SetActive(false);
            Trap.SetActive(false);
            Destroy(this.gameObject);
        }
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
