using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    //private Camera _mainCam;
    private bool _pickup;
    
    public Transform attachPoint;
    
    public float rayDistance;

    public float handlePower;
    public float rotatePower;
    public float detachDistance;
    public float rotateSpeed;

    private PickupCube _pickupCube;
    private PickupCube _rayhitCube;

    //private Transform _grabTransform;
    private Vector3 _rayDirection;

    public bool activeWhenUseVr;

    private void Start()
    {
#if USE_VR
        //_grabTransform = 
        if (!attachPoint)
        {  
            attachPoint = transform;
        }
#else
        //_mainCam = Camera.main;
        //_grabTransform = _mainCam.transform;
#endif
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_pickup)
            {
                Drop();
            }
            else
            {
                Pickup();
            }
        }


        if (_pickup)
        {
            var distance = Vector3.Distance(attachPoint.position, _pickupCube.transform.position);
            if (distance > detachDistance)
            {
                Drop();
            }
        }
    }


    private void FixedUpdate()
    {
#if USE_VR

        _rayDirection = attachPoint.right * -1;
#else
        float rotY = Input.GetKey(KeyCode.Q) ? -1.0f : 0.0f;
        rotY += Input.GetKey(KeyCode.E) ? 1.0f : 0.0f;
        rotY *= Time.deltaTime * rotateSpeed;
        var newRotY = new Vector3(0, rotY, 0);
        attachPoint.Rotate(newRotY);

        float rotX = Input.GetKey(KeyCode.R) ? -1.0f : 0.0f;
        rotX += Input.GetKey(KeyCode.F) ? 1.0f : 0.0f;
        rotX *= Time.deltaTime * rotateSpeed;
        var newRotX = new Vector3(rotX, 0, 0);
        attachPoint.Rotate(newRotX);
        _rayDirection = attachPoint.forward;
#endif


        RaycastHit hit;
        if (Physics.Raycast(attachPoint.position, _rayDirection, out hit, rayDistance))
        {
            Debug.DrawRay(attachPoint.position, _rayDirection * rayDistance, Color.red, .02f);
            if (hit.transform.GetComponent<PickupCube>())
            {
                var temp = hit.transform.GetComponent<PickupCube>() ? hit.transform.GetComponent<PickupCube>() : null;

                if (_rayhitCube != temp)
                {
                    if (_rayhitCube) _rayhitCube.ActivateOutline(false);
                }

                _rayhitCube = temp;
                _rayhitCube.ActivateOutline(true);
            }
        }
        else
        {
            if (_rayhitCube)
            {
                _rayhitCube.ActivateOutline(false);
                _rayhitCube = null;
            }
        }
    }


    public void Pickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(attachPoint.position, _rayDirection, out hit, rayDistance))
        {
            if (hit.transform.GetComponent<PickupCube>())
            {
                _pickup = true;
                _pickupCube = hit.transform.GetComponent<PickupCube>();
                _pickupCube.Pickup(attachPoint, handlePower, rotatePower);
            }
        }
    }

    public void Drop()
    {
        if (_pickupCube)
        {
            _pickup = false;
            _pickupCube.Drop();
            attachPoint.rotation = transform.rotation;
        }
    }
}