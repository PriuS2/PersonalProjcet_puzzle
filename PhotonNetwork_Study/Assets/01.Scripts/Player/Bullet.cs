using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //[HideInInspector]
    public Vector3 launch;
    [HideInInspector] public float damage = 1;
    public GameObject collisionHitEffect;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = launch;
        // print(_rigidbody.velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        var effect = Instantiate(collisionHitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3);
        Destroy(gameObject);
    }

    public void InitBullet(Vector3 launch, float damage)
    {
        this.damage = damage;
        this.launch = launch;
    }

}
