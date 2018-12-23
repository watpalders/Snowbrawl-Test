using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;
    float nextShotTime;
    private Rage rageScript;
    private float rageMultiplier = 1f;
    private EnemyBehavior enemyBehavior;

    [Header("Animation Timing Things")]
    private Animator animator;
    public float timerRandomizer = 1f;
    public float makeSnowball = 2f;
    public float windUp = 3f;
    public float throwBall = 1f;
    public float makeSnowballTimer;
    public float windUpTimer;
    public float throwBallTimer;


    private void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        rageScript = this.gameObject.GetComponentInParent<Rage>();
        animator = this.gameObject.GetComponent<Animator>();
        makeSnowballTimer = makeSnowball + Random.Range(0f, timerRandomizer);
        windUpTimer = windUp;
        throwBallTimer = throwBall;
        if (startingGun != null){
            EquipGun(startingGun);
        }
    }
    //    Rage STUFF!!

        //



    private void Update()
    {
        if(enemyBehavior.selectedTarget != null)
        {
            RunTimer();
        }

        rageMultiplier = rageScript.rage;
    }

    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    private void RunTimer()
    {
        makeSnowballTimer -= Time.deltaTime * rageMultiplier;
        if (windUpTimer > 0 && throwBallTimer > 0)
        {
            animator.SetBool("Make", true);
            animator.SetBool("WindUp", false);
            animator.SetBool("Throw", false);
            windUpTimer -= Time.deltaTime * rageMultiplier;
        }
        if (makeSnowballTimer < 0 && windUpTimer < 0 && throwBallTimer > 0)
        {
            animator.SetBool("Make", false);
            animator.SetBool("WindUp", true);
            animator.SetBool("Throw", false);
            throwBallTimer -= Time.deltaTime * rageMultiplier;
        }
        if (makeSnowballTimer < 0 && windUpTimer < 0 && throwBallTimer < 0)
        {
            animator.SetBool("Make", false);
            animator.SetBool("WindUp", false);
            animator.SetBool("Throw", true);
            AudioManager.instance.Play("Throw");
            equippedGun.EnemyShoot();
            makeSnowballTimer = makeSnowball + Random.Range(0f, timerRandomizer);
            windUpTimer = windUp;
            throwBallTimer = throwBall;
        }
    }

    public void HitBySnowball()
    {
        animator.Play("Hit");
    }
}