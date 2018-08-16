using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonGun : MonoBehaviour
{


    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 1000f;
    float snowballVelocity;
    float nextShotTime;

    
    void FixedUpdate()
    {
        EnemyShoot();
    }

    public void EnemyShoot()
    {
        if (transform.parent.parent.gameObject.tag == "Enemy")
        {
            if (Time.time > nextShotTime)
            {
                snowballVelocity = 2000f; //set based on player location
                print(snowballVelocity);
                nextShotTime = Time.time + msBetweenShots / 1000;
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
                print(snowballVelocity);
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            }
        }
    }


}
