using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    [SerializeField] public Transform hat;
    public Transform hatRack;
    Transform newHat;
    public List<Transform> targets;
    public Transform selectedTarget;
    private Transform myTransform;
    public ParticleSystem deathParticle;

    private void Awake ()
    {
        
        newHat = Instantiate(hat,hatRack.position , Quaternion.identity);
        
    }
    void Start()
    {
        targets = new List<Transform>();
        AddAllTargets();
        myTransform = transform;
    }


    void FixedUpdate()
    {
        TargetPlayer();

    }
    void Update ()
    {
        HatStaysOnHead();
    }

    private void HatStaysOnHead()
    {
        Vector3 hatPosition = hatRack.transform.position;
        newHat.transform.position = hatPosition;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "snowball")
        {

            Destroy(gameObject);
            ParticleSystem newDeathParticle = Instantiate(deathParticle, transform.position, transform.rotation) as ParticleSystem;

            //Debug.Log("collision detected!");
        }
    }
    public void AddAllTargets()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in go)
        {
            AddTarget(player.transform);
        }
    }
    public void AddTarget(Transform player)
    {
        targets.Add(player);
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
        SortTargetsByDistance();
        selectedTarget = targets[0];
        transform.LookAt(selectedTarget);
    }
}
