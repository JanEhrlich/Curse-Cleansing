using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDialogueBox : MonoBehaviour
{
    public GameObject nextDialogue;

    private bool markForDestruction = false;
    private bool leftBox = false;

    public void Update()
    {
        if (markForDestruction && leftBox)
        {
            nextDialogue.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    public void MarkForDestruction()
    {
        markForDestruction = true;
    }

    public void EnteredBox()
    {
        leftBox = false;
    }

    public void LeftBox()
    {
        leftBox = true;
    }
}
