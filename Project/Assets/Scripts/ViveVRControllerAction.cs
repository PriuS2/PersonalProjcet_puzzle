using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; //SteamVR

public class ViveVRControllerAction : MonoBehaviour
{
    public SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;
    public SteamVR_Input_Sources anyHand = SteamVR_Input_Sources.Any;
    
    //키 입력 변수
    public SteamVR_Action_Boolean trigger = SteamVR_Actions.default_Trigger;
    public SteamVR_Action_Boolean grip = SteamVR_Actions.default_Grip;
    public SteamVR_Action_Boolean padTouch = SteamVR_Actions.default_PadTouch;
    public SteamVR_Action_Vector2 padPosition = SteamVR_Actions.default_PadPosition;


    private void Update()
    {
        if (trigger.GetStateDown(rightHand))
        {
            print("오른손 트리거 누름");
        }

        if (grip.GetStateUp(leftHand))
        {
            print("왼손그립 뗌");
        }

        if (padTouch.GetState(anyHand))
        {
            print("anyHand padTouch");

            var pos = padPosition.GetAxis(anyHand);
            print("패드 위치 : " +  pos);
        }
        
        
    }
}
