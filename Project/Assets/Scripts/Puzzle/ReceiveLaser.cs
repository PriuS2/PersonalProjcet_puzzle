using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

        public LaserEmitter.LaserColor reactColor;
        private Light _light;
        
        private void Start()
        {   
            Color color = new Color();
            switch (reactColor)
            {
                case LaserEmitter.LaserColor.Red:
                {
                    color = Color.red;
                    break;
                }
                case LaserEmitter.LaserColor.Green:
                {
                    color = Color.green;
                    break;
                }
                case LaserEmitter.LaserColor.Blue:
                {
                    color = Color.blue;
                    break;
                }
            }

            if (type == ReactType.Sensor)
            {
                Material newMaterial = new Material(GetComponent<MeshRenderer>().material);
                GetComponent<MeshRenderer>().material = newMaterial;
                newMaterial.color = color;
                
                _light = transform.GetComponentInChildren<Light>();
                _light.color = color;
                _light.enabled = false;
            }
            //GetComponent<MeshRenderer>().material.color = color;
            
            // Material test = new Material(GetComponent<MeshRenderer>().material);
            // GetComponent<MeshRenderer>().material = test;
            // test.color = color;


        }


        public void ReceiveMessage(MessageState state)
        {

            if (state.state)//On
            {
                if (!laserList.Contains(state.owner) & state.laserColor == reactColor)
                {
                    if(type == ReactType.Sensor) _light.enabled = true;
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
                if (laserList.Contains(state.owner) & state.laserColor == reactColor)
                {
                    if(type == ReactType.Sensor) _light.enabled = false;
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

            // if (reactColor == LaserEmitter.LaserColor.Red)
            // {
            //     print(state.state);
            // }
            

        }
    }
}
