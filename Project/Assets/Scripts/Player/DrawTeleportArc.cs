using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class DrawTeleportArc : MonoBehaviour
    {
        public Transform launchTransform;
        public float launchPower;
        public float samplingFrequency;
        private bool _draw;
        public bool drawDebugLine;

        public float maxPathNum;
        private Vector3 _gravity;

        private LineRenderer _lineRenderer;
        private RaycastHit _raycastHit;

        private Vector3 _destination;

        [SerializeField]private List<Vector3> _path;

        private void Start()
        {
            //_draw = true;
            _gravity = Physics.gravity;
            _lineRenderer = gameObject.GetComponent<LineRenderer>()?  gameObject.GetComponent<LineRenderer>() : gameObject.AddComponent<LineRenderer>();
            _path = new List<Vector3>();
        }


        private void Update()
        {
            if (!_draw) return;

            
            _path.Clear();

            var startPos = launchTransform.position;
            _path.Add(startPos);
            
            
            // var launchDegree = launchTransform.eulerAngles.z;
            var deltaPosition = launchTransform.forward * launchPower;
            var endPos = startPos + deltaPosition;
            
            
            for (int i = 0; i < maxPathNum; i++)
            {
                var isHit = Raycast(startPos, endPos);
                
                startPos = endPos;
                deltaPosition += _gravity * .02f;
                endPos = startPos + deltaPosition;

                if (isHit)
                {
                    _path.Add(_raycastHit.point);
                    _destination = _raycastHit.point;
                    break;
                }
                else
                {
                    _path.Add(startPos);
                }
            }
            
            _lineRenderer.positionCount = _path.Count;
            _lineRenderer.SetPositions(_path.ToArray());
            
        }

        public void DrawStart()
        {
            _draw = true;
            _lineRenderer.enabled = true;
        }

        public Vector3 DrawEnd()
        {
            _draw = false;
            _lineRenderer.enabled = false;
            return _destination;
            
        }

        private  bool Raycast(Vector3 startPos, Vector3 endPos)
        {
            //RaycastHit hit;
            var deltaVec = endPos - startPos;
            var dist = deltaVec.magnitude;
            var dir = deltaVec.normalized;
            
            if (Physics.Raycast(startPos, dir, out _raycastHit, dist))
            {
                DrawDebugLine(startPos, _raycastHit.point);
                DrawDebugBox(_raycastHit.point, .15f, Color.green, 0.02f);
                return true;
            }
            else
            {
                DrawDebugLine(startPos, endPos, Color.red);
                DrawDebugBox(endPos, .15f, Color.red, 0.02f);
                return false;
            }
        }


        private void DrawDebugBox(Vector3 position, float radius, Color color, float duration)
        {
            if (!drawDebugLine) return;
            
            var up = position + Vector3.up * radius;
            var down = position + Vector3.down * radius;
            var left = position + Vector3.left * radius;
            var right = position + Vector3.right * radius;
            var forward = position + Vector3.forward * radius;
            var back = position + Vector3.back * radius;

            DrawDebugLine(up, left, color, duration);
            DrawDebugLine(up, right, color, duration);
            DrawDebugLine(up, forward, color, duration);
            DrawDebugLine(up, back, color, duration);
            
            DrawDebugLine(down, left, color, duration);
            DrawDebugLine(down, right, color, duration);
            DrawDebugLine(down, forward, color, duration);
            DrawDebugLine(down, back, color, duration);

            DrawDebugLine(left, back, color, duration);
            DrawDebugLine(back, right, color, duration);
            DrawDebugLine(forward, right, color, duration);
            DrawDebugLine(forward, left, color, duration);
        }

        
        
        private void DrawDebugLine(Vector3 start, Vector3 end)
        {
            if (!drawDebugLine) return;
            Debug.DrawLine(start, end, Color.green, 0.02f);
        }
        
        private void DrawDebugLine(Vector3 start, Vector3 end, Color color)
        {
            if (!drawDebugLine) return;
            Debug.DrawLine(start, end, color, 0.02f);
        }
        
        private void DrawDebugLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if (!drawDebugLine) return;
            Debug.DrawLine(start, end, color, duration);
        }
    }
}    
