using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTraps : SystemEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    void FixedUpdate(){
        if(Physics2D.IsTouchingLayers(mainCharacterGameObject.GetComponentInChildren<BoxCollider2D>(), systemGameMaster.SystemUtility.TransformToLayerMask(LayerMask.NameToLayer("Traps")))){
            gameLogic.GetComponent<SystemMainCharacterMovement>().ReceiveDamage(componentEnemyState.damage, 0);
        }
    }

      /*
     * damage and knock back the player, if he runs into the enemy
     */
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //throw the player back
            gameLogic.GetComponent<SystemMainCharacterMovement>().ReceiveDamage(componentEnemyState.damage, 0);
        }
    }

}
