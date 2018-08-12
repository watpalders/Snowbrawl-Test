using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 1000f;
    float snowballVelocity;
    float nextShotTime;
    public Vector3 mousePoint;
      


    void FixedUpdate()
    {
        EnemyShoot();
        //MousePosition();
    }

    //void MousePosition()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    //    float rayDistance;
    //    if (groundPlane.Raycast(ray, out rayDistance))
    //    {
    //        Vector3 point = ray.GetPoint(rayDistance);
    //        print(point);
    //    }
    //}

    public void EnemyShoot()
    {
        if (transform.parent.parent.gameObject.tag == "Enemy")
        {
            if (Time.time > nextShotTime)
            {
                snowballVelocity = Random.Range(1000f, 1800f); //set based on player location
                //print(snowballVelocity);
                nextShotTime = Time.time + msBetweenShots / Random.Range(300, 1000);
                Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
                newProjectile.SetSpeed(snowballVelocity);
            }
        }
    }
    public void Shoot()
    {
        if (transform.parent.parent.gameObject.tag == "Player")
        {            
            if (Time.time > nextShotTime)
            {
                snowballVelocity = transform.parent.parent.gameObject.GetComponent<ThirdPersonController>().muzzlePower;
                print(snowballVelocity);
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
                newProjectile.SetSpeed(snowballVelocity);
            }
        }
    }


}
