using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TrashTriggerController : MonoBehaviour
{

    public string productTag;
    public string modelTag;
    [Space]
    [Header("To slow down draw the curve from 1 to 0. Sets Position as well")]
    [Tooltip("Velocity and Position Curve")]
    public AnimationCurve velocityCurve;
    public float animationTime = 0.0f;

    private Vector3 initialVelocity;
    private Vector3 initialAngularVelocity;
    private Vector3 initialEntryPosition;

    private GameObject currentObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        GameObject target = other.transform.parent?.gameObject;
        if(target == null || target.transform.tag != productTag){
            return;
        }
        if(target.GetComponentInParent<Hand>() != null) {
            return;
        }
        if(currentObject != target.gameObject) {
            Destroy(currentObject);
        }
        if (other.gameObject.tag.Contains(modelTag)) {
            Destroy(other.gameObject);
        }
        animationTime = 0.0f;
        currentObject = target.gameObject;
        initialVelocity = target.GetComponent<Rigidbody>().velocity;
        initialAngularVelocity = target.GetComponent<Rigidbody>().angularVelocity;
        initialEntryPosition = target.transform.position;
    }

    private void OnTriggerStay(Collider other) {
        GameObject target = other.transform.parent?.gameObject;
        if (target == null || target.tag != productTag) {
            return;
        }
        if (target.GetComponentInParent<Hand>() != null) {
            return;
        }
        Rigidbody rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody.velocity == Vector3.zero && rigidbody.angularVelocity == Vector3.zero) {
            return;
        }
        if(animationTime >= 10000.0f) {
            animationTime = 0.0f;
        }
        animationTime += Time.deltaTime;
        float velocityMultiplier = Mathf.Lerp(0.0f, 1.0f, velocityCurve.Evaluate(animationTime));

        if(velocityMultiplier <= 0.0f) {
            rigidbody.useGravity = false;
        }
        rigidbody.velocity = initialVelocity * velocityMultiplier;
        rigidbody.angularVelocity = initialAngularVelocity * velocityMultiplier;

        //Curve is from 1 to 0, so have to reverse the lerp B -> A, rather than usual 0 -> 1 or A -> B

        target.transform.position = Vector3.Lerp(transform.position, initialEntryPosition, velocityCurve.Evaluate(animationTime));
    }

}
