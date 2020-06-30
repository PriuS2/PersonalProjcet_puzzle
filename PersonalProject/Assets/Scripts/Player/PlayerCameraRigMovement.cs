using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRigMovement : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void UpdateCameraPosition(Vector3 deltaPosition, float height)
    {
        var pos = _transform.position + deltaPosition;
        var newPos = new Vector3(pos.x, height, pos.z);
        _transform.position = newPos;
    }
}
