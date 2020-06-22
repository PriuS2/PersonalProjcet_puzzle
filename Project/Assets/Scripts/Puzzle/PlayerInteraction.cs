using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private Camera _mainCam;
    private bool _pickup;
    public Transform attachPoint;
    public float handlePower;
    public float rotatePower;
    public float detachDistance;
    
    private PickupCube _pickupCube;

    private void Start()
    {
        _mainCam = Camera.main;
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
                //hit.transform.SetParent(_mainCam.transform);
            }
        }
    }

    private void Drop()
    {
        if (_pickupCube)
        {
            _pickup = false;
            _pickupCube.Drop();
        }
    }
    
    
}
