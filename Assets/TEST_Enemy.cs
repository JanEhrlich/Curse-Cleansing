using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Enemy : MonoBehaviour
{
    private SystemGameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<SystemGameMaster>();
        gm.ComponentMainCharacterAction.hasSword = true;
        gm.ComponentMainCharacterAction.hasKraken = true;
        gm.ComponentMainCharacterAction.hasBat = true;
    }
}
