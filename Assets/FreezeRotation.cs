using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{

    // Use this for initialization
    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, 90, 0);
    }
}
