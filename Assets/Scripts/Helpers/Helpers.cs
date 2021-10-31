using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public static class Helpers
{
    public enum Sides {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        FRONT,
        BACK
    }
    public static Vector3 ? SnapToBoundInternal(Sides side, GameObject target, GameObject dest) {
        Vector3 res = Vector3.zero;

        bool initiallyDisabled = false;

        Collider targetCollider = target.GetComponent<Collider>();
        Collider destCollider = dest.GetComponent<Collider>();

        if(targetCollider == null || destCollider == null) {
            return null;
        }

        if (!targetCollider.enabled) {
            targetCollider.enabled = true;
            initiallyDisabled = true;
        }

        Physics.SyncTransforms();

        switch (side) {
            case Sides.TOP: {
                    Vector3 offset = new Vector3(0.0f, targetCollider.bounds.max.y - destCollider.bounds.max.y, 0.0f);
                    res = target.transform.localPosition - offset;
                    break;
                }

            case Sides.BOTTOM: {
                    Vector3 offset = new Vector3(0.0f, targetCollider.bounds.min.y - destCollider.bounds.min.y , 0.0f);
                    res = target.transform.localPosition - offset;

                    break;
                }

            case Sides.LEFT: {
                    Vector3 offset = new Vector3(targetCollider.bounds.max.x - destCollider.bounds.max.x, 0.0f, 0.0f);
                    res = target.transform.localPosition - offset;
                    break;
                }

            case Sides.RIGHT: {
                    Vector3 offset = new Vector3(targetCollider.bounds.min.x - destCollider.bounds.min.x, 0.0f, 0.0f);
                    res = target.transform.localPosition - offset;
                    break;
                }

            case Sides.FRONT: {
                    Vector3 offset = new Vector3(0.0f, 0.0f, targetCollider.bounds.max.z - destCollider.bounds.max.z);
                    res = target.transform.localPosition - offset;
                    break;
                }

            case Sides.BACK: {
                    Vector3 offset = new Vector3(0.0f, 0.0f, targetCollider.bounds.min.z - destCollider.bounds.min.z);
                    res = target.transform.localPosition - offset;
                    break;
                }
        }

        if (initiallyDisabled) {
            targetCollider.enabled = false;
        }

        target.transform.localPosition = res;

        return res;
    }
}
