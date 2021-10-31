using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{

    Quaternion initialRotation;
    public AnimationCurve animationCurve;
    private float _animationTime;
    private bool _coroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartAnimation() {
        StopAnimation();
        StartAnimation();
    }

    public void StopAnimation() {
        if (_coroutineRunning) {
            StopCoroutine(Rotate());
            _coroutineRunning = false;
        }
    }

    public void StartAnimation() {
        if (!_coroutineRunning) {
            StartCoroutine(Rotate());
            _coroutineRunning = true;
        }
    }

    public void ResetRotation() {
        transform.localRotation = initialRotation;
    }

    IEnumerator Rotate() {
        _animationTime = 0.0f;
        while (true) {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.Lerp(0.0f, 359.0f, animationCurve.Evaluate(_animationTime)), transform.localRotation.eulerAngles.z);

            _animationTime += Time.deltaTime;
            if (_animationTime >= animationCurve[animationCurve.length-1].time) {
                _animationTime = 0.0f;
            }
            yield return null;
        }
    }
}
