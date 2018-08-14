using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] public float speed = 10f;
    Rigidbody rb;
    public Transform deathBall;
    public ParticleSystem _psystem;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        ContactPoint contact = collision.contacts[0];
        Transform newDeathBall = Instantiate(deathBall, transform.position, transform.rotation, contact.otherCollider.transform) as Transform;
        ParticleSystem newBallDeathParticle = Instantiate(_psystem, transform.position, transform.rotation) as ParticleSystem;
    }


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

}