using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonCart {
    public List<JsonCartItem> cart;
}

[System.Serializable]
public class JsonCartItem
{
    public int product_id;
    public int quantity;

    //public JsonCartItem(int id, int quantity) {
    //    this.id = id;
    //    this.quantity = quantity;
    //}
}
