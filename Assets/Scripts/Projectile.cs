using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 mousePosition;    // target transform
    public GameObject clickLight;
    [Range(10.0f, 500.0f)] public float TargetRadius;
    float LaunchAngle;
    float launchPower;

    Rigidbody rb;
    public Transform deathBall;
    public ParticleSystem _psystem;
    ThirdPersonController tpc;


    private void Start()  // Basically Launch
    {
        tpc = FindObjectOfType<ThirdPersonController>();
        GetMousePosLook();
        SetLaunchAngle();

        Launch();
        GameObject light = (GameObject)Instantiate(clickLight, new Vector3(mousePosition.x, 5+ mousePosition.y, mousePosition.z), Quaternion.Euler(90f,0f,0f));
        Destroy(light, 1.5f);
    }
    public void SetLaunchAngle()
    {
        LaunchAngle = tpc.launchAngle;
    }

    private void GetMousePosLook() // THIS IS CORRECT!!!
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                mousePosition = hit.point;
                transform.LookAt(mousePosition);
            }
        }           
    }

    private void Launch()
    {
        rb = GetComponent<Rigidbody>();
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetXZPos = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = mousePosition.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        launchPower = tpc.launchPower;
        Vector3 localVelocity = new Vector3(0f, Vy, launchPower + Vz);
        print(launchPower);
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
}