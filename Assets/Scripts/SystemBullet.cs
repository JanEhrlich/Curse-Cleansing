using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBullet : MonoBehaviour
{

    //handles
    Rigidbody2D rigidbody;
    ComponentBullet componentBullet;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector3.right * ComponentBullet.speedForRangePirate;
    }
}
