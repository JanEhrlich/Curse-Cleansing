using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperAttackMainCharacter : MonoBehaviour
{
    SystemMainCharacterMovement characterMovement;
    private void Start()
    {
        characterMovement = GameObject.Find("GameLogic").GetComponent<SystemMainCharacterMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterMovement.AttackWithCollider(collision);
    }
}
