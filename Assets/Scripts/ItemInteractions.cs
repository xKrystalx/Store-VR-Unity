using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Debug = Sisus.Debugging.Debug;

public class ItemInteractions : MonoBehaviour
{
    public SteamVR_Action_Boolean toggleDisplayAction;

    private Interactable interactable;
    private void Start() {
    }

    private void Update() {

    }

    private void ToggleDisplay(SteamVR_Action_Boolean action, SteamVR_Input_Sources source) {
        //Debug.Log("[Controllers][ItemInteractions] ToggleDisplay Triggered.");
        GetComponent<ItemPropertiesScript>().ToggleDisplayState();
    }


    protected virtual void OnAttachedToHand(Hand hand) {
        if (toggleDisplayAction != null) {
            toggleDisplayAction.AddOnStateDownListener(ToggleDisplay, hand.handType);
        }

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    protected virtual void OnDetachedFromHand(Hand hand) {
        if (toggleDisplayAction != null) {
            toggleDisplayAction.RemoveOnStateDownListener(ToggleDisplay, hand.handType);
        }
    }
}
