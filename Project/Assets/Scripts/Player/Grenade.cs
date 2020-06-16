using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody _rb;
    public PlayerFire playerFire;
    public float launchPower;
    public GameObject explosion;
    
    public ParticleSystem[] particles;
    
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce((transform.forward+transform.up)*launchPower);
    }
    
    
    private void OnCollisionEnter(Collision other)
    {
        foreach (var particle in particles)
        {
            particle.Play();
        }

        playerFire.ReturnToPool(gameObject);
    }

}
