using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ReceiveMessage : MonoBehaviour
{
    public GameObject light;
    public void ReceiveMessage(bool state)
    {
        light.SetActive(state);
    }
}
