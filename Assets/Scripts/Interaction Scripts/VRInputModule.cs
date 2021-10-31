using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
public class VRInputModule : BaseInputModule
{
    public Transform leftPointerParent;
    public Transform rightPointerParent;

    public Pointer pointer;

    public SteamVR_Action_Boolean actionInteract = SteamVR_Actions._default.InteractUI;

    [SerializeField]
    private string m_SubmitButton = "Submit";
    [SerializeField]
    private string m_CancelButton = "Cancel";

    private SteamVR_Input_Sources currentInputSource = SteamVR_Input_Sources.Any;

    public PointerEventData data = null;

    protected override void Start() {

        data = new PointerEventData(eventSystem);
        data.position = new Vector2(pointer.Camera.pixelWidth / 2, pointer.Camera.pixelHeight / 2);

        if(rightPointerParent.GetChild(0) != null) {
            currentInputSource = SteamVR_Input_Sources.RightHand;
        }
        else {
            currentInputSource = SteamVR_Input_Sources.LeftHand;
        }
    }

    private void Update() {

        if (actionInteract.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
            pointer.transform.SetParent(leftPointerParent);
            pointer.transform.localRotation = Quaternion.identity;
            pointer.transform.localPosition = Vector3.zero;
            currentInputSource = SteamVR_Input_Sources.LeftHand;
        }
        else if (actionInteract.GetStateDown(SteamVR_Input_Sources.RightHand)) {
            pointer.transform.SetParent(rightPointerParent);
            pointer.transform.localRotation = Quaternion.identity;
            pointer.transform.localPosition = Vector3.zero;
            currentInputSource = SteamVR_Input_Sources.RightHand;
        }
    }

    public override void Process() {

        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        //if(data.pointerCurrentRaycast.distance > pointer.defaultLength) {
        //    return;
        //}

        HandlePointerExitAndEnter(data, data.pointerCurrentRaycast.gameObject);

        if (actionInteract.GetStateDown(currentInputSource)) {
            Press(data);
        }

        if (actionInteract.GetStateUp(currentInputSource)) {
            Release(data);
        }

        bool usedEvent = SendUpdateEventToSelectedObject();

        if (!usedEvent) {
            SendSubmitEventToSelectedObject();
        }
    }

    public void Press(PointerEventData data) {
        if (data.pointerCurrentRaycast.distance > pointer.defaultLength && pointer.defaultLength > 0) {
            return;
        }

        data.pointerPressRaycast = data.pointerCurrentRaycast;

        data.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(data.pointerPressRaycast.gameObject);

        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerDownHandler);
    }

    public void Release(PointerEventData data) {
        if ((data.pointerCurrentRaycast.distance == 0 || data.pointerCurrentRaycast.distance > pointer.defaultLength) && pointer.defaultLength > 0) {
            if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != data.pointerPressRaycast.gameObject) {
                eventSystem.SetSelectedGameObject(null);
            }
            return;
        }

        GameObject pointerRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(data.pointerCurrentRaycast.gameObject);

        if(data.pointerPress == pointerRelease) {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        data.pointerPress = null;

        data.pointerCurrentRaycast.Clear();
    }

    private bool SendUpdateEventToSelectedObject() {
        if(eventSystem.currentSelectedGameObject == null) {
            return false;
        }

        BaseEventData data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }

    private bool SendSubmitEventToSelectedObject() {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (Input.GetButtonDown(m_SubmitButton))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        if (Input.GetButtonDown(m_CancelButton))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }
}
