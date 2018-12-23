using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballColorByAngle : MonoBehaviour
{
    private ParticleSystem psystem;
    public EnemyProjectile eProjectileScript;
    public Projectile pProjectileScript;
    private float throwAngle;
    public Gradient grad;
    public float gradColor;

    private void Start()
    {
        psystem = GetComponent<ParticleSystem>();
        if(eProjectileScript != null)
        {
            throwAngle = eProjectileScript.launchAngle;

        }
        if(pProjectileScript != null)
        {
            throwAngle = pProjectileScript.launchAngle;
        }
        //eProjectileScript = GetComponentInParent<EnemyProjectile>();
        //throwAngle = eProjectileScript.launchAngle;

        gradColor = throwAngle / 70f;
        var psystemcolor = psystem.main;
        psystemcolor.startColor = grad.Evaluate(gradColor);

    }




}
