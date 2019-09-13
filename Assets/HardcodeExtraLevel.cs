using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodeExtraLevel : MonoBehaviour
{
    private bool hasSword = true;
    private bool hasKraken = true;
    private bool hasBat = true;

    private SystemGameMaster gm;

    // Start is called before the first frame update
    void Update()
    {
        gm = GetComponent<SystemGameMaster>();
        gm.ComponentMainCharacterAction.hasSword = hasSword;
        gm.ComponentMainCharacterAction.hasKraken = hasKraken;
        gm.ComponentMainCharacterAction.hasBat = hasBat;
    }
}
