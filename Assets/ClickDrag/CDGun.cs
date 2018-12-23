using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDGun : MonoBehaviour
{


    public Transform muzzle;
    public CDProjectile projectile;
    public float msBetweenShots = 1000f;
    float nextShotTime;



    public void EnemyShoot()
    {
        if (transform.parent.parent.gameObject.tag == "Enemy")
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + msBetweenShots / 1000f;
                CDProjectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as CDProjectile;
            }
        }
    }
    public void Shoot()
    {
        if (transform.parent.parent.gameObject.tag == "Player")
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + 200f / 1000f;
                CDProjectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as CDProjectile;
            }
        }
    }


}