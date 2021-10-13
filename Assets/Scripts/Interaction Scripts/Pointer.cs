using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    [HideInInspector]
    public float defaultLength = 5.0f;
    public GameObject pointerTip;

    public Camera Camera
    {
        get;
        private set;
    }

    private VRInputModule inputModule;
    private LineRenderer lineRenderer;

    private void Awake() {
        Camera = GetComponent<Camera>();
        Camera.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        defaultLength = lineRenderer.GetPosition(1).z;
    }

    private void Start() {
        inputModule = EventSystem.current.gameObject.GetComponent<VRInputModule>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdatePointerLine();
    }

    private void UpdatePointerLine() {
        if(inputModule == null) {
            return;
        }

        //Get the PointerEventData that matches the hand the pointer is attached to
        PointerEventData pointerData = inputModule.data;

        //RaycastHit hit = CreateRaycast();

        //float colliderDistance = hit.distance == 0 ? defaultLength : hit.distance;
        float canvasDistance = pointerData.pointerCurrentRaycast.distance == 0 ? defaultLength : pointerData.pointerCurrentRaycast.distance;

        if (canvasDistance >= defaultLength && lineRenderer.enabled) {
            lineRenderer.enabled = false;
            pointerTip.SetActive(false);
        }
        else if (canvasDistance < defaultLength && !lineRenderer.enabled) {
            lineRenderer.enabled = true;
            pointerTip.SetActive(true);
        }

        Vector3 pointerEndPosition = transform.position + (transform.forward * canvasDistance);

        pointerTip.transform.position = pointerEndPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, pointerEndPosition);
    }

    //private RaycastHit CreateRaycast() {
    //    RaycastHit hit;
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    Physics.Raycast(ray, out hit, defaultLength);

    //    return hit;
    //}
}
