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
    public float launchAngle;
    public float launchPower;
    public float maxPower = 200f;
    public float powerRatio = 1f;
    float launchPlaceholder;
    public float angleRatio = 30f;
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
            SetLaunchAngle();
            SetLaunchPower();
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
    public void SetLaunchPower()
    {
        if (Input.GetMouseButton(0))
        {

            //StopMoving();
            launchPlaceholder += Time.deltaTime * powerRatio;

            if (launchPlaceholder <= 0)
            {
                launchPower = 0;
            }
            else if (launchPlaceholder > 100)
            {
                launchPower = 100;
            }
            else
            {
                launchPower = launchPlaceholder;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            launchPlaceholder = -20f;
        }
    }
    public void SetLaunchAngle()
    {

        if (Input.GetMouseButton(0))
        {
            //StopMoving();
            launchAngle -= Time.deltaTime * angleRatio;
            if (launchAngle >= 50)
            {
                launchAngle = 50;
            }
            if (launchAngle <= 10)
            {
                launchAngle = 10;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            launchAngle = 70f;
        }
    }

    private void StartMoving()
    {
        Vector3 moveInput = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")); 
        Vector3 moveVelocity = moveInput * moveSpeed;
        controller.Move(moveVelocity);
    }

    public void LookAtMouse()
    {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                controller.LookAt(hit.point);
            }
    }

    private void StopMoving()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        Vector3 moveVelocity = moveInput.normalized * 0;  // the 0 makes it stop imediatly. make the movespeed taper down to smooth stopping
        controller.Move(moveVelocity);
    }
}