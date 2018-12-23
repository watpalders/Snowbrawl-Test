using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowBankHealth : MonoBehaviour
{

    public float startingHealth;
    private float currentHealth;
    public ParticleSystem bankBoom;
    public Image snowbankImage;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }  

    // Update is called once per frame
    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "EnemySnowball")
        {
            currentHealth -= 1f;
            snowbankImage.fillAmount = currentHealth / startingHealth;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                ParticleSystem SnowBankBoooom = Instantiate(bankBoom, transform.position, transform.rotation) as ParticleSystem;
            }
        }
    }
}
