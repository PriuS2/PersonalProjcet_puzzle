using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    private OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    
    private OVRInput.Button triggerClick = OVRInput.Button.PrimaryIndexTrigger;
    private OVRInput.Touch triggerTouch = OVRInput.Touch.PrimaryIndexTrigger;
    private OVRInput.Axis1D triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
    
    
    private OVRInput.Button grab = OVRInput.Button.PrimaryHandTrigger;
    private OVRInput.Axis1D grabAxis = OVRInput.Axis1D.PrimaryHandTrigger;
    
    
    private OVRInput.Button thumbClick = OVRInput.Button.SecondaryThumbstick;
    private OVRInput.Touch thumbTouch = OVRInput.Touch.PrimaryThumbstick;
    private OVRInput.Axis2D thumbAxis = OVRInput.Axis2D.PrimaryThumbstick;

    private OVRInput.Button buttonAXClick = OVRInput.Button.One;
    private OVRInput.Touch buttonAXTouch = OVRInput.Touch.One;
    
    private OVRInput.Button buttonBY = OVRInput.Button.Two;
    private OVRInput.Touch buttonBYTouch = OVRInput.Touch.Two;
    
    private void Update()
    {
        OVRInput.Get(triggerClick, leftController);
    }
}
