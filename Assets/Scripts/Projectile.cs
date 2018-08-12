using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] public float speed = 10f;
    Rigidbody rb;

    public ParticleSystem _psystem;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * speed);
    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
        ParticleSystem newBallDeathParticle = Instantiate(_psystem, transform.position, transform.rotation) as ParticleSystem;
    }


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

}