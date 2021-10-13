using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartItem
{
    private readonly int _id;
    private readonly string _sku;
    private readonly string _name;

    public int Id
    {
        get {
            return _id;
        }
    }

    public string Name
    {
        get {
            return _name;
        }
        set {

        }
    }
    private int _quantity;
    public int Quantity
    {
        get {
            return _quantity;
        }
        set {
            if (value <= 0) {
                value = 0;
            }
            _quantity = value;
        }
    }

    private float _price;

    public float Price
    {
        get {
            return _price;
        }
        set {
            if(value <= 0.0f) {
                value = 0;
            }
            _price = value;
        }
    }

    public CartItem(int id, string sku, string name, int quantity, float price) {
        _id = id;
        _sku = sku;
        _name = name;
        Quantity = quantity;
        Price = price;
    }
}
