using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HardcodeAnimationPlayer : MonoBehaviour
{
    private Animator anim;

    private int animationNumber;

    public List<string> animName;
    public TextMeshPro text;
    public int remove;

    private bool once = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animationNumber = int.Parse(gameObject.name.Remove(0, remove).Replace(")",""));
        if (text)
        {
            text.text = animName[animationNumber];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!once)
        {
            anim.Play(animName[animationNumber]);
            once = true;
        }
    }
}
