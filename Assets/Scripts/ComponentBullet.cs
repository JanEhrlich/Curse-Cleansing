using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBullet : MonoBehaviour
{
    #region staticVariables
    public static float speedForRangePirate = 15f;               //speed of the bulled
    public static float lifetime = 7f;
    #endregion

    #region dynamicVariables
    public float timeUntilVanish;
    #endregion
}
