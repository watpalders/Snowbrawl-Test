using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlats : MonoBehaviour
{
    public float speed = 2f;
    private bool isMoving = true;
    public bool moveToTheRight = true;
    public float ptimer = 0f;


    private void Start()
    {
        InvokeRepeating("ToggleIsMoving", 5f, 5f);
    }

    private void ToggleIsMoving()
    {
        if(isMoving)
        {
           // print("Not moving naumore");
            isMoving = false;
        }
        else
        {
           // print("Moving");
            isMoving = true;
        }
    }

    private void FixedUpdate()
    {
        StartMovingPlats();
    }

    private void StartMovingPlats()
    {
        if (isMoving)
        {
            if (!moveToTheRight && transform.position.x > 245f)
            {
                transform.Translate(Vector3.left * speed *3 * Time.deltaTime);
                ptimer = ptimer + Time.deltaTime;
                if(ptimer > 3f)
                {
                    moveToTheRight = true;
                    ptimer = 0f;
                }
            }
            if (moveToTheRight)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }


            //= Vector3.MoveTowards(this.transform.position, transform.forward, -speed * Time.deltaTime);
        }
    }
}

