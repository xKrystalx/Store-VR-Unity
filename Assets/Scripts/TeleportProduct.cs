using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TeleportProduct : MonoBehaviour
{
    public Transform destination;
    public string productTag = "product_item";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Transform interactable = other.GetComponentInParent<Interactable>()?.transform?? null;
        if (interactable != null && interactable.GetComponentInParent<Hand>() == null && interactable.parent == null && destination != null && interactable.tag == productTag) {
            interactable.position = destination.position;
            Rigidbody rb = interactable.GetComponent<Rigidbody>();
            if(rb != null) {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
