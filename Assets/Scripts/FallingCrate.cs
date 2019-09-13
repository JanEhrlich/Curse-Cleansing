using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCrate : MonoBehaviour
{
    public GameObject Lever1;

    public GameObject Lever2;

    public GameObject Wagon;

    public GameObject deactivate1;
    public GameObject deactivate2;
    public GameObject deactivate3;
    public GameObject deactivate4;

    private bool inside = false;

    private bool activated = false;
    float speed = 0.01f;
    float step = 0f;
    float smoothTime = 1F;

    float despawnTime = 3.2f;
    private Vector3 velocity = Vector3.zero;

    public void Update()
    {
        if(activated && despawnTime < Time.time) Destroy(this.gameObject);

        if (Input.GetButtonDown("Interact") && inside)
        {
            Lever1.SetActive(false);
            Lever2.SetActive(true);
            deactivate1.SetActive(false);
            deactivate2.SetActive(false);
            deactivate3.SetActive(false);
            deactivate4.SetActive(false);
            activated = true;
            despawnTime += Time.time;
            //
        }

        if (activated && despawnTime > Time.time)
        {
            step = speed * Time.deltaTime;
            //Wagon.transform.position = Vector2.MoveTowards(Wagon.transform.position, Wagon.transform.GetChild(0).transform.position, step);
            Wagon.transform.position = Vector3.SmoothDamp(Wagon.transform.position, Wagon.transform.GetChild(0).transform.position, ref velocity, smoothTime, 3f);
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
