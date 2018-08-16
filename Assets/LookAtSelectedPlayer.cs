using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSelectedPlayer : MonoBehaviour {
    private Vector3 enemyPosition;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        EnemyBehavior enemyBehavior = FindObjectOfType<EnemyBehavior>();
        enemyPosition = enemyBehavior.selectedTarget.transform.position;
    }
}
