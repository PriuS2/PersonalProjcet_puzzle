using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessage : MonoBehaviour
{
    public enum ReactType
    {
        None,
        CollisionHit,
        TriggerHit
    }

    public ReactType reactType;
    public GameObject sendTo;
    public string methodName = "ReceiveMessage";

    [SerializeField] private bool _state;


    private void OnCollisionEnter(Collision other)
    {
        if (sendTo & reactType == ReactType.CollisionHit)
        {
            sendTo.SendMessage(methodName, true);
            _state = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (sendTo & reactType == ReactType.CollisionHit)
        {
            sendTo.SendMessage(methodName, false);
            _state = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sendTo& reactType == ReactType.TriggerHit)
        {
            sendTo.SendMessage(methodName, true);
            _state = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sendTo& reactType == ReactType.TriggerHit)
        {
            sendTo.SendMessage("ReceiveMessage", false);
            _state = false;
        }
    }

    public void ReceiveMessage(bool state)
    {
        _state = state;
        if (sendTo)
        {
            sendTo.SendMessage("ReceiveMessage", state);
        }
    }
}
