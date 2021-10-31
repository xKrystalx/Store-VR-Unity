using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;
using Debug = Sisus.Debugging.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SlotEvent : UnityEvent<GameObject> {

}

public class SlotController : MonoBehaviour {

    public enum Modes {
        Slot,
        DisplaySlot
    };

    [Tooltip("DisplaySlot only used for displaying models, Slot for whole items with info and previews")]
    public Modes mode = Modes.Slot;
    public GameObject currentItem;

    public bool snapToBottom = false;

    public List<string> tagsToAccept;

    public SlotEvent TriggerEnter;
    public SlotEvent TriggerExit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        bool valid = false;
        GameObject target = null;
        if (mode == Modes.Slot) {
            target = other.transform.parent?.gameObject;
        }
        else if(mode == Modes.DisplaySlot) {
            target = other.gameObject;
        }
        if(target == null) {
            return;
        }
        foreach(string tag in tagsToAccept) {
            if (target.tag == tag) {
                valid = true;
                break;
            }
        }

        if (!valid) {
            return;
        }
        if (mode == Modes.Slot) {
            if (!target.GetComponentInParent<ItemPropertiesScript>() || target.GetComponentInParent<Hand>() != null || currentItem) {
                return;
            }
            currentItem = target;
            currentItem.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        //Model itself checked for collision

        if (mode == Modes.DisplaySlot) {
            if (!target.GetComponent<ModelProperties>() || target.GetComponentInParent<Hand>() != null) {
                return;
            }
            Destroy(currentItem);
            currentItem = target;
            currentItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        //Debug.Log("[Event][Slot] Trigger Enter");
        currentItem.transform.SetParent(transform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

        TriggerEnter.Invoke(currentItem);
        

        if (snapToBottom) {
            Helpers.SnapToBoundInternal(Helpers.Sides.BOTTOM, currentItem, gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
    }

    private void OnTriggerExit(Collider other) {
        GameObject target = null;
        if (mode == Modes.Slot) {
            target = other.transform.parent?.gameObject;
        }
        else if (mode == Modes.DisplaySlot) {
            target = other.gameObject;
        }
        if(target == null) {
            return;
        }
        if (mode == Modes.Slot) {
            if (!target.GetComponentInParent<ItemPropertiesScript>() || currentItem != target) {
                return;
            }
        }

        if (mode == Modes.DisplaySlot) {
            if (!target.GetComponent<ModelProperties>() || currentItem != target) {
                return;
            }
        }

        if (mode == Modes.Slot) {
            if (target == currentItem) {
                currentItem.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.None;
                TriggerExit.Invoke(currentItem);
                currentItem = null;
            }
        }
        if (mode == Modes.DisplaySlot) {
            if (target == currentItem) {
                currentItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                TriggerExit.Invoke(currentItem);
                currentItem = null;
            }
        }

    }
}