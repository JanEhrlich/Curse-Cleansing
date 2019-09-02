using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBullet : MonoBehaviour
{

    //handles
    Rigidbody2D rigidBody;
    ComponentBullet componentBullet;

    //tmp variables
    Vector3 attackDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        componentBullet = GetComponent<ComponentBullet>();
        componentBullet.timeUntilVanish = ComponentBullet.lifetime + Time.time;
       
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (componentBullet.timeUntilVanish < Time.time) Destroy(gameObject);

        if (attackDirection == Vector3.zero && rigidBody == null) return;
        rigidBody.velocity = attackDirection * ComponentBullet.speedForRangePirate;
    }

    /*
     * set the direction of of the bullet
     */
    public void SetDirection(Vector3 direction)
    {
        attackDirection = direction;
    }

    public Vector3 getAttackDirection()
    {
        return attackDirection;
    }
}
