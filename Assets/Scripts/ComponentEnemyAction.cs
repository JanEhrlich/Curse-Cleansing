using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentEnemyAction : MonoBehaviour
{
    /*
     * Hardcoded Variables which shape the games behaviour
     */
    #region StaticVariables
    public static float knockBackTime = 0.5f;                     //time untill the enemy can attack again
    public static float knockBackPowerUp = 3f;                  //the upwards velocity
    public static float knowBackPowerHorizontal = 2f;           //the horizontal velocity
    
    
    #endregion


    /*
     * Variables which will be set on Startup and never changed later
     */
    #region SetOnInitVariables
    public Vector3 attackPositionOffset;                        // position of the enemys attack 
    public float followRange = 7f;                       //range where the enemy can follow the player
    public float attackRange = 2f;                       //mostly for close combat
    public float timeToAttack = 0.5f;                 //time for attack, if the player enters the viewfield of the enemy
    public float timeBetweenAttacks = 2f;                //time between consecutive attacks, needs again timeToAttack
    public Vector2 attackBoxNormal;                         //attackBox ist the area of applying dmg
    #endregion


    /*
     * Dynamic Variables which will change very often depending on the game state and player's actions
     */
    #region DynamictVariables
    public float followRangeMultiplier = 1f;                    //TODO maybe widen the attack radius, such that the player can not run in and out
    public float timeForNextAttack = 0f;                        //time when the next attack occurs
    public float timeUntillKnockBackEnd = 0f;                   //current time left untill enemy can move again
    public float distanceToMainCharacter;                       //current distance to the main character
    #endregion

}
