using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartColorByAngel : MonoBehaviour
{
    private EnemyBehavior enemyBehavior;
    private float throwAngle;
    public Gradient grad;
    public float gradColor;
    public Image healthBar;



    private void Start()
    {
        Renderer renderer = transform.GetComponent<Renderer>();
        enemyBehavior = transform.GetComponentInParent<EnemyBehavior>();
        throwAngle = enemyBehavior.launchAngle;
        gradColor = throwAngle / 70f;
      //  renderer.material.shader = Shader.Find("_Color");
        renderer.material.color = grad.Evaluate(gradColor);
        if (healthBar != null)
        {
            healthBar.color = grad.Evaluate(gradColor);
        }

    }




}
