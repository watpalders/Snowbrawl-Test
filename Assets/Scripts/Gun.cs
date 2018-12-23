using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public EnemyProjectile enemyProjectile;
    public float msBetweenShots = 1000f;
    float nextShotTime;
      

    public void EnemyShoot()
    {
        if (transform.parent.parent.gameObject.tag == "Enemy")
        {
                EnemyProjectile newProjectile = Instantiate(enemyProjectile, muzzle.position, muzzle.rotation) as EnemyProjectile;
                newProjectile.GetComponent<EnemyProjectile>().shooter = transform;
        }
    }
    public void Shoot()
    {
        if (transform.parent.parent.gameObject.tag == "Player")
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                //Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            }
        }
    }


}
