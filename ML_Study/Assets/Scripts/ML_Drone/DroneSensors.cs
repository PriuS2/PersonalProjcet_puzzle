using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSensors : MonoBehaviour
{
    public Transform[] sensors;

    public float rayRadius = 0.2f;
    public float maxDistance = 10.0f;
    private int _layerMask;

    private void Start()
    {
        _layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
    }


    public float[] SensorCheck()
    {
        var sensorLength = sensors.Length;
        var result = new float[sensors.Length];
        
        
        //foreach (var sensor in sensors)
        for(int sensorNum = 0; sensorNum < sensorLength; sensorNum++)
        {
            var ray = new Ray(sensors[sensorNum].position, sensors[sensorNum].forward);
            var hit = new RaycastHit();
            bool isHit;
            
            var hitDistance = 0.0f;
            if (Physics.SphereCast(ray, rayRadius, out hit, maxDistance, _layerMask))
            {
                hitDistance = hit.distance;
                isHit = true;
            }
            else
            {
                hitDistance = maxDistance;
                isHit = false;
            }
            DrawLine(ray.origin, ray.direction * hitDistance, isHit);
            result[sensorNum] = hitDistance / maxDistance;
        }
        return result;
    }

    // public int GetCollectionSize()
    // {
    //     return sensors.Length;
    // }

    private void DrawLine(Vector3 start, Vector3 directionDistance, bool hit)
    {
        var color = hit? Color.green : Color.black;
        Debug.DrawRay(start, directionDistance, color);
    }
}
