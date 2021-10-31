using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartTotalSection : MonoBehaviour
{
    public TextMeshProUGUI totalPrice;

    // Start is called before the first frame update
    void Start()
    {
        if (Cart.Instance) {
            Cart.Instance.OnCartChanged += UpdateTotal;
        }
    }

    public void UpdateTotal(List<CartItem> data) {
        float total = 0.0f;
        foreach(CartItem item in data) {
            total += item.Price * item.Quantity;
        }
        totalPrice.text = total.ToString();
    }
}
