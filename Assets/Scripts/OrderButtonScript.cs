using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class OrderButtonScript : MonoBehaviour
{
    public OrderController orderController;
    private Interactable button;
    // Start is called before the first frame update
    void Start()
    {
        if(orderController != null) {
            orderController.OnOrderStatusChanged += OnOrderStatusChanged;
        }
        if(button == null) {
            button = GetComponent<Interactable>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonDown(Hand fromHand) {
        ColorSelf(Color.cyan);
        fromHand.TriggerHapticPulse(1000);
    }

    public void OnButtonUp(Hand fromHand) {
        ColorSelf(Color.white);
    }

    private void ColorSelf(Color newColor) {
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++) {
            renderers[rendererIndex].material.color = newColor;
        }
    }

    private void OnOrderStatusChanged(string status, OrderController.OrderState state) {
        if (state == OrderController.OrderState.Processing) {
            if (button != null) {
                button.enabled = false;
            }
        }

        if(state == OrderController.OrderState.Error || state == OrderController.OrderState.Completed) {
            StartCoroutine(DelayButton(5.0f));
        }
    }

    IEnumerator DelayButton(float time) {
        float currentTime = 0.0f;
        while (currentTime < time) {
            currentTime += Time.deltaTime;
            yield return null;
        }
        button.enabled = true;
        yield break;
    }


}
