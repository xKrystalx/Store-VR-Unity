using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartObjectData {
    public GameObject obj;
    public Product item;
    public CartItem cartItem;
    public float animationTime;
    public Vector3 initialVelocity;
    public Vector3 initialAngularVelocity;

    public CartObjectData(GameObject obj, CartItem item) {
        this.obj = obj;
        this.item = obj.GetComponent<ItemPropertiesScript>().GetProduct();
        this.cartItem = item;
        this.animationTime = 0.0f;
        this.initialVelocity = obj.GetComponent<Rigidbody>().velocity;
        this.initialAngularVelocity = obj.GetComponent<Rigidbody>().angularVelocity;
    }
}