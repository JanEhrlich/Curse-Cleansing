using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Information about the Player
 */

public class ComponentMainCharacterState : MonoBehaviour
{
    private Transform transform;
    private Rigidbody2D ridgetBody2D;
    private float speed;

    public float Speed { get { return speed; } set { speed = value; } }
    public Transform Transform { get{ return transform; } set {transform = value; } }
    public Rigidbody2D Rigidbody2D { get { return ridgetBody2D; } set{ ridgetBody2D = value; } }
}
