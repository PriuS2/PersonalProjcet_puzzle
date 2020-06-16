using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
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
        private GameObject[] _lasers;

        public Transform emitPoint;
        private Vector3 _reflectionStartPosition;
        private Vector3 _laserDirection;
        
        private void Start()
        {
            _lasers = new GameObject[maxReflectionNum];
            for (int i = 0; i < maxReflectionNum; i++)
            {
                var temp = Instantiate(laserPrefab, transform);
                _lasers[i] = temp;
                temp.SetActive(false);
            }
            
        }


        private void Update()
        {
            // RaycastHit hit;
            // if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 1000))
            // {
            //     //Draw Debug Line
            //     StartCoroutine(DrawLine(.4f));
            //     var hitPoint = hit.point;
            //
            //     //var impact = Instantiate(bulletImpact, hitPoint, Quaternion.identity);
            //     //
            //     bulletImpact.transform.position = hitPoint;
            //     bulletImpact.transform.LookAt(hitPoint + hit.normal, hit.normal);
            //     _bulletParticle.Play();
            // }
            _currentReflectionNum = -1;
            _reflectionStartPosition = emitPoint.position;
            _laserDirection = emitPoint.forward;
            var newDirection = _laserDirection;
            _RemainLaserDistance = maxLaserDistance;


            //StartCoroutine(DebugLine(5, new Vector3(0, 0, 0), Vector3.up, 1000));


            while (true)
            {
                RaycastHit hit;
                if (Physics.Raycast(_reflectionStartPosition, newDirection, out hit, _RemainLaserDistance))
                {
                    //Debug
                    Debug.DrawRay(_reflectionStartPosition, newDirection * hit.distance, Color.red);
                    Debug.DrawRay(_reflectionStartPosition + (newDirection * hit.distance),
                        newDirection * _RemainLaserDistance, Color.green);
                    Debug.DrawRay(hit.point, Vector3.up + Vector3.right, Color.cyan);
                    Debug.DrawRay(hit.point, Vector3.right + Vector3.forward, Color.cyan);
                    Debug.DrawRay(hit.point, Vector3.forward + Vector3.up, Color.cyan);
                    
                    // 맞은 지점 hit.point
                    // 거리 hit.distance
                    // hit.normal

                    _RemainLaserDistance -= hit.distance;
                    _reflectionStartPosition = hit.point;
                    newDirection = Vector3.Reflect(newDirection, hit.normal);
                    print(newDirection);
                }
                else
                {
                    break;
                }
                
                _currentReflectionNum++;
                if (_currentReflectionNum >= maxReflectionNum)
                {
                    break;
                }
            }
        }
        
        
        
        
    }
}

