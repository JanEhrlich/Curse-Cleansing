using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_Blink : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color color;
    private Color colorWithoutAlpha;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;
        colorWithoutAlpha = color;
        colorWithoutAlpha.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = Color.Lerp(color, colorWithoutAlpha, Mathf.PingPong(Time.time, 1));
    }
}
