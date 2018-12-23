using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour {

    private List<Transform> targets;
    private List<Transform> playersAliveList;
    public Transform selectedTarget;
    private Transform myTransform;
    public ParticleSystem deathParticle;
    public float enemyStartHealth = 1f;
    private float enemyHealth;
    public int listSize;
    private int playersAliveListSize;
    public float randomAccuracy = 5f;
    [Header("Healthbar Stuff")]
    public Image healthBar;
    private GunController gunController;

    //private MovingPlats movingPlats;
    

    //public GameObject clickLight;
    [Range(30.0f, 500.0f)] public float targetRadius;
    [Range(10.0f, 70.0f)] public float launchAngle;
    public float enemyMaxForce = 20f;





    void Awake()
    {
        targets = new List<Transform>();
        playersAliveList = new List<Transform>();
        myTransform = transform;
        enemyHealth = enemyStartHealth;
        gunController = GetComponent<GunController>();

        //movingPlats = FindObjectOfType<MovingPlats>();
    }


    void Update()
    {
        AddAllTargets();
        ListOfPlayersLeft();

        if (listSize + playersAliveListSize < 1)
        {
            //Game Over
            StartCoroutine(WaitForIt(3.0F));
        }
        TargetPlayer();

    }
    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // +1
        Debug.Log("Game Over UI?!?!?! AAHHHH YEAAAA");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "snowball")
        {
            gunController.HitBySnowball();
            enemyHealth -= 1f;
            healthBar.fillAmount = enemyHealth/enemyStartHealth;
            if(enemyHealth <= 0)
            {
                //movingPlats.moveToTheRight = false;
                AudioManager.instance.Play("PlayerDeath");
                Destroy(gameObject);
                ParticleSystem newDeathParticle = Instantiate(deathParticle, transform.position, transform.rotation) as ParticleSystem;
            }


            //Debug.Log("collision detected!");
        }
    }
    public void AddAllTargets()
    {
        targets.RemoveAll(item => item == null);
        GameObject[] go = GameObject.FindGameObjectsWithTag("ActivePlayer");
        foreach (GameObject player in go)
        {
            AddTarget(player.transform);
        }
        playersAliveList.RemoveAll(item => item == null);
    }
    public void AddTarget(Transform player)
    {
        if (targets.IndexOf(player) < 0)
        {
            targets.Add(player);

        }
    }

    private void SortTargetsByDistance()
    {
        targets.Sort(delegate (Transform t1, Transform t2)
        {
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });
    }

    private void TargetPlayer()
    {
        if (listSize > 0)
        {
            SortTargetsByDistance();
            selectedTarget = targets[0];
            transform.LookAt(selectedTarget);
        }
        //if(listSize <= 0)
        //{
        //    selectedTarget = Camera.main.transform;
        //    transform.LookAt(Camera.main.transform);
        //}
    }
    private void ListOfPlayersLeft()
    {
        GameObject[] goa = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject allplayer in goa)
        {
            AddAllTarget(allplayer.transform);
        }
        listSize = targets.Count;
        playersAliveListSize = playersAliveList.Count;
    }
    public void AddAllTarget(Transform allplayer)
    {
        if (playersAliveList.IndexOf(allplayer) < 0)
        {
            playersAliveList.Add(allplayer);

        }
    }
}
