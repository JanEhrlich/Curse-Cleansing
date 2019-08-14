using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUtility
{
    //Tmp Variables used for Calculations
    Vector2 pos;
    RaycastHit2D hit;
    Color color;

    /*
     * Shoots a Raycast from a given position with a given offset.
     * If it hits any layer of the layermaks within the given length, then it will return a hit which is true
     */
    public RaycastHit2D Raycast(Vector2 position, Vector2 offset, Vector2 rayDirection, float length, int layerMask, bool drawDebugRaycasts)
    {
        //Record the player's position
        pos = position;

        //Send out the desired raycasr and record the result
        hit = Physics2D.Raycast(pos + offset, rayDirection, length, layerMask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }
}
