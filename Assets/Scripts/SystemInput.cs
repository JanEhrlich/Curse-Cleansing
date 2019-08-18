using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this class handles the input from the user
 * 
 * TODO:
 *  -store input in the input component
 */

public class SystemInput : MonoBehaviour
{
    //Set distribution of Curses in Unity Editor possible with this.
    public enum Curse {Kraken, Bat, Ghost, Wolf, EmptySlot};
    public Curse dPadLeftCurse;
    public Curse dPadRightCurse;
    public Curse dPadUpCurse;
    public Curse dPadDownCurse;

    //Handle to ComponentInput
    ComponentInput componentInput;

    //Handle to SystemGameMaster
    SystemGameMaster systemGameMaster;

    //Variables Used to to identify the Type of the connected Controller
    int fingerprintXboxOne = 33;
    int fingerprintXbox360Wireless = 51;
    int fingerptintSwitchPro = 16;
    int fingerprintOfLastState = -1;
    string[] controllerNames;

    //Tmp Variables to calculate Joystick and DPad Position 
    float tmp_x,tmp_y;
    Vector2 tmp_axis;

    //Tmp Variables to calculate DPad presses and cancels
    bool left, right, up, down;
    bool oldLeft, oldRight, oldUp, oldDown;

    //Tmp Variables to calculate Trigger presses and cancels on Xbox Controllers
    bool xboxTriggerPressed;
    bool oldXboxTriggerPressed;

    #region MappingStrings

    //Actually used Strings in the code. The Content will switch depending on Connected Status. Initial Mapping is Switch Controller
    string moveHorizontalString;
    string moveVerticalString;
    string quickTransformHorizontalString;
    string quickTransformVerticalString;
    string dashString;
    string dashStringXboxTrigger2;
    string pauseString;
    string batAttackString;
    string interactString;
    string jumpString;
    string attackString;
    string tentacleString;

    //Input Mapping Names for quick changes on Naming [Nintendo Switch Pro Controller]
    string moveHorizontalStringPrefix = "MoveHorizontal";
    string moveVerticalStringPrefix = "MoveVertical";
    string quickTransformHorizontalStringPrefix = "QuickTransformHorizontal";
    string quickTransformVerticalStringPrefix = "QuickTransformVertical";
    string dashStringPrefix = "Dash";
    string dashStringPrefixXbox1 = "DashTriggerRight";
    string dashStringPrefixXbox2 = "DashTriggerLeft";
    string pauseStringPrefix = "Pause";
    string batAttackStringPrefix = "BatAttack";
    string interactStringPrefix = "Interact";
    string jumpStringPrefix = "Jump";
    string attackStringPrefix = "Attack";
    string tentacleStringPrefix = "Tentacle";

    string switchProSuffix = "Switch";
    string xboxSuffix = "Xbox";
    #endregion

    public void Init(SystemGameMaster gameMaster)
    {       
        systemGameMaster = gameMaster;
        componentInput = systemGameMaster.ComponentInput;

        CheckForControllerChange();

        #region Debugging
        //Uncomment for debugging
        /*
        componentInput.AddAttackButtonPressFunction(() => Debug.Log("Attack Pressed"));
        componentInput.AddAttackButtonCancelFunction(() => Debug.Log("Attack Cancel"));
        componentInput.AddJumpButtonPressFunction(() => Debug.Log("jump Pressed"));
        componentInput.AddJumpButtonCancelFunction(() => Debug.Log("jump Cancel"));
        componentInput.AddTentacleButtonPressFunction(() => Debug.Log("ten Pressed"));
        componentInput.AddTentacleButtonCancelFunction(() => Debug.Log("ten Cancel"));
        componentInput.AddDashButtonPressFunction(() => Debug.Log("dash Pressed"));
        componentInput.AddDashButtonCancelFunction(() => Debug.Log("dash Cancel"));
        componentInput.AddPauseButtonPressFunction(() => Debug.Log("pau Pressed"));
        componentInput.AddPauseButtonCancelFunction(() => Debug.Log("pau Cancel"));
        componentInput.AddBatAttackButtonPressFunction(() => Debug.Log("bat Pressed"));
        componentInput.AddBatAttackButtonCancelFunction(() => Debug.Log("bat Cancel"));
        componentInput.AddInteractButtonPressFunction(() => Debug.Log("int Pressed"));
        componentInput.AddInteractButtonCancelFunction(() => Debug.Log("int Cancel"));
        componentInput.AddQuickTransformKrakenButtonPressFunction(() => Debug.Log("kraken Pressed"));
        componentInput.AddQuickTransformKrakenButtonCancelFunction(() => Debug.Log("kraken Cancel"));
        componentInput.AddQuickTransformBatButtonPressFunction(() => Debug.Log("battrans Pressed"));
        componentInput.AddQuickTransformBatButtonCancelFunction(() => Debug.Log("battrans Cancel"));
        componentInput.AddQuickTransformGhostButtonPressFunction(() => Debug.Log("ghost Pressed"));
        componentInput.AddQuickTransformGhostButtonCancelFunction(() => Debug.Log("ghost Cancel"));
        componentInput.AddQuickTransformWolfButtonPressFunction(() => Debug.Log("wolf Pressed"));
        componentInput.AddQuickTransformWolfButtonCancelFunction(() => Debug.Log("wolf Cancel"));*/
        #endregion
    }

    public void Tick()
    {
        /*
        Debug.Log("Controller Connected: " + componentInput.isControllerConnected());
        Debug.Log("Controller Type: " + componentInput.getCurrentControllerType());
        Debug.Log("Controller Slot: " + componentInput.getControllerSlot());*/

        CheckForControllerChange();

        if (componentInput.isControllerConnected())
        {
            UpdateJoystickValues();

            UpdateDPadState();

            CheckButtonDownsAndUps();

            if (componentInput.getCurrentControllerType() == ComponentInput.ControllerType.Xbox)
            {
                HandelXboxTriggerButtons();
            }
        }
    }

    public void FixedTick()
    {

    }

    //Sets all relevant Values for Movement Directions in ComponentInput
    void UpdateJoystickValues()
    {
        tmp_x = Input.GetAxisRaw(moveHorizontalString);
        tmp_y = Input.GetAxisRaw(moveVerticalString);

        componentInput.setCurrentHorizontalJoystickPosition(tmp_x);
        componentInput.setCurrentVerticalJoystickPosition(tmp_y);

        componentInput.setCurrentJoystickDirection(tmp_x, tmp_y);

        componentInput.setCurrentJoystickDirectionClamped(ClampValue(tmp_x), ClampValue(tmp_y));

        tmp_axis = CalculateActiveAxis(tmp_x, tmp_y);
        componentInput.setCurrentJoystickAxis(tmp_axis);
    }

    //Clamping to 1,0 or -1
    private int ClampValue(float value)
    {
        if (value > 0)
        {
            return 1;
        }

        if (value < 0)
        {
            return -1;
        }

        return 0;
    }

    //Only sets one value of the Vector to 1 or -1 or it sets both to 0
    private Vector2 CalculateActiveAxis(float x, float y)
    {
        if (x * x >= y * y)
        {
            return new Vector2(ClampValue(x), 0);
        }
        else
        {
            return new Vector2(0, ClampValue(y));
        }

    }

    //Calculating which DPad Button was pressed or canceled and call triggers
    private void UpdateDPadState()
    {
        tmp_x = Input.GetAxis(quickTransformHorizontalString);
        tmp_y = Input.GetAxis(quickTransformVerticalString);

        tmp_axis = CalculateActiveAxis(tmp_x, tmp_y);
        MapAxisToDirection(tmp_axis);
        DetectDPadTriggerAndCancels();
    }

    //Setting the Bools for every possible Direction
    private void MapAxisToDirection(Vector2 axis)
    {
        right = axis.x == 1;
        left = axis.x == -1;     
        up = axis.y == 1;
        down = axis.y == -1;
    }

    /*
     * Very ugly function which basically checks the binding of curse to each direction 
     * and calls the appropriate press and cancel functions
     */
    private void DetectDPadTriggerAndCancels()
    {
        #region CheckDPadRightMapping

        if (right != oldRight && dPadRightCurse != Curse.EmptySlot)
        {
            if (right)
            {
                switch (dPadRightCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonPress())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonPress())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonPress())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonPress())
                            func();
                        break;
                }
            }
            else
            {
                switch (dPadRightCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonCancel())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonCancel())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonCancel())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonCancel())
                            func();
                        break;
                }
            }
        }
        #endregion

        #region CheckDPadLeftMapping

        if (left != oldLeft && dPadLeftCurse != Curse.EmptySlot)
        {
            if (left)
            {
                switch (dPadLeftCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonPress())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonPress())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonPress())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonPress())
                            func();
                        break;
                }
            }
            else
            {
                switch (dPadLeftCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonCancel())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonCancel())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonCancel())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonCancel())
                            func();
                        break;
                }
            }
        }
        #endregion

        #region CheckDPadUpMapping

        if (up != oldUp && dPadUpCurse != Curse.EmptySlot)
        {
            if (up)
            {
                switch (dPadUpCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonPress())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonPress())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonPress())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonPress())
                            func();
                        break;
                }
            }
            else
            {
                switch (dPadUpCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonCancel())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonCancel())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonCancel())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonCancel())
                            func();
                        break;
                }
            }
        }
        #endregion

        #region CheckDPadDownMapping

        if (down != oldDown && dPadDownCurse != Curse.EmptySlot)
        {
            if (down)
            {
                switch (dPadDownCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonPress())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonPress())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonPress())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonPress())
                            func();
                        break;
                }
            }
            else
            {
                switch (dPadDownCurse)
                {
                    case Curse.Kraken:
                        foreach (Action func in componentInput.getCallOnQuickTransformKrakenButtonCancel())
                            func();
                        break;
                    case Curse.Bat:
                        foreach (Action func in componentInput.getCallOnQuickTransformBatButtonCancel())
                            func();
                        break;
                    case Curse.Ghost:
                        foreach (Action func in componentInput.getCallOnQuickTransformGhostButtonCancel())
                            func();
                        break;
                    default:
                        foreach (Action func in componentInput.getCallOnQuickTransformWolfButtonCancel())
                            func();
                        break;
                }
            }
        }
        #endregion

        oldLeft = left;
        oldRight = right;
        oldUp = up;
        oldDown = down;
    }

    //Here the buttons calls will be processed
    private void CheckButtonDownsAndUps()
    {
        if (Input.GetButtonDown(jumpString))
        {
            foreach (Action func in componentInput.getCallOnJumpButtonPress())
                func();
        }
        if (Input.GetButtonUp(jumpString)){
            foreach (Action func in componentInput.getCallOnJumpButtonCancel())
                func();
        }

        if (Input.GetButtonDown(attackString))
        {
            foreach (Action func in componentInput.getCallOnAttackButtonPress())
                func();
        }
        if (Input.GetButtonUp(attackString))
        {
            foreach (Action func in componentInput.getCallOnAttackButtonCancel())
                func();
        }

        if (Input.GetButtonDown(tentacleString))
        {
            foreach (Action func in componentInput.getCallOnTentacleButtonPress())
                func();
        }
        if (Input.GetButtonUp(tentacleString))
        {
            foreach (Action func in componentInput.getCallOnTentacleButtonCancel())
                func();
        }

        if (Input.GetButtonDown(dashString))
        {
            foreach (Action func in componentInput.getCallOnDashButtonPress())
                func();
        }
        if (Input.GetButtonUp(dashString))
        {
            foreach (Action func in componentInput.getCallOnDashButtonCancel())
                func();
        }

        if (Input.GetButtonDown(pauseString))
        {
            foreach (Action func in componentInput.getCallOnPauseButtonPress())
                func();
        }
        if (Input.GetButtonUp(pauseString))
        {
            foreach (Action func in componentInput.getCallOnPauseButtonCancel())
                func();
        }

        if (Input.GetButtonDown(batAttackString))
        {
            foreach (Action func in componentInput.getCallOnBatAttackButtonPress())
                func();
        }
        if (Input.GetButtonUp(batAttackString))
        {
            foreach (Action func in componentInput.getCallOnBatAttackButtonCancel())
                func();
        }

        if (Input.GetButtonDown(interactString))
        {
            foreach (Action func in componentInput.getCallOnInteractButtonPress())
                func();
        }
        if (Input.GetButtonUp(interactString))
        {
            foreach (Action func in componentInput.getCallOnInteractButtonCancel())
                func();
        }
    }

    //Checks if current Controller is still connected and if not tries to reconnect to another controller
    private void CheckForControllerChange()
    {
        controllerNames = Input.GetJoystickNames();
        if (controllerNames.Length != 0 && fingerprintOfLastState != controllerNames[componentInput.getControllerSlot()].Length)
        {
            UpdateUsedController();
        }
    }

    //Checks whether a controller is connected. And if there is one, whether it is a Xbox Controller or a Switch Pro Controller
    private void UpdateUsedController()
    {
        for (int i = 0; i < controllerNames.Length; i++)
        {
            if (controllerNames[i].Length != fingerprintXboxOne && controllerNames[i].Length != fingerptintSwitchPro && controllerNames[i].Length != fingerprintXbox360Wireless)
            {
                componentInput.setControllerTypeToNone();
                componentInput.setConnectedStatus(false);
                fingerprintOfLastState = -1;
                MapInputStringsToController();
                //Debug.Log("There is no Controller connected || Slot: " + i + " of " + (controllerNames.Length-1) + " ||  String: " + controllerNames[i]);
            }
            else
            {               
                if (controllerNames[i].Length == fingerprintXboxOne || controllerNames[i].Length == fingerprintXbox360Wireless)
                {
                    componentInput.setControllerTypeToXbox();
                    fingerprintOfLastState = controllerNames[i].Length;
                    //Debug.Log("XBOX Controller connected || Slot: " + i + " of " + (controllerNames.Length - 1) + "  ||  String: " + controllerNames[i]);
                }

                if (controllerNames[i].Length == fingerptintSwitchPro)
                {
                    componentInput.setControllerTypeToSwitchPro();
                    fingerprintOfLastState = fingerptintSwitchPro;
                    //Debug.Log("SWITCH Controller connected || Slot: " + i + " of " + (controllerNames.Length - 1) + "  ||  String: " + controllerNames[i]);
                }

                componentInput.setControllerSlot(i);
                componentInput.setConnectedStatus(true);

                MapInputStringsToController();

                break;
            }
        }
    }

    //Sets the used Strings for the Input names to the specific controller
    private void MapInputStringsToController()
    {
        string suffix = componentInput.getControllerSlot().ToString();

        if (componentInput.getCurrentControllerType() == ComponentInput.ControllerType.SwitchPro)
        {
            suffix += switchProSuffix;

            dashString = dashStringPrefix;
            pauseString = pauseStringPrefix;
            //Debug.Log("Mapping2Switch SlotID = " + componentInput.getControllerSlot());
        }

        if (componentInput.getCurrentControllerType() == ComponentInput.ControllerType.Xbox)
        {
            suffix += xboxSuffix;

            dashString = dashStringPrefixXbox1 + suffix;
            dashStringXboxTrigger2 = dashStringPrefixXbox2 + suffix;
            pauseString = pauseStringPrefix + xboxSuffix;
            //Debug.Log("Mapping2Xbox SlotID = " + componentInput.getControllerSlot());
        }

        moveHorizontalString = moveHorizontalStringPrefix + suffix;
        moveVerticalString = moveVerticalStringPrefix + suffix;
        quickTransformHorizontalString = quickTransformHorizontalStringPrefix + suffix;
        quickTransformVerticalString = quickTransformVerticalStringPrefix + suffix;
        
        batAttackString = batAttackStringPrefix;
        interactString = interactStringPrefix;
        jumpString = jumpStringPrefix;
        attackString = attackStringPrefix;
        tentacleString = tentacleStringPrefix;
    }

    //Function only used for Xbox Controller because Trigger Buttons aren't buttons but axes
    private void HandelXboxTriggerButtons()
    {
        tmp_x = Input.GetAxis(dashString);
        tmp_y = Input.GetAxis(dashStringXboxTrigger2);

        xboxTriggerPressed = tmp_x > 0 || tmp_y > 0;

        if (xboxTriggerPressed != oldXboxTriggerPressed)
        {
            if (xboxTriggerPressed)
            {
                foreach (Action func in componentInput.getCallOnDashButtonPress())
                    func();
            }
            else
            {
                foreach (Action func in componentInput.getCallOnDashButtonCancel())
                    func();
            }
        }

        oldXboxTriggerPressed = xboxTriggerPressed;
    }
}
