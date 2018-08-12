using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{

    Vector3 velocity;
    Rigidbody myRigidbody;
    Camera viewCamera;
    ThirdPersonController controller;
    TPGunController gunController;
    float snowballChargeTimer = 0;
    public float muzzlePower = 0f;
    [SerializeField] float powerRatio = 4000f;
    public float moveSpeed = 7;

    void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        myRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<TPGunController>();
        viewCamera = Camera.main;
    }

    private void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    public void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);

    }
    void Update()
    {

        if (GetComponent<ClickOn>().currentlySelected != true) // if selected
        {
            StartMoving();
            LookAtMouse();
            // weapon input
            SetMuzzlePower();
            if (Input.GetMouseButtonUp(0))
            {
                gunController.Shoot();
            }

        }
        else
        {
            StopMoving();
        }

    }

    void SetMuzzlePower()
    {
        if (Input.GetMouseButton(0))
        {
            //StopMoving();
            snowballChargeTimer += Time.deltaTime;
            muzzlePower = snowballChargeTimer * powerRatio;
        }
        if (Input.GetMouseButtonDown(0))
        {
            snowballChargeTimer = 0;
        }

    }


    private void StartMoving()
    {
        Vector3 moveInput = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")); 
        Vector3 moveVelocity = moveInput * moveSpeed;
        controller.Move(moveVelocity);
    }

    private void LookAtMouse()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }
    }

    private void StopMoving()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        Vector3 moveVelocity = moveInput.normalized * 0;  // the 0 makes it stop imediatly. make the movespeed taper down to smooth stopping
        controller.Move(moveVelocity);
    }
}