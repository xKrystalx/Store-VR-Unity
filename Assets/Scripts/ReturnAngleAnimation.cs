using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ReturnAngleAnimation : MonoBehaviour
{
    public CircularDrive circularDrive;
    public LinearMapping linearMapping;
    public AnimationCurve animationCurve;

    private Quaternion _targetAngle;


    private void Start() {
        _targetAngle = transform.localRotation;
        if(circularDrive == null) {
            circularDrive = GetComponent<CircularDrive>();
        }
        if(linearMapping == null) {
            linearMapping = GetComponent<LinearMapping>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnHandHoverBegin(Hand hand) {
        if (!enabled) return;
        StopAllCoroutines();
    }

    protected virtual void OnHandHoverEnd(Hand hand) {
        if (!enabled) return;
        StartCoroutine(Rotate());
    }

    public void StopAnimating() {
        StopAllCoroutines();
    }

    IEnumerator Rotate() {
        float animationTime = 0.0f;
        Vector3 rotationAxis = Vector3.zero;
        rotationAxis[(int)circularDrive.axisOfRotation] = 1.0f;
        bool negative = false;

        if(circularDrive.minAngle < 0 || circularDrive.maxAngle < 0) {
            negative = true;
        }

        while (linearMapping.value < 1) {
            float mappedLinearValue = Mathf.Lerp(linearMapping.value, 1.0f, animationCurve.Evaluate(animationTime));

            float angle = Mathf.Lerp(circularDrive.minAngle, circularDrive.maxAngle, mappedLinearValue);

            Vector3 temp = transform.localRotation.eulerAngles;
            temp[(int)circularDrive.axisOfRotation] = angle;
            circularDrive.outAngle = angle;
            linearMapping.value = mappedLinearValue;
            transform.localRotation = Quaternion.Euler(temp);
            animationTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
