using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform lookAtTransform;
    [Header("Ignore Axis")]
    public Axis ignoreAxis = Axis.Y;
    // Start is called before the first frame update
    public enum Axis {
        X,
        Y,
        Z
    }
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(lookAtTransform != null) {
            
            Vector3 temp = lookAtTransform.position;
            temp[(int)ignoreAxis] = transform.position[(int)ignoreAxis];
            transform.LookAt(temp);
        }
        
    }
}
