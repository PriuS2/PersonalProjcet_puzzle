using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Camera _mainCam;
    private bool _pickup;
    public Transform attachPoint;
    //private Vector3 initPosition_attachPoint;
    
    public float handlePower;
    public float rotatePower;
    public float detachDistance;
    public float rotateSpeed;
    
    private PickupCube _pickupCube;
    private PickupCube _rayhitCube;

    private void Start()
    {
        _mainCam = Camera.main;
        //initPosition_attachPoint = attachPoint.position;
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
        
        
        
        RaycastHit hit;
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out hit, 2))
        {
            if (hit.transform.GetComponent<PickupCube>())
            {
                
                var temp = hit.transform.GetComponent<PickupCube>()? hit.transform.GetComponent<PickupCube>() : null;

                if (_rayhitCube != temp)
                {
                    if(_rayhitCube) _rayhitCube.ActivateOutline(false);
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


    private void Pickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out hit, 2))
        {
            if (hit.transform.GetComponent<PickupCube>())
            {
                _pickup = true;
                _pickupCube = hit.transform.GetComponent<PickupCube>();
                _pickupCube.Pickup(attachPoint, handlePower, rotatePower);
            }
        }
    }

    private void Drop()
    {
        if (_pickupCube)
        {
            _pickup = false;
            _pickupCube.Drop();
            attachPoint.rotation = transform.rotation;
        }
    }
    
    
}
