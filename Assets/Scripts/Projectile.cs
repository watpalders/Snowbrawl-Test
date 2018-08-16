using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Vector3 mousePosition;    // target transform
    public GameObject clickLight;
    [Range(30.0f, 500.0f)] public float TargetRadius;
    float launchAngle;
    float muzzlePower = 0f;
    public float powerRatio = 1f;
    [Range(20.0f, 70.0f)] public float LaunchAngle;

    [SerializeField] public float speed = 10f;
    Rigidbody rb;
    public Transform deathBall;
    public ParticleSystem _psystem;


    private void Start()  // Basically Launch
    {

        GetMousePos();
        LookAtMouse();
        rb = GetComponent<Rigidbody>();
        SetMuzzlePower();
        Launch();
        GameObject light = (GameObject)Instantiate(clickLight, new Vector3(mousePosition.x, 5+ mousePosition.y, mousePosition.z), Quaternion.Euler(90f,0f,0f));
        Destroy(light, 1.5f);
        //  rb.AddForce(transform.up * speed);
    }

    private void SetMuzzlePower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            muzzlePower = 20f;
        }
        if (Input.GetMouseButton(0))
        {
            //StopMoving();
            launchAngle += Time.deltaTime;
            Debug.Log(launchAngle);
            muzzlePower = launchAngle * powerRatio;
            Debug.Log(muzzlePower);
            LaunchAngle = LaunchAngle + muzzlePower;
            Debug.Log(LaunchAngle);
            //if (muzzlePower >= 3300)
            //{
            //    muzzlePower = 3300;
            //}
            //if (muzzlePower <= 1500)
            //{
            //    muzzlePower = 1500;
            //}

            // todo Max Range = 1 = 2800 , Min Range = .4 = 1500 , 
        }
    }
    void LookAtMouse()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            transform.LookAt(hit.point);
        }
    }
    private void GetMousePos() // THIS IS CORRECT!!!
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                mousePosition = hit.point;
            }
        }           
    }

    private void Launch()
    {

        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(mousePosition.x, 0.0f, mousePosition.z);

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

}