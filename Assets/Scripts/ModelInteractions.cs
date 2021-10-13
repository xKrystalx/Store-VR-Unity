using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ModelInteractions : MonoBehaviour
{
    bool isHeld = false;
    int initialLayer;

    // Start is called before the first frame update
    void Start()
    {
        initialLayer = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnAttachedToHand(Hand hand) {
        isHeld = true;
        initialLayer = this.gameObject.layer;
        this.gameObject.layer = Player.instance.gameObject.layer;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    protected virtual void OnDetachedFromHand(Hand hand) {
        isHeld = false;
        this.gameObject.layer = initialLayer;
    }
}
