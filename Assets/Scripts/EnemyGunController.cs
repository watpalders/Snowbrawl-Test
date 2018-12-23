using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{

    [SerializeField] private Vector3 enemyPosition;    // target transform
    public GameObject clickLight;
    [Range(1.0f, 300.0f)] public float TargetRadius;
    [Range(20.0f, 70.0f)] public float LaunchAngle;

    [SerializeField] public float speed = 10f;
    Rigidbody rb;
    public Transform deathBall;
    public ParticleSystem _psystem;

    //old gun controller
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;


    private void Start()  // Basically Launch
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
        GetEnemyPos(); // get from Target File
        rb = GetComponent<Rigidbody>();
        Launch();
        GameObject light = (GameObject)Instantiate(clickLight, new Vector3(enemyPosition.x, 5 + enemyPosition.y, enemyPosition.z), Quaternion.Euler(90f, 0f, 0f));
        Destroy(light, 1.5f);   
        //  rb.AddForce(transform.up * speed);
    }

    private void GetEnemyPos()
    {
                EnemyBehavior enemyBehavior = GetComponent<EnemyBehavior>();
        enemyPosition = enemyBehavior.selectedTarget.transform.position;
    }

    private void Launch()
    {

        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(enemyPosition.x, 0.0f, enemyPosition.z);

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
        Destroy(gameObject);
        ContactPoint contact = collision.contacts[0];
        Transform newDeathBall = Instantiate(deathBall, transform.position, transform.rotation, contact.otherCollider.transform) as Transform;
        ParticleSystem newBallDeathParticle = Instantiate(_psystem, transform.position, transform.rotation) as ParticleSystem;
    }


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }
    public void Shoot()
    {
        if (equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }
}
