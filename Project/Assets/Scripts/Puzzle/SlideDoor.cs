using System;
using UnityEngine;

namespace Puzzle
{
    public class SlideDoor : MonoBehaviour
    {
        public Transform doorTransform;
        public float slideHeight;
        public float slideSpeed;

        private float _initHeight;
        private float _targetHeight;

        private bool _needMove;

        private void Start()
        {
            _initHeight = doorTransform.position.y;
        }

        private void Update()
        {
            if (!_needMove) return;
            
            var oldPos = doorTransform.position;
            var newHeight = Mathf.Lerp(doorTransform.position.y, _targetHeight, Time.deltaTime * slideSpeed);
            Vector3 newPos;
            

            if (Mathf.Abs(newHeight - _targetHeight) < .02)
            {
                newPos = new Vector3(oldPos.x, _targetHeight, oldPos.z);
            }
            else
            {
                newPos = new Vector3(oldPos.x, newHeight, oldPos.z);
            }
            doorTransform.transform.position = newPos;
        }


        public void ReceiveMessage(MessageState ms)
        {
            
            if (ms.state)
            {
                _targetHeight = _initHeight + slideHeight;
            }
            else
            {
                _targetHeight = _initHeight;
            }
            
            _needMove = true;
        }
    }
}
