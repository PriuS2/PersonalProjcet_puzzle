using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;

public class Test_ReceiveMessage : MonoBehaviour
{
    public GameObject light;
    public void ReceiveMessage(MessageState state)
    {
        light.SetActive(state.state);
    }
}
