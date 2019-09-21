using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodeProgression : MonoBehaviour
{
    public bool hasSword = true;
    public bool hasKraken = true;
    public bool hasBat = false;

    private SystemGameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<SystemGameMaster>();
        gm.ComponentMainCharacterAction.hasSword = hasSword;
        gm.ComponentMainCharacterAction.hasKraken = hasKraken;
        gm.ComponentMainCharacterAction.hasBat = hasBat;
    }
}
