
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        Ray ray = new Ray(_mainCam.transform.position, _mainCam.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
            //transform.loc
            var hitDist = hitInfo.distance;
        }
        else
        {
            transform.position = _mainCam.transform.position + _mainCam.transform.forward;
        }
    }
}
