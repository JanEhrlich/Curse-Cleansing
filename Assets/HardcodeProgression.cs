using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodeProgression : MonoBehaviour
{
    private bool hasSword = true;
    private bool hasKraken = true;

    private SystemGameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<SystemGameMaster>();
        gm.ComponentMainCharacterAction.hasSword = hasSword;
        gm.ComponentMainCharacterAction.hasKraken = hasKraken;
    }
}
