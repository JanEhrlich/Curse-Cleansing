using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class handles the special attacks from the curses
 * 
 * handled by Game-Master-System
 * 
 * TODO:
 *  -maintain the curse-bar
 *  -only allow one ability at a time
 */

public class SystemMainCharacterAbilities : MonoBehaviour
{
    //handles
    GameObject mainCharacterGameObject;
    Rigidbody2D rigidBody;
    PolygonCollider2D hitCollider;
    SystemGameMaster systemGameMaster;
    ComponentInput componentInput;
    ComponentMainCharacterAction componentMainCharacterAction;
    ComponentMainCharacterState componentMainCharacterState;
    Transform mainCharacterTransform;

    ComponentEnemyState componentEnemyState; //TEST

    public void Init(SystemGameMaster gameMaster)
    {
        systemGameMaster = gameMaster;
        mainCharacterGameObject = systemGameMaster.getMainCharacterGameobject();
        rigidBody = mainCharacterGameObject.GetComponent<Rigidbody2D>();
        hitCollider = mainCharacterGameObject.GetComponentInChildren<PolygonCollider2D>();
        componentInput = systemGameMaster.ComponentInput;
        componentMainCharacterState = systemGameMaster.ComponentMainCharacterState;
        componentMainCharacterAction = systemGameMaster.ComponentMainCharacterAction;
        mainCharacterTransform = mainCharacterGameObject.transform;
        componentEnemyState = systemGameMaster.ComponentEnemyState; //TEST

        //add button functions
        componentInput.AddAttackButtonPressFunction(receivedAttackPressInput);
    }

    void receivedAttackPressInput()
    {
        componentMainCharacterAction.attack = true;
    }

    public void Tick()
    {
        
    }

    public void FixedTick()
    {
        if (componentMainCharacterAction.attack)
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, 2f,componentEnemyState.layerMask);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
