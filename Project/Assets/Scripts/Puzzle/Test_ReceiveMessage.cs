using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;

public class Test_ReceiveMessage : MonoBehaviour
{
    public GameObject light;
    public bool inverse;


    private void Start()
    {
        light.SetActive(inverse);
    }

    public void ReceiveMessage(MessageState state)
    {
        bool newState = inverse ? !state.state: state.state;
        light.SetActive(newState);
    }
    
    
}
