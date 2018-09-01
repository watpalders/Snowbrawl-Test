using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour
{

    public float moveSpeed = 5;

    Camera viewCamera;
    PlayerController controller;
    GunController gunController;
    public float snowballChargeTimer = 0;
    public float muzzlePower = 0f;
    [SerializeField] float powerRatio = 1000f;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;

    }

    void Update()
    {

        if (GetComponent<ClickOn>().currentlySelected != false) // if selected
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
            StopMoving();
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
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
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
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * 0;  // the 0 makes it stop imediatly. make the movespeed taper down to smooth stopping
        controller.Move(moveVelocity);
    }
}