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

        public bool drawDebugLine;
        
        private void Start()
        {
            _lasers = new VolumetricLineBehavior[maxReflectionNum];
            for (int i = 0; i < maxReflectionNum; i++)
            {
                var temp = Instantiate(laserPrefab, transform);
                _lasers[i] = temp.GetComponent<VolumetricLineBehavior>();
                temp.SetActive(false);
            }
            
        }


        private void Update()
        {
            _currentReflectionNum = 0;
            _reflectionStartPosition = emitPoint.position;
            _laserDirection = emitPoint.forward;
            var newDirection = _laserDirection;
            _RemainLaserDistance = maxLaserDistance;

            foreach (var laser in _lasers)
            {
                laser.gameObject.SetActive(false);
            }

            while (true)
            {
                RaycastHit hit;
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
            
        }


        private void DrawLaser(VolumetricLineBehavior laser, Vector3 start, Vector3 direction, float length)
        {
            _lasers[_currentReflectionNum].gameObject.SetActive(true);
            _lasers[_currentReflectionNum].transform.rotation = Quaternion.identity;
            _lasers[_currentReflectionNum].StartPos = start - transform.position;
            _lasers[_currentReflectionNum].EndPos = start + (direction * length) - transform.position;
            //_lasers[_currentReflectionNum].LineColor = Color.green;
        }
        
    }
}

