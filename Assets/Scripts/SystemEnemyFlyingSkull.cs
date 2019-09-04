﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemEnemyFlyingSkull : SystemEnemy
{
    public enum Direction {LEFT, RIGHT};

    public Direction flyingDirection = Direction.RIGHT; 

    //tmp variables
    Vector2 movement;
    float timeUntilFlap = 0;
    float timeBetweenFlaps = 1f;
    int direction;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        if(flyingDirection == Direction.RIGHT){
            direction = 1;
        }else{
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        UpdatedSpeedAndJumpForce();

        Fly();
    }

    /*
     * simply update the current speed and jump force of the enemy
     */
    private void UpdatedSpeedAndJumpForce()
    {
        componentEnemyState.currentSpeed = ComponentEnemyState.speed * componentEnemyState.speedMultiplier;
        componentEnemyState.currentJumpForce = ComponentEnemyState.jumpForce/3.5f * componentEnemyState.jumpForceMultiplier;
    }


    void Fly(){
        if(timeUntilFlap <= Time.time){
            //multiply with direction, since this is either 1 or -1 for the correct direction
            rigidBody.velocity = new Vector2(direction * componentEnemyState.currentSpeed, componentEnemyState.currentJumpForce);
            timeUntilFlap  = Time.time + timeBetweenFlaps;
        }
    }


    /*
     * damage and knock back the player, if he runs into the enemy
     */
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //throw the enemy back, but not as hard, therefore 1/2
            rigidBody.velocity = new Vector2((mainCharacterGameObject.transform.position.x <= transform.position.x ? 1 : -1) * ComponentEnemyAction.knowBackPowerHorizontal/2, ComponentEnemyAction.knockBackPowerUp/2 );
            componentEnemyAction.timeUntillKnockBackEnd = Time.time + ComponentEnemyAction.knockBackTime/2;

            //throw the player back
            gameLogic.GetComponent<SystemMainCharacterMovement>().ReceiveDamage(ComponentEnemyState.damage, mainCharacterGameObject.transform.position.x <= transform.position.x ? -1 : 1);

            Destroy(gameObject);
        }
    }

    /*
     * remove collisions
     */
    void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
              Physics2D.IgnoreCollision(collider2d, collision);
        }
    }

}
