using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGetHitByBullet : MonoBehaviour
{
    //handles
    SystemMainCharacterMovement systemMainCharacterMovement;
    private void Start()
    {
        systemMainCharacterMovement = GameObject.Find("GameLogic").GetComponent<SystemMainCharacterMovement>();
    }

    /*
     * damage and knock back the player, if he runs into a bullet
     */
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Bullet"))
        {
            systemMainCharacterMovement.BulletHit(collision, ComponentBullet.damage);
        }

    }
}
