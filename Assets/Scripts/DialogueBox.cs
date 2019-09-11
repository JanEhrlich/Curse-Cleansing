using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueBox : MonoBehaviour
{
    public bool canRepeat;
    public Notifications notification;

    public List<string> dialogueTexts;
    public UnityEvent callAfterLastDialogue;

    private TextMeshPro textBox;
    private int nextTextIndex = 0;
    private bool calledAfterLastDialogue;
    private GameObject parent;

    public void Start()
    {
        textBox = GetComponent<TextMeshPro>();
        parent = transform.parent.gameObject;
        NextDialogueText();
        DisableBox();
    }

    public void NextDialogueText()
    {
        if (textBox.enabled)
        {
            if (nextTextIndex < dialogueTexts.Count)
            {
                textBox.text = dialogueTexts[nextTextIndex];
                nextTextIndex++;
            }
            else
            {
                if (callAfterLastDialogue != null && !calledAfterLastDialogue)
                {
                    callAfterLastDialogue.Invoke();
                    calledAfterLastDialogue = true;
                }

                if (canRepeat)
                {
                    nextTextIndex = 0;
                    textBox.text = dialogueTexts[nextTextIndex];
                    nextTextIndex++;
                    DisableBox();
                    notification.DeactivateAll();
                }
                else
                {
                    if (parent.GetComponent<NextDialogueBox>() == null)
                    {
                        Destroy(parent);
                    }
                    else
                    {
                        DisableBox();
                        notification.DeactivateAll();
                    }
                }
            }
        }
    }

    public void EnableBox()
    {
        textBox.enabled = true;
        foreach (Transform child in transform)
        {
            //child is your child transform
            child.gameObject.SetActive(true);
        }
    }

    public void DisableBox()
    {
        textBox.enabled = false;
        foreach (Transform child in transform)
        {
            //child is your child transform
            child.gameObject.SetActive(false);
        }
    }
}
