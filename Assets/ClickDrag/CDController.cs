using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CDController : MonoBehaviour
{

    Vector3 velocity;
    Rigidbody myRigidbody;
    Camera viewCamera;
    CDController controller;
    CDGunController gunController;
    public float launchAngle;
    public float launchPower;
    public float maxPower = 70f;
    public float powerRatio = 200f;
    float launchPlaceholder;
    float launchAnglePlaceholder;
    public float angleRatio = 30f;
    public float moveSpeed = 7;

    void Start()
    {
        controller = this.gameObject.GetComponent<CDController>();
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        gunController = this.gameObject.GetComponent<CDGunController>();
        viewCamera = Camera.main;
    }

    private void Update()
    {

        if (Input.GetMouseButton(0)) // if selected
        {
            SetLaunchAngle();
            SetLaunchPower();
        }

        if (this.gameObject.GetComponent<CDClickOn>().currentlySelected == true && Input.GetMouseButtonUp(0)) // had to move this here from above.
        {
            gunController.Shoot();

        }
    }
    public void SetLaunchPower()
    {
        if (Input.GetMouseButton(0))
        {
            launchPlaceholder += Time.deltaTime * powerRatio;
            if (launchPlaceholder <= 0)
            {
                launchPower = 1;
            }
            else if (launchPlaceholder > maxPower)
            {
                launchPower = maxPower;
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
        if (Input.GetMouseButtonDown(0))
        {
            launchAngle = 70f;
        }
        if (Input.GetMouseButton(0))
        {
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
    }
}