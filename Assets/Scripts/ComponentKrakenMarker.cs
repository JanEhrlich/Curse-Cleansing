using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentKrakenMarker
{
    /*
     * Hardcoded Variables which shape the games behaviour
     */
    #region StaticVariables

    //The Max distance to a Marker to mark it as active
    public float distanceThreshold = 3;

    #endregion

    /*
     * Dynamic Variables which will change very often depending on the game state and player's actions
     */
    #region DynamictVariables
    public GameObject closestMarkerInRange;
    public List<GameObject> krakenMarkers = new List<GameObject>();

    public GameObject previousFrameClosestMarkerInRange;
    #endregion

    #region Functions

    public void AddMarker (GameObject marker)
    {
        krakenMarkers.Add(marker);
    }

    public void ClearMarker()
    {
        krakenMarkers.Clear();
    }

    public void RemoveMarker(GameObject marker)
    {
        krakenMarkers.Remove(marker);
    }

    #endregion
}
