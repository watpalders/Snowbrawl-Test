using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SledSpawner : MonoBehaviour
{

    public GameObject sledPrefab;
    private GameObject thisObject;
    // Start is called before the first frame update
    void Start()
    {
        thisObject = this.gameObject;
        InvokeRepeating("SpawnSomeSleds", 0f, 1f);
    }

private void SpawnSomeSleds()
    {
        GameObject newSled = Instantiate(sledPrefab, thisObject.transform.position, thisObject.transform.rotation) as GameObject;
        Rigidbody newSledRB = newSled.GetComponent<Rigidbody>();
        newSledRB.AddForce(-transform.forward * 800); ;

    }
}
