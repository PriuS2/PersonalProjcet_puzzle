﻿using System;
using UnityEngine;

namespace Puzzle
{
    public class PickupCube : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private bool _attached;
        private Transform _attachPoint;
        private float _handlePower;
        private float _rotatePower;
        private bool _collision;
        private Outline _outline;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>()? GetComponent<Rigidbody>() : gameObject.AddComponent<Rigidbody>();
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = Color.red;
            _outline.OutlineWidth = 0f;
        }

        private void Update()
        {
            if (_attached)
            {
                var thisTransform = transform;
                var newDir = _attachPoint.position - thisTransform.position;
                var newVel = _collision? newDir * (_handlePower * .3f) : newDir * _handlePower;
                _rigidbody.velocity = newVel;

                var targetRot = _attachPoint.rotation.eulerAngles;
                thisTransform.eulerAngles = targetRot;
            }
        }

        public void Pickup(Transform attachPoint, float handlePower, float rotatePower)
        {
            _rigidbody.useGravity = false;
            _attached = true;
            _attachPoint = attachPoint;
            _handlePower = handlePower;
            _rotatePower = rotatePower;
        }

        public void Drop()
        {
            var vel = _rigidbody.velocity;
            _rigidbody.useGravity = true;
            _attached = false;
        }


        private void OnCollisionEnter(Collision other)
        {
            _collision = true;
        }

        private void OnCollisionExit(Collision other)
        {
            _collision = false;
        }

        public void ActivateOutline(bool state)
        {
            _outline.OutlineWidth = state ? 10.0f : 0.0f;
        }
    }
}