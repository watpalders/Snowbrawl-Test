using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkParticle : MonoBehaviour
{
    private ParticleSystem psystem;
    public EnemyBehavior eBehavior;
    private float throwAngle;
    public Gradient grad;
    public float gradColor;

    private void Start()
    {
        psystem = GetComponent<ParticleSystem>();
        if (eBehavior != null)
        {
            throwAngle = eBehavior.launchAngle;
        }
        //eProjectileScript = GetComponentInParent<EnemyProjectile>();
        //throwAngle = eProjectileScript.launchAngle;

        gradColor = throwAngle / 70f;
        var psystemcolor = psystem.main;
        psystemcolor.startColor = grad.Evaluate(gradColor);

    }

}
