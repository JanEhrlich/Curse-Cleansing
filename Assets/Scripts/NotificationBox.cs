using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationBox : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float displayDuration = 5;
    public bool displayOnLevelStart = false;
    public List<string> notificationTexts;

    private Animator anim;
    private float waitLevelStart = 0.5F;

    private float timer;
    private float onLevelStartTimer;

    private bool isShowing;
    private bool shownOnLevelStart = false;

    private int popup = Animator.StringToHash("Popup");
    private int popdown = Animator.StringToHash("Popdown");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing)
        {
            if (timer < displayDuration)
            {
                timer += Time.deltaTime;
            }
            else
            {
                HideNotification();
            }
        }

        if (displayOnLevelStart && !shownOnLevelStart)
        {
            if (onLevelStartTimer < waitLevelStart)
            {
                onLevelStartTimer += Time.deltaTime;
            }
            else
            {
                ShowNotification(0);
                shownOnLevelStart = true;
            }
        }
    }

    private void HideNotification()
    {
        anim.Play(popdown);
        isShowing = false;
    }

    public void ShowNotification(int textNumber)
    {
        if (textNumber >= 0 && textNumber < notificationTexts.Count)
        {
            textMeshPro.text = notificationTexts[textNumber];
        }
        else
        {
            Debug.Log("Notification Box Text Number out of Range");
        }

        anim.Play(popup);
        timer = 0;
        isShowing = true;
    }
}
