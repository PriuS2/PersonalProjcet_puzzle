using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawGizomForward : MonoBehaviour
{
    public float length;
    public float arrowScale = 0.1f;
    //
    // public float fov;
    // public float maxRange;
    // public float minRange;
    // public float aspect;

    private void OnDrawGizmos()
    {
        //var upDir = transform.up;
        var forwardDir = transform.forward;
        Gizmos.color = new Color(forwardDir.x, forwardDir.y, forwardDir.z);

        var pos = transform.position;
        
        Gizmos.DrawRay(pos, forwardDir*length);
        
        // Gizmos.color = Color.red;
        // Gizmos.DrawFrustum(transform.position + forwardDir*length, fov, maxRange, minRange, aspect);

        var rightDir = transform.right;
        var arrowEndPos = pos + forwardDir * length;
        Gizmos.DrawLine(arrowEndPos, arrowEndPos + (- forwardDir+rightDir)*arrowScale);
        Gizmos.DrawLine(arrowEndPos, arrowEndPos + (-forwardDir-rightDir)*arrowScale);
    }
}
