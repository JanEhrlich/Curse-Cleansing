using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemKrakenMarker : MonoBehaviour
{
    SystemGameMaster gameMaster;
    ComponentKrakenMarker componentMarker;
    ComponentMainCharacterState state;

    //Tmp Variables
    Vector2 player_pos;
    int facingDirection;
    GameObject closestMarker;
    float closestDistanceSquared;
    Vector2 tmpDirectionVector;
    float tmpDistanceSquared;

    public void Init(SystemGameMaster gm)
    {
        gameMaster = gm;
        componentMarker = gameMaster.ComponentKrakenMarker;
        state = gameMaster.ComponentMainCharacterState;
    }

    public void Tick()
    {
        CalculateClosestMarker();
        ActivateClosestMarker();
        DeactivateIfNoMarker();
        ResetTmpVariablesForNextCycle();
    }

    public void AddMarker(GameObject marker)
    {
        componentMarker.AddMarker(marker);      
    }

    public void RemoveMarker(GameObject marker)
    {
        componentMarker.RemoveMarker(marker);
    }

    private void CalculateClosestMarker()
    {
        player_pos = gameMaster.mainCharacterGameObject.transform.position;
        facingDirection = state.direction;

        foreach (GameObject marker in componentMarker.krakenMarkers)
        {
            if (facingDirection > 0)
            {
                // if (marker.transform.position.x < player_pos.x - 0.5f || marker.transform.position.y < player_pos.y - 0.5f)
                if (marker.transform.position.x < player_pos.x - 0.6f)
                {
                    continue;
                }
                else
                {
                    CheckDistances(player_pos,marker);
                }
            }
            else
            {
                //if (marker.transform.position.x > player_pos.x + 1f || marker.transform.position.y < player_pos.y - 0.5f)
                if (marker.transform.position.x > player_pos.x + 0.5f)
                {
                    continue;
                }
                else
                {
                    CheckDistances(player_pos,marker);
                }
            }
        }

        componentMarker.closestMarkerInRange = closestMarker;
    }

    /*
     * This function is calculating the distance the distance from the player to a marker GameObject
     * The fuuntion is written efficiently by not using the time consuming squarteroot function for distance calculation
     */
    private void CheckDistances(Vector2 player_pos, GameObject marker)
    {
        tmpDirectionVector = player_pos - (Vector2) marker.transform.position;
        tmpDistanceSquared = tmpDirectionVector.sqrMagnitude;

        if (tmpDistanceSquared < componentMarker.distanceThreshold * componentMarker.distanceThreshold)
        {
            if (tmpDistanceSquared < closestDistanceSquared)
            {
                closestDistanceSquared = tmpDistanceSquared;
                closestMarker = marker;
            }
        }
    }

    private void ResetTmpVariablesForNextCycle()
    {
        closestMarker = null;
        closestDistanceSquared = float.MaxValue;
        componentMarker.previousFrameClosestMarkerInRange = componentMarker.closestMarkerInRange;
    }


    private void ActivateClosestMarker()
    {
        if (componentMarker.closestMarkerInRange != null)
        {
            if (componentMarker.previousFrameClosestMarkerInRange == componentMarker.closestMarkerInRange)
            {
                return;
            }

            if (componentMarker.previousFrameClosestMarkerInRange == null)
            {
                componentMarker.closestMarkerInRange.GetComponent<KrakenMarker>().MarkAsActive();
                return;
            }

            if (componentMarker.previousFrameClosestMarkerInRange != componentMarker.closestMarkerInRange)
            {
                componentMarker.previousFrameClosestMarkerInRange.GetComponent<KrakenMarker>().MarkAsPassive();
                componentMarker.closestMarkerInRange.GetComponent<KrakenMarker>().MarkAsActive(); ;
            }
        }
    }

    private void DeactivateIfNoMarker()
    {
        if (componentMarker.closestMarkerInRange == null  && componentMarker.previousFrameClosestMarkerInRange != null)
        {
            componentMarker.previousFrameClosestMarkerInRange.GetComponent<KrakenMarker>().MarkAsPassive();
        }
    }
}
