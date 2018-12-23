using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 enemyPosition;
    //public GameObject clickLight;
    //[Range(30.0f, 500.0f)] private float targetRadius;
    [Range(10.0f, 45.0f)] public float launchAngle = 20f;
    public float enemyMaxForce = 25f;
    EnemyBehavior enemyBehavior;

    Rigidbody rb;
    public Transform deathBall;
    public ParticleSystem _psystem;
    private float randomXValue;
    private float randomZValue;
    private float enemyAccuracy;
    public Transform shooter;

    private void Start()  // Basically Launch
    {
        enemyBehavior = shooter.parent.parent.GetComponent<EnemyBehavior>();
        //enemyMaxForce = enemyBehavior.enemyMaxForce;
        if (enemyBehavior.listSize > 0)
        {
            GetEnemyPos();
            Launch();
            //GameObject light = Instantiate(clickLight, new Vector3(enemyPosition.x + randomXValue, 10 + enemyPosition.y, enemyPosition.z + randomZValue), Quaternion.Euler(90f, 0f, 0f));
            //Destroy(light, 1.5f);
        }

    }

    private void GetEnemyPos() // THIS IS CORRECT!!!
    {
        enemyPosition = enemyBehavior.selectedTarget.transform.position;
        enemyAccuracy = enemyBehavior.randomAccuracy;
        randomZValue = Random.Range(-enemyAccuracy, enemyAccuracy);
        randomXValue = Random.Range(-enemyAccuracy, enemyAccuracy);
        //GameObject light = Instantiate(clickLight, new Vector3(enemyPosition.x, enemyPosition.y + enemyAccuracy + 2f, enemyPosition.z), Quaternion.Euler(90f, 0f, 0f));  
        enemyPosition = new Vector3((enemyPosition.x + randomXValue), enemyPosition.y +1f, (enemyPosition.z + randomZValue));

        //Destroy(light, 1.5f);
        transform.LookAt(enemyPosition);
        
    }

    private void Launch()
    {
        rb = GetComponent<Rigidbody>();
        enemyMaxForce = enemyBehavior.enemyMaxForce;
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetXZPos = new Vector3(enemyPosition.x, enemyPosition.y, enemyPosition.z);
        

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        launchAngle = enemyBehavior.launchAngle;
        float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
 
        float H = enemyPosition.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;
        if(Vz > enemyMaxForce)
        {
            Vz = enemyMaxForce;
        }
        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);  //  Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        //print(Vz+ "Is the VZ float.");
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rb.velocity = globalVelocity;
    }


    void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        if (contact.otherCollider.GetComponent<Collider>().tag == "SoftGround")
        {
            AudioManager.instance.Play("Ground");
        }
        if (contact.otherCollider.GetComponent<Collider>().tag == "ActivePlayer")
        {
            AudioManager.instance.Play("PlayerHit");
        }
        else
        {
            AudioManager.instance.Play("HardSurface");
        }
        Transform newDeathBall = Instantiate(deathBall, transform.position, transform.rotation, contact.otherCollider.transform) as Transform;
        ParticleSystem newBallDeathParticle = Instantiate(_psystem, transform.position, transform.rotation) as ParticleSystem;
        Destroy(gameObject);
    }
}