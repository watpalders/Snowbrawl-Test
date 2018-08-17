using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 enemyPosition;
    public GameObject clickLight;
    [Range(30.0f, 500.0f)] public float TargetRadius;
    [Range(20.0f, 70.0f)] public float LaunchAngle;

    Rigidbody rb;
    public Transform deathBall;
    public ParticleSystem _psystem;


    private void Start()  // Basically Launch
    {
        GetEnemyPos();
        Launch();
        GameObject light = (GameObject)Instantiate(clickLight, new Vector3(enemyPosition.x, 5+ enemyPosition.y, enemyPosition.z), Quaternion.Euler(90f,0f,0f));
        Destroy(light, 1.5f);
    }

    private void GetEnemyPos() // THIS IS CORRECT!!!
    {
        EnemyBehavior enemyBehavior = FindObjectOfType<EnemyBehavior>();
        enemyPosition = enemyBehavior.selectedTarget.transform.position;
        transform.LookAt(enemyPosition);
    }

    private void Launch()
    {
        rb = GetComponent<Rigidbody>();
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetXZPos = new Vector3(enemyPosition.x, enemyPosition.y, enemyPosition.z);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = enemyPosition.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rb.velocity = globalVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
       
        ContactPoint contact = collision.contacts[0];
        Transform newDeathBall = Instantiate(deathBall, transform.position, transform.rotation, contact.otherCollider.transform) as Transform;
        ParticleSystem newBallDeathParticle = Instantiate(_psystem, transform.position, transform.rotation) as ParticleSystem;
        Destroy(gameObject);
    }
}