using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.Serialization;
using UnityEngine.UI;

// using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _inputHorizontal;
    private float _inputVertical;
    private Vector3 _cameraDeltaPosition;
    private Vector3 _initRot;
    private bool _jumpStart;
    private CharacterController _controller;
    private Vector3 _lastPosition;
    private float _allowOffset = .15f;

    public Transform cameraTransform;
    public PlayerCameraRigMovement cameraRigMovement;
    public OVRInput.Axis2D ovrWalkInput = OVRInput.Axis2D.PrimaryThumbstick;


    [Header("move")] public float moveSpeed;
    public float returnToCameraPower = 1;

    [Header("jump")] private float _yVelocity = 0;
    private int _remainJump;
    public float jumpPower;
    public float gravityScale;
    public int jumpStack;


    private bool _isHmd;

    private void Start()
    {
        _remainJump = jumpStack;
        _initRot = transform.eulerAngles;
        _controller = GetComponent<CharacterController>();
        _lastPosition = transform.position;

        var headset = OVRPlugin.GetSystemHeadsetType();
        _isHmd = (headset != OVRPlugin.SystemHeadset.None);

        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var resultMove = Vector3.zero;
        bool cameraPositionUpdate = true;
        transform.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0);
        print(new Vector3(0, cameraTransform.eulerAngles.y, 0));
        //_controller.
        if (_isHmd)
        {
            var axis = OVRInput.Get(ovrWalkInput, OVRInput.Controller.Touch);
            _inputHorizontal = axis.x;
            _inputVertical = axis.y;
            //print(axis);

            _cameraDeltaPosition = transform.position - cameraTransform.position;
            _cameraDeltaPosition = new Vector3(_cameraDeltaPosition.x, 0, _cameraDeltaPosition.z);
            _cameraDeltaPosition = transform.TransformDirection(_cameraDeltaPosition);
            var distance = new Vector2(_cameraDeltaPosition.x, _cameraDeltaPosition.z).magnitude;
            
            
            if (distance > _allowOffset)
            {
                //인풋벡터, _cameraDeltaPosition 내적이 -일때
                Vector2 input = new Vector2(_inputHorizontal, _inputVertical);
                Vector2 camDelta = new Vector2(_cameraDeltaPosition.x, _cameraDeltaPosition.z);
                var dot = Vector2.Dot(input, camDelta);
                
                print(dot);
                
                cameraPositionUpdate = false;
                resultMove += Move(_cameraDeltaPosition.x, _cameraDeltaPosition.z, returnToCameraPower);
                
                //인풋벡터, _cameraDeltaPosition 내적이 +일때
            }

            UpdateCapsuleHeight();
        }
        else
        {
            _inputHorizontal = Input.GetAxis("Horizontal");
            _inputVertical = Input.GetAxis("Vertical");
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        if (_controller.collisionFlags == CollisionFlags.Above || _controller.collisionFlags == CollisionFlags.Below)
        {
            _yVelocity = 0;
        }

        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     _teleportArc.DrawStart();
        // }
        //
        // if (Input.GetKeyUp(KeyCode.Alpha1))
        // {
        //     var destination = _teleportArc.DrawEnd();
        //     PlayerTeleport(destination);
        //     return;
        // }

        if(cameraPositionUpdate)
        resultMove += Move(_inputHorizontal, _inputVertical, moveSpeed);

        //resultMove += 
        Gravity();

        if (_jumpStart)
        {
            _jumpStart = false;
            _yVelocity = jumpPower * .01f;
        }

        resultMove += new Vector3(0, _yVelocity, 0);

        _controller.Move(resultMove);


        if (_inputHorizontal != 0 & _inputVertical != 0)
        {
            var deltaPos = transform.position - _lastPosition;
            cameraRigMovement.UpdateCameraPosition(deltaPos, GetCapsuleBottom());
        }

        _lastPosition = transform.position;
    }


    public void Jump()
    {
        if (_controller.collisionFlags == CollisionFlags.Below || _controller.isGrounded)
        {
            _remainJump = jumpStack;
            _yVelocity = 0;
        }

        if (_remainJump > 0)
        {
            _remainJump--;
            _jumpStart = true;
        }
    }

    private void UpdateCapsuleHeight()
    {
        var capsuleBottom = GetCapsuleBottom();
        var cameraHeight = cameraTransform.position.y;
        var newHeight = cameraHeight - capsuleBottom;

        _controller.height = newHeight;
        //_controller.height = 
    }

    private float GetCapsuleBottom()
    {
        var capsuleBottom = transform.position.y - (_controller.height * 0.5f);
        return capsuleBottom;
    }
    

    // public void DrawTeleportArc()
    // {
    //     _teleportArc.DrawStart();
    // }
    //
    // public void DrawTeleportArc(Transform dir)
    // {
    //     _teleportArc.launchTransform = dir;
    //     _teleportArc.DrawStart();
    //     
    // }

    public void ExcuteTeleport(Vector3 destination)
    {
        //PlayerTeleport(destination);
    }

    private Vector3 Move(float horizontalInput, float verticalInput, float speed)
    {
        var deltaTime = Time.deltaTime;
        var dir = new Vector3(horizontalInput, 0, verticalInput);
        if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }

        var move = dir * (speed * Time.deltaTime);
        move = transform.TransformDirection(move);

        return move;
    }

    private void Gravity()
    {
        _yVelocity -= gravityScale * Time.deltaTime;
    }

    public void RotateX(float rotX)
    {
        transform.eulerAngles = _initRot + new Vector3(0, rotX, 0);
    }

    private bool isTeleport = false;

    private void PlayerTeleport(Vector3 destination)
    {
        StartCoroutine(CameraFade(destination));
    }

    IEnumerator CameraFade(Vector3 destination)
    {
        if (isTeleport) yield break;

        isTeleport = true;
        var time = .2f;
        //var targetAlpha = 1;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            var newAlpha = i / time;
            //_cameraFadeMaterial.color = new Color(0, 0, 0, newAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = destination + Vector3.up;


        for (float i = 0; i < time; i += Time.deltaTime)
        {
            var newAlpha = 1 - i / time;
            //_cameraFadeMaterial.color = new Color(0, 0, 0, newAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isTeleport = false;
    }
}