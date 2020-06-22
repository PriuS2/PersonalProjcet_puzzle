using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Puzzle
{
    public class ReceiveLaser : MonoBehaviour
    {
        public Component sendComponent;
        public string methodName = "ReceiveMessage";
        [SerializeField]private List<Transform> laserList = null;

        [SerializeField] private bool currentState;
        public enum ReactType
        {
            None,
            Reflect,
            Sensor
        };

        public ReactType type = ReactType.None;


        public void ReceiveMessage(MessageState state)
        {

            if (state.state)//On
            {
                if (!laserList.Contains(state.owner))
                {
                    laserList.Add(state.owner);
                }

                if (laserList.Count > 0)
                {
                    currentState = true;
                    if (type == ReactType.Sensor & sendComponent)
                    {
                        sendComponent.SendMessage(methodName, state);
                    }
                }

            }
            else//Off
            {
                if (laserList.Contains(state.owner))
                {
                    laserList.Remove(state.owner);
                }
                
                if (laserList.Count <= 0)
                {
                    currentState = false;
                    if (type == ReactType.Sensor & sendComponent)
                    {
                        sendComponent.SendMessage(methodName, state);
                    }
                }
            }
            
            

        }
    }
}
