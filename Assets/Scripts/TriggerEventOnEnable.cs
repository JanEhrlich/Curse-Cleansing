using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnEnable : MonoBehaviour
{

    public UnityEvent callAfterLastDialogue;

    private void OnEnable()
    {
        if (callAfterLastDialogue != null)
        {
            callAfterLastDialogue.Invoke();
        }
    }
}
