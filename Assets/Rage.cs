using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rage : MonoBehaviour
{
    public Image rageUI;
    public float rage;
    public float rageAdder = .1f;
    public float rageNewWave = 10f;
    public float rageCountdown =3f;
    public float rageTimer;
    public float rageMax = 5f;

    private void Start()
    {
        rageTimer = rageNewWave;
    }

    private void Update()
    {
        RageCountDown();
        rageUI.fillAmount = (rage - 1f)/(rageMax - 1f);
    }

    private void RageCountDown()
    {

        if(rage < rageMax) {
            rageTimer -= Time.deltaTime;
            if (rageTimer <= 0)
            {
                rage += rageAdder;
                rageTimer = rageCountdown;
            }
        }

    
    }
    public void ResetRageCounter()
    {
        rage = 1f;
        rageTimer = rageNewWave;
    }
}
