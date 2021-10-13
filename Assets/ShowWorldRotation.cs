using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWorldRotation : MonoBehaviour
{
    [ReadOnly]
    public Vector3 worldRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        worldRotation = transform.rotation.eulerAngles;
    }
}
