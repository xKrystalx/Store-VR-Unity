using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRescaleTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        other.gameObject.transform.localScale = new Vector3(2, 2, 2);
    }

    private void OnTriggerExit(Collider other) {
        other.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
