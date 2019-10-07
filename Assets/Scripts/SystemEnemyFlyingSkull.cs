using System.Collections;
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
    int tmpdirection;

    // Start is called before the first frame update

    //has error, that collider2d is null, if base.Start is called from the Start method
    private void Awake()
    {
        base.Awake();
        if (flyingDirection == Direction.RIGHT)
        {
            tmpdirection = 1;
        }
        else
        {
            tmpdirection = -1;
        }
    }

    void FixedUpdate(){
        UpdatedSpeedAndJumpForce();
        UpdateDirection(flyingDirection);

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
            rigidBody.velocity = new Vector2(tmpdirection * componentEnemyState.currentSpeed, componentEnemyState.currentJumpForce);
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
            gameLogic.GetComponent<SystemMainCharacterMovement>().ReceiveDamage(componentEnemyState.damage, mainCharacterGameObject.transform.position.x <= transform.position.x ? -1 : 1);

            HandleDieEnemy();
        }
        else
        {
            Physics2D.IgnoreCollision(collider2d, collision.collider);
        }
    }

    void UpdateDirection(Direction update)
    {
        if (update == Direction.RIGHT)
        {
           tmpdirection = 1;
        }
        else
        {
           tmpdirection = -1;
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
