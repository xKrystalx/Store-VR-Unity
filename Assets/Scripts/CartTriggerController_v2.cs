using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CartTriggerController_v2 : MonoBehaviour
{
    public delegate void CartTriggerChange(CartObjectData data);

    public string productTag = "product_item";

    [Tooltip("Slow down over time")]
    public AnimationCurve velocityCurve;

    private List<CartObjectData> _cartObjects;

    private float _animationTime = 0.0f;

    private void Start() {
        _cartObjects = new List<CartObjectData>();
        Cart.Instance.OnCartCleared += ClearCart;
    }

    private void OnTriggerEnter(Collider other) {
        GameObject target = other.transform.parent?.gameObject;
        if (target == null || target.tag != productTag && target.GetComponentInParent<Hand>() != null) {
            return;
        }
        Rigidbody rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody == null) {
            return;
        }
        foreach (CartObjectData cartObj in _cartObjects) {
            if(target.GetComponent<ItemPropertiesScript>().GetProduct().productInfo.id == cartObj.obj.GetComponent<ItemPropertiesScript>().GetProduct().productInfo.id) {

                ChangeQuantity(cartObj, 1);

                Destroy(target.gameObject);
                return;
            }
        }
        ItemPropertiesScript properties = target.GetComponent<ItemPropertiesScript>();
        ProductInfo productInfo = properties.GetProduct().productInfo;
        CartItem cartItem = new CartItem(productInfo.id, productInfo.sku, productInfo.name, 1, productInfo.price);
        Cart.Instance.AddItem(cartItem);

        CartObjectData newItem = new CartObjectData(target, cartItem);
        _cartObjects.Add(newItem);
    }

    private void OnTriggerStay(Collider other) {
        GameObject target = other.transform.parent?.gameObject;
        if (target == null || target.tag != productTag && target.GetComponentInParent<Hand>() != null) {
            return;
        }
        Rigidbody rigidbody = target.GetComponent<Rigidbody>();
        if(rigidbody == null) {
            return;
        }
        if(rigidbody.velocity == Vector3.zero && rigidbody.angularVelocity == Vector3.zero) {
            return;
        }
        foreach(CartObjectData data in _cartObjects) {
            if (data.obj == target) {
                data.animationTime += Time.deltaTime;
                float velocityMultiplier = Mathf.Lerp(0.0f, 1.0f, velocityCurve.Evaluate(data.animationTime));;
                if (velocityMultiplier <= 0.0f) {
                    rigidbody.useGravity = false;
                }
                rigidbody.velocity = data.initialVelocity * velocityMultiplier;
                rigidbody.angularVelocity = data.initialAngularVelocity * velocityMultiplier;
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject target = other.transform.parent?.gameObject;
        if(target == null) {
            return;
        }
        foreach(CartObjectData data in _cartObjects) {
            if(data.obj == target) {
                _cartObjects.Remove(data);
                target.GetComponent<Rigidbody>().useGravity = true;

                Cart.Instance.RemoveItem(data.cartItem);
                return;
            }
        }
    }


    public void ClearCart() {
        foreach(CartObjectData item in _cartObjects.ToArray()) {
            Destroy(item.obj);
            _cartObjects.Remove(item);
        }
    }

    public void ChangeQuantity(CartObjectData item, int change) {
        CartObjectData match = _cartObjects.Find(x => x == item);
        if (match != null) {

            Cart.Instance.UpdateItemQuantity(match.cartItem, change);
        }
    }
}
