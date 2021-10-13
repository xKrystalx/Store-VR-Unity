using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject trackingTarget;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - trackingTarget.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = trackingTarget.transform.position + offset;
    }

    public void SetTrackingTarget(GameObject target) {
        trackingTarget = target;
    }
}
