using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{

    public bool isSpawning = true;
    private int listCount;
    WaveManager waveManager;

    private void Start()
    {
        waveManager = GetComponentInParent<WaveManager>();
        isSpawning = true;
        //listCount = enemies.Count;
        //if (isSpawning)
        //{
        //    InvokeRepeating("SpawnEnemies", 2f, .75f);
        //}
    }

    private void Update()
    {


    }

    //private void SpawnEnemies()
    //{
    //    if (listCount > 0)
    //    {
    //        GameObject newEnemy = Instantiate(enemies[0], transform.position, transform.rotation, this.transform.parent) as GameObject;
    //        enemies.Remove(enemies[0]);
    //        listCount -= 1;
    //    }
    //    else
    //        isSpawning = false;



    //}

}
