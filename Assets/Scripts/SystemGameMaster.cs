﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class is the main entry point,
 * also used to handle resume after pausing a game
 * 
 * TODO:
 *  -handle all logic
 *  -call all classes
 */

public class SystemGameMaster : MonoBehaviour
{
    private ComponentMainCharacterState componentMainCharacterState;
    void Tick()
    {
        
    }

    void FixedTick()
    {
        
    }

    public ComponentMainCharacterState MainCharacterState { get { return componentMainCharacterState; } }
}
