using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRHandInput : MonoBehaviour
{
    private OVRHand _ovrHand;
    private Dictionary<int, OVRHand.HandFinger> _indexToFinger = new Dictionary<int, OVRHand.HandFinger>();
    private Vector3 _relativePosition;
    public GameObject sphereAnchor;
    public PlayerMovement PlayerMovement;
    
    
    
    public enum Action
    {
        None,
        Walk,
        Teleport
    }
    
    [System.Serializable]
    public struct FingerAction
    {
        //private Dictionary<int, Action>
        
        public Action index;
        public Action middle;
        public Action ring;
        public Action pinky;
    }

    public struct InputResult
    {
        public bool State;
        public float Horizontal;
        public float Vertical;
    }
    
    

    
    //Inspector
    public FingerAction fingerAction;
    
    [Header("#Debug")]
    [SerializeField]private bool[] _fingerPinch = new bool[5];
    [SerializeField]private float[] _fingerStrenth = new float[5];
    [SerializeField]private int _uniqueState = 0;
    //###############
    
    
    
    private Transform _transform;
    void Start()
    {
        _ovrHand = GetComponent<OVRHand>();
        _transform = transform;
        
        _indexToFinger.Clear();
        _indexToFinger.Add(0, OVRHand.HandFinger.Thumb);
        _indexToFinger.Add(1, OVRHand.HandFinger.Index);
        _indexToFinger.Add(2, OVRHand.HandFinger.Middle);
        _indexToFinger.Add(3, OVRHand.HandFinger.Ring);
        _indexToFinger.Add(4, OVRHand.HandFinger.Pinky);
    }
    
    void Update()
    {
        var confidence = _ovrHand.IsDataHighConfidence;
        if (!confidence)
        {
            //_uniqueState = 0;
            for (int i = 0; i < 5; i++)
            {
                _fingerPinch[i] = false;
                _fingerStrenth[i] = 0.0f;
            }
            return;
        }


        var lastState = _uniqueState;
        var trueNum = 0;
        var fingerIndex = 0;
        for (int i = 0; i < 5; i++)
        {
            _fingerPinch[i] = _ovrHand.GetFingerIsPinching(_indexToFinger[i]);
            _fingerStrenth[i] = _ovrHand.GetFingerPinchStrength(_indexToFinger[i]);

            if (_fingerPinch[i])
            {
                trueNum++;
                fingerIndex = i;
            }
        }

        if (trueNum == 2)
        {
            _uniqueState = fingerIndex;
        }
        else
        {
            _uniqueState = 0;
        }

        if (lastState != _uniqueState)
        {
            print("StateChanged");
            StateChanged(lastState);
        }
    }

    private void StateChanged(int last)
    {
        _relativePosition = transform.parent.localPosition;
        if (sphereAnchor) sphereAnchor.transform.position = transform.position;
        
        
        var teleport = Action.Teleport;
        var index = 0;
        if (fingerAction.index == teleport) index = 1;
        else if (fingerAction.middle == teleport) index = 2;
        else if (fingerAction.ring == teleport) index = 3;
        else if (fingerAction.pinky == teleport) index = 4;

        if (index != 0 & index == _uniqueState)
        {
            //draw teleportArc
            PlayerMovement.DrawTeleportArc(transform);
        }
        else if (last == index)
        {
            //excute teleport
            PlayerMovement.ExcuteTeleport();
            
        }
        
    }

    public InputResult GetMoveInput()
    {
        
        var walk = Action.Walk;
        var index = 0;
        if (fingerAction.index == walk) index = 1;
        else if (fingerAction.middle == walk) index = 2;
        else if (fingerAction.ring == walk) index = 3;
        else if (fingerAction.pinky == walk) index = 4;

        var result = new InputResult();
        
        if (index != 0 & index == _uniqueState)
        {
            result.State = true;
            var dir = GetMoveDirection();
            result.Vertical = Mathf.Abs(dir.x) > 0.2f ? dir.x : 0;
            result.Horizontal =  Mathf.Abs(dir.y) > 0.2f ? dir.y : 0;
            
            VRDebug.VrDebug.Print(result.Vertical + " / " + result.Horizontal);
            //print(result.Vertical + " / " + result.Horizontal);

            if (sphereAnchor) if(!sphereAnchor.active) sphereAnchor.SetActive(true);
        }
        else
        {
            result.State = false;
            if (sphereAnchor) if(sphereAnchor.active)  sphereAnchor.SetActive(false);
        }
        return result;
    }

    private Vector2 GetMoveDirection()
    {
        var deltaPosition =  _relativePosition - transform.parent.localPosition;

        //var newDelta = transform.parent.parent.InverseTransformDirection(deltaPosition);
        var result =  new Vector2(Mathf.Clamp(deltaPosition.x * 15, -1.0f, 1.0f), Mathf.Clamp(deltaPosition.z * 15, -1.0f, 1.0f));
        //print(result);
        VRDebug.VrDebug.Print(-result.x + " \n / " + -result.y);
        result *= -1;
        return result;
    }

    private Quaternion GetTeleportDirection()
    {
        return new Quaternion();
    }
}
