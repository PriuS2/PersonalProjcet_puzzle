using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
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
            MessageState ms = new MessageState {state = true, owner = transform};
            sendTo.SendMessage(methodName, ms);
            _state = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (sendTo & reactType == ReactType.CollisionHit)
        {
            MessageState ms = new MessageState {state = false, owner = transform};
            sendTo.SendMessage(methodName, ms);
            _state = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sendTo& reactType == ReactType.TriggerHit)
        {
            MessageState ms = new MessageState {state = true, owner = transform};
            sendTo.SendMessage(methodName, ms);
            _state = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sendTo& reactType == ReactType.TriggerHit)
        {
            MessageState ms = new MessageState {state = false, owner = transform};
            sendTo.SendMessage("ReceiveMessage", ms);
            _state = false;
        }
    }

    public void ReceiveMessage(MessageState state)
    {
        _state = state.state;
        if (sendTo)
        {
            sendTo.SendMessage("ReceiveMessage", state);
        }
    }
}
