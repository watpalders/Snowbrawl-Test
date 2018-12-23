using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator animationComp;

    private void Start()
    {
        animationComp = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && this.gameObject.GetComponent<CDClickOn>().currentlySelected == true)
        {
            animationComp.Play("WindUp");
        }

        if (this.gameObject.GetComponent<CDClickOn>().currentlySelected == true && Input.GetMouseButtonUp(0))
        {
            animationComp.Play("Throw");
        }
    }
}
