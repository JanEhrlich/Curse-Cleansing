using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentInput
{
    /*
     * The precise position of the Joystick in the x Axis. 
     * 1 = Right
     * 0 = Middle/Neural
     * -1 = Left
     */
    private float currentHorizontalJoystickPosition;

    /*
     * The precise position of the Joystick in the y Axis. 
     * 1 = Up
     * 0 = Middle/Neural
     * -1 = Down
     */
    private float currentVerticalJoystickPosition;

    /*
     * A Vector calculated by the vertical and horizontal Position of the Joystick.
     * Those are Raw Values
     */
    private Vector2 currentJoystickDirection;

    /*
     * A Vector calculated by the vertical and horizontal Position of the Joystick.
     * The x and y are clamped ==> x and y can only be 1,0 or -1
     */
    private Vector2 currentJoystickDirectionClamped;

    /*
     * A Vector calculated by the vertical and horizontal Position of the Joystick.
     * The x and y are clamped ==> x and y can only be 1,0 or -1
     * Also only x or y can be different from 0 at any given time ==> Only one active direction
     * Vector2(1,0) = Right
     * Vector2(-1,0) = Left
     * Vector2(0,1) = Up
     * Vector2(0,-1) = Down
     * Vector2(0,0) = Middle/Neutral
     */
    private Vector2 currentJoystickAxis;

    private List<Action> callOnJumpButtonPress = new List<Action>();
    private List<Action> callOnJumpButtonCancel = new List<Action>();

    private List<Action> callOnAttackButtonPress = new List<Action>();
    private List<Action> callOnAttackButtonCancel = new List<Action>();

    private List<Action> callOnTentacleButtonPress = new List<Action>();
    private List<Action> callOnTentacleButtonCancel = new List<Action>();

    private List<Action> callOnDashButtonPress = new List<Action>();
    private List<Action> callOnDashButtonCancel = new List<Action>();

    private List<Action> callOnPauseButtonPress = new List<Action>();
    private List<Action> callOnPauseButtonCancel = new List<Action>();

    private List<Action> callOnBatAttackButtonPress = new List<Action>();
    private List<Action> callOnBatAttackButtonCancel = new List<Action>();

    private List<Action> callOnInteractButtonPress = new List<Action>();
    private List<Action> callOnInteractButtonCancel = new List<Action>();

    private List<Action> callOnQuickTransformKrakenButtonPress = new List<Action>();
    private List<Action> callOnQuickTransformKrakenButtonCancel = new List<Action>();

    private List<Action> callOnQuickTransformBatButtonPress = new List<Action>();
    private List<Action> callOnQuickTransformBatButtonCancel = new List<Action>();

    private List<Action> callOnQuickTransformGhostButtonPress = new List<Action>();
    private List<Action> callOnQuickTransformGhostButtonCancel = new List<Action>();

    private List<Action> callOnQuickTransformWolfButtonPress = new List<Action>();
    private List<Action> callOnQuickTransformWolfButtonCancel = new List<Action>();

    //Call these to Add a Function to be called when a button was pressed or canceled
    #region AddFunctionsToLists

    public void AddJumpButtonPressFunction(Action function)
    {
        callOnJumpButtonPress.Add(function);
    }

    public void AddJumpButtonCancelFunction(Action function)
    {
        callOnJumpButtonCancel.Add(function);
    }

    public void AddAttackButtonPressFunction(Action function)
    {
        callOnAttackButtonPress.Add(function);
    }

    public void AddAttackButtonCancelFunction(Action function)
    {
        callOnAttackButtonCancel.Add(function);
    }

    public void AddTentacleButtonPressFunction(Action function)
    {
        callOnTentacleButtonPress.Add(function);
    }

    public void AddTentacleButtonCancelFunction(Action function)
    {
        callOnTentacleButtonCancel.Add(function);
    }

    public void AddDashButtonPressFunction(Action function)
    {
        callOnDashButtonPress.Add(function);
    }

    public void AddDashButtonCancelFunction(Action function)
    {
        callOnDashButtonCancel.Add(function);
    }

    public void AddPauseButtonPressFunction(Action function)
    {
        callOnPauseButtonPress.Add(function);
    }

    public void AddPauseButtonCancelFunction(Action function)
    {
        callOnPauseButtonCancel.Add(function);
    }

    public void AddBatAttackButtonPressFunction(Action function)
    {
        callOnBatAttackButtonPress.Add(function);
    }

    public void AddBatAttackButtonCancelFunction(Action function)
    {
        callOnBatAttackButtonCancel.Add(function);
    }

    public void AddInteractButtonPressFunction(Action function)
    {
        callOnInteractButtonPress.Add(function);
    }

    public void AddInteractButtonCancelFunction(Action function)
    {
        callOnInteractButtonCancel.Add(function);
    }

    public void AddQuickTransformKrakenButtonPressFunction(Action function)
    {
        callOnQuickTransformKrakenButtonPress.Add(function);
    }

    public void AddQuickTransformKrakenButtonCancelFunction(Action function)
    {
        callOnQuickTransformKrakenButtonCancel.Add(function);
    }

    public void AddQuickTransformBatButtonPressFunction(Action function)
    {
        callOnQuickTransformBatButtonPress.Add(function);
    }

    public void AddQuickTransformBatButtonCancelFunction(Action function)
    {
        callOnQuickTransformBatButtonCancel.Add(function);
    }

    public void AddQuickTransformGhostButtonPressFunction(Action function)
    {
        callOnQuickTransformGhostButtonPress.Add(function);
    }

    public void AddQuickTransformGhostButtonCancelFunction(Action function)
    {
        callOnQuickTransformGhostButtonCancel.Add(function);
    }

    public void AddQuickTransformWolfButtonPressFunction(Action function)
    {
        callOnQuickTransformWolfButtonPress.Add(function);
    }

    public void AddQuickTransformWolfButtonCancelFunction(Action function)
    {
        callOnQuickTransformWolfButtonCancel.Add(function);
    }
    #endregion

    //Getter for the SystemInteraction Class
    #region GetterForPressAndCancelActions

    public List<Action> getCallOnJumpButtonPress()
    {
        return callOnJumpButtonPress;
    }

    public List<Action> getCallOnJumpButtonCancel()
    {
        return callOnJumpButtonCancel;
    }

    public List<Action> getCallOnAttackButtonPress()
    {
        return callOnAttackButtonPress;
    }

    public List<Action> getCallOnAttackButtonCancel()
    {
        return callOnAttackButtonCancel;
    }

    public List<Action> getCallOnTentacleButtonPress()
    {
        return callOnTentacleButtonPress;
    }

    public List<Action> getCallOnTentacleButtonCancel()
    {
        return callOnTentacleButtonCancel;
    }

    public List<Action> getCallOnDashButtonPress()
    {
        return callOnDashButtonPress;
    }

    public List<Action> getCallOnDashButtonCancel()
    {
        return callOnDashButtonCancel;
    }

    public List<Action> getCallOnPauseButtonPress()
    {
        return callOnPauseButtonPress;
    }

    public List<Action> getCallOnPauseButtonCancel()
    {
        return callOnPauseButtonCancel;
    }

    public List<Action> getCallOnBatAttackButtonPress()
    {
        return callOnBatAttackButtonPress;
    }

    public List<Action> getCallOnBatAttackButtonCancel()
    {
        return callOnBatAttackButtonCancel;
    }

    public List<Action> getCallOnInteractButtonPress()
    {
        return callOnInteractButtonPress;
    }

    public List<Action> getCallOnInteractButtonCancel()
    {
        return callOnInteractButtonCancel;
    }

    public List<Action> getCallOnQuickTransformKrakenButtonPress()
    {
        return callOnQuickTransformKrakenButtonPress;
    }

    public List<Action> getCallOnQuickTransformKrakenButtonCancel()
    {
        return callOnQuickTransformKrakenButtonCancel;
    }

    public List<Action> getCallOnQuickTransformBatButtonPress()
    {
        return callOnQuickTransformBatButtonPress;
    }

    public List<Action> getCallOnQuickTransformBatButtonCancel()
    {
        return callOnQuickTransformBatButtonCancel;
    }

    public List<Action> getCallOnQuickTransformGhostButtonPress()
    {
        return callOnQuickTransformGhostButtonPress;
    }

    public List<Action> getCallOnQuickTransformGhostButtonCancel()
    {
        return callOnQuickTransformGhostButtonCancel;
    }

    public List<Action> getCallOnQuickTransformWolfButtonPress()
    {
        return callOnQuickTransformWolfButtonPress;
    }

    public List<Action> getCallOnQuickTransformWolfButtonCancel()
    {
        return callOnQuickTransformWolfButtonCancel;
    }
    #endregion

    /*
     * Getter for any Class which needs to know the current Joystick Position.
     * Setter for the SystemInteraction Class to set new Values
     */
    #region GetterAndSetterForJoystickVariables

    /// <summary>
    /// The precise position of the Joystick in the x Axis. 
    /// </summary>
    public float getCurrentHorizontalJoystickPosition()
    {
        return currentHorizontalJoystickPosition;
    }

    public void setCurrentHorizontalJoystickPosition(float x)
    {
        currentHorizontalJoystickPosition = x;
    }

    /// <summary>
    /// The precise position of the Joystick in the y Axis. 
    /// </summary>
    public float getCurrentVerticalJoystickPosition()
    {
        return currentVerticalJoystickPosition;
    }

    public void setCurrentVerticalJoystickPosition(float y)
    {
        currentVerticalJoystickPosition = y;
    }

    /// <summary>
    /// A Vector calculated by the vertical and horizontal Position of the Joystick.
    /// Those are Raw Values
    /// </summary>
    public Vector2 getCurrentJoystickDirection()
    {
        return currentJoystickDirection;
    }

    public void setCurrentJoystickDirection(float x,float y)
    {
        currentJoystickDirection = new Vector2(x,y);
    }

    /// <summary>
    /// A Vector calculated by the vertical and horizontal Position of the Joystick.
    /// The x and y are clamped ==> x and y can only be 1,0 or -1
    /// </summary>
    public Vector2 getCurrentJoystickDirectionClamped()
    {
        return currentJoystickDirectionClamped;
    }

    public void setCurrentJoystickDirectionClamped(float x, float y)
    {
        if ((x < 1 && x > 0) || (x < 0 && x > -1) || (y < 1 && y > 0) || (y < 0 && y > -1))
        {
            Debug.Log("Unclamped value detected in setCurrentJoystickDirectionClamped");
        }
        currentJoystickDirectionClamped = new Vector2(x, y);
    }

    /// <summary>
    /// A Vector calculated by the vertical and horizontal Position of the Joystick.
    /// The x and y are clamped ==> x and y can only be 1,0 or -1
    /// Also only x or y can be different from 0 at any given time ==> Only one active direction
    /// </summary>
    public Vector2 getCurrentJoystickAxis()
    {
        return currentJoystickAxis;
    }

    public void setCurrentJoystickAxis(Vector2 input)
    {
        float x = input.x;
        float y = input.y;
        if ((x < 1 && x > 0) || (x < 0 && x > -1) || (y < 1 && y > 0) || (y < 0 && y > -1))
        {
            Debug.Log("Unclamped value detected in setCurrentJoystickAxis");

            if (x != 0 && y != 0)
            {
                Debug.Log("Undefined Axis detected in setCurrentJoystickAxis");
            }
        }
        currentJoystickAxis = new Vector2(x, y);
    }
    #endregion
}