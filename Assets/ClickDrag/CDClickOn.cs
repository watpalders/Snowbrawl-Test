using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDClickOn : MonoBehaviour {
    [SerializeField]    private Material notSelectedColor;
    [SerializeField]    private Material selectedColor;

    private MeshRenderer myRend;

    [HideInInspector]public bool currentlySelected = false;

    void Start () {
        myRend = GetComponent<MeshRenderer>();
        Camera.main.gameObject.GetComponent<CDClick>().selectableObjects.Add(this.gameObject);
        ClickMe();
    }
	
    public void ClickMe()
    {
        if (currentlySelected == false)
        {
            myRend.material = notSelectedColor;
        }
        else
        {
            myRend.material = selectedColor;
        }
    }
}
