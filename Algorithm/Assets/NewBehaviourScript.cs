using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Algo))]
public class CubeGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var component = (Algo)target;
        if (GUILayout.Button("Swap!!!!!"))
        {
            component.ElementSwap();
        }
        
        if (GUILayout.Button("Suffle!!!!!"))
        {
            component.Shuffle();
        }
        
        if (GUILayout.Button("Reset!!!!!"))
        {
            component.ResetArray();
        }
    }
}