using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]

public class Test : MonoBehaviour
{
    public List<GameObject> testOjbjeObjects = new List<GameObject>();
    
    public GameObject obj;
    
    private void Awake()
    {
        
    }

    private void Update()
    {
        testOjbjeObjects.Add(Instantiate(obj, transform.position, Quaternion.identity));
        
        
        //Destroy(testOjbjeObjects[0], 3.0f);
        while (true)
        {
            if (testOjbjeObjects.Count > 30)
            {
                DestroyImmediate(testOjbjeObjects[0], true);
                testOjbjeObjects.Remove(testOjbjeObjects[0]);
            }
            else
            {
                break;
            }
        }

    }
}

public class Test2 : MonoBehaviour
{
    private void Update()
    {
        print("222222");
    }
}