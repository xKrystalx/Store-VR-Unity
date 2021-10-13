using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SlotTriggerController : MonoBehaviour
{
    protected bool isTouching;
    protected bool isHolding;
    // Start is called before the first frame update
    void Start()
    {
        isTouching = true;
        isHolding = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag != "product"){
            return;
        }
        isTouching = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform.tag != "product"){
            return;
        }
        isTouching = false;
        Rigidbody obj = other.gameObject.GetComponent<Rigidbody>();
        obj.WakeUp();
        obj.isKinematic = false;
    }

    private void OnTriggerStay(Collider other){
        if(other.transform.tag != "product" || other.gameObject.GetComponent<Rigidbody>().isKinematic){
            return;
        }
        // other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if(!other.gameObject.GetComponent<InteractionsController>().isHeld){
            Rigidbody obj = other.gameObject.GetComponent<Rigidbody>();
            obj.velocity = Vector3.zero;
            obj.angularVelocity = Vector3.zero;
            obj.Sleep();
            obj.isKinematic = true;
        }
    }
}
