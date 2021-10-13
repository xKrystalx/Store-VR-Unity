using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FreezeOnAngles : MonoBehaviour
{
    public bool freeze = true;
    CircularDrive circularDrive = null;
    // Start is called before the first frame update
    void Start()
    {
        if(circularDrive == null) {
            circularDrive = GetComponent<CircularDrive>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(circularDrive == null) {
            return;
        }
        float adjustedAngle = 0.0f;
        if (transform.localEulerAngles[(int)circularDrive.axisOfRotation] > 180.0f) {
            adjustedAngle = transform.localEulerAngles[(int)circularDrive.axisOfRotation] - 360.0f;
        }

        if(adjustedAngle <= circularDrive.minAngle) {
            adjustedAngle = circularDrive.minAngle;
        }

        if(adjustedAngle >= circularDrive.maxAngle) {
            adjustedAngle = circularDrive.maxAngle;
        }

        Vector3 temp = transform.localRotation.eulerAngles;
        temp[(int)circularDrive.axisOfRotation] = adjustedAngle;
        transform.localRotation = Quaternion.Euler(temp);

        circularDrive.outAngle = adjustedAngle;
    }
}
