using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using VolumetricLines;
using Random = UnityEngine.Random;

namespace Puzzle
{
    public class LaserEmitter : MonoBehaviour
    {
        public int maxReflectionNum;
        private int _currentReflectionNum;
        
        public float maxLaserDistance;
        private float _RemainLaserDistance;
        
        public GameObject laserPrefab;
        private VolumetricLineBehavior[] _lasers;
        public Transform emitPoint;
        private Vector3 _reflectionStartPosition;
        private Vector3 _laserDirection;

        public bool activateWhenStart = true;
        private bool _activate = false;
        
        

        public enum LaserColor
        {
            Red,
            Green,
            Blue
        }

        public LaserColor laserColor;

        public bool drawDebugLine;

        private Transform _lastHitSensor;
        
        private void Start()
        {
            _lasers = new VolumetricLineBehavior[maxReflectionNum];
            for (int i = 0; i < maxReflectionNum; i++)
            {
                var temp = Instantiate(laserPrefab, transform);
                _lasers[i] = temp.GetComponent<VolumetricLineBehavior>();


                switch (laserColor)
                {
                    case LaserColor.Red:
                    {
                        _lasers[i].LineColor = Color.red;
                        break;
                    }
                    case LaserColor.Green:
                    {
                        _lasers[i].LineColor = Color.green;
                        break;
                    }
                    case LaserColor.Blue:
                    {
                        _lasers[i].LineColor = Color.blue;
                        break;
                    }
                }
                
                temp.SetActive(false);
            }
            
        }


        private void FixedUpdate()
        {
            //print(activateWhenStart + " / " + _activate + " / " + (activateWhenStart ^ _activate));


            foreach (var laser in _lasers)
            {
                laser.gameObject.SetActive(false);
            }
            
            
            if (!(activateWhenStart ^ _activate))
            {
                if (_lastHitSensor)
                {
                    SendMessage(_lastHitSensor, false);
                    _lastHitSensor = null;
                }
                return;
            }
            
            
            _currentReflectionNum = 0;
            _reflectionStartPosition = emitPoint.position;
            _laserDirection = emitPoint.forward;
            var newDirection = _laserDirection;
            _RemainLaserDistance = maxLaserDistance;
            


            RaycastHit hit;
            while (true)
            {
                if (Physics.Raycast(_reflectionStartPosition, newDirection, out hit, _RemainLaserDistance))
                {
                    //Debug
                    if (drawDebugLine)
                    {
                        Debug.DrawRay(_reflectionStartPosition, newDirection * hit.distance, Color.green);
                        // Debug.DrawRay(_reflectionStartPosition + (newDirection * hit.distance),
                        //     newDirection * _RemainLaserDistance, Color.green);
                        Debug.DrawRay(hit.point, Vector3.up + Vector3.right, Color.cyan);
                        Debug.DrawRay(hit.point, Vector3.right + Vector3.forward, Color.cyan);
                        Debug.DrawRay(hit.point, Vector3.forward + Vector3.up, Color.cyan);
                    }

                    DrawLaser(_lasers[_currentReflectionNum], _reflectionStartPosition, newDirection, hit.distance);
                    
                    _RemainLaserDistance -= hit.distance;
                    _reflectionStartPosition = hit.point;
                    newDirection = Vector3.Reflect(newDirection, hit.normal);


                    if (hit.transform.GetComponent<ReceiveLaser>())
                    {
                        var hitType = hit.transform.GetComponent<ReceiveLaser>().type;
                        // if (hitType != ReceiveLaser.ReactType.Reflect)
                        // {
                        //     _currentReflectionNum = maxReflectionNum;
                        // }
                        switch (hitType)
                        {
                            case ReceiveLaser.ReactType.None:
                            {
                                _currentReflectionNum = maxReflectionNum;
                                break;
                            }
                            case ReceiveLaser.ReactType.Reflect:
                            {

                                break;
                            }
                            case ReceiveLaser.ReactType.Sensor:
                            {
                                _currentReflectionNum = maxReflectionNum;

                                if (_lastHitSensor != hit.transform)
                                {
                                    SendMessage(hit.transform, true);
                                }
                                break;
                            }
                        }
                        
                    }
                    else
                    {
                        _currentReflectionNum = maxReflectionNum;
                    }
                    
                    
                }
                else
                {
                    if(drawDebugLine) Debug.DrawRay(_reflectionStartPosition, newDirection * _RemainLaserDistance, Color.red);
                    DrawLaser(_lasers[_currentReflectionNum], _reflectionStartPosition, newDirection, _RemainLaserDistance);

                    break;
                }
                
                _currentReflectionNum++;
                if (_currentReflectionNum >= maxReflectionNum)
                {
                    break;
                }
            }

            if (_lastHitSensor)
            {
                if (hit.transform != _lastHitSensor)
                {
                    SendMessage(_lastHitSensor, false);
                    _lastHitSensor = null;
                }
            }
        }


        private void DrawLaser(VolumetricLineBehavior laser, Vector3 start, Vector3 direction, float length)
        {
            _lasers[_currentReflectionNum].gameObject.SetActive(true);
            _lasers[_currentReflectionNum].transform.rotation = Quaternion.identity;
            _lasers[_currentReflectionNum].StartPos = start - transform.position;
            _lasers[_currentReflectionNum].EndPos = start + (direction * length) - transform.position;
        }


        private void SendMessage(Transform hitTransform, bool state)
        {
            _lastHitSensor = hitTransform;
            MessageState ms = new MessageState();
            ms.owner = transform;
            ms.state = state;
            ms.laserColor = laserColor;
            _lastHitSensor.gameObject.SendMessage("ReceiveMessage", ms);
            //_lastHitSensor.gameObject.SendMessage("ReceiveMessage",);
        }

        public void ReceiveMessage(MessageState ms)
        {
            _activate = ms.state;
            //print(ms.state);
        }
    }
}

