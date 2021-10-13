using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartItemRowInfo : MonoBehaviour
{
    private CartItem cartItem;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI price;
    public TextMeshProUGUI quantity;

    public Button lowerQuantityButton;

    private void Start() {
        Cart.Instance.OnCartItemRemoved += RemoveSelf;
        Cart.Instance.OnCartItemQuantityUpdated += OnQuantityUpdated;
    }

    public void ChangeQuantity(int change) {
        if(cartItem == null) {
            return;
        }
        Cart.Instance.UpdateItemQuantity(cartItem, change);
    }

    public void RemoveSelf(CartItem item) {
        if(cartItem == item) {
            Destroy(gameObject);
        }
    }

    public void SetItem(CartItem item) {
        cartItem = item;

        itemName.text = cartItem.Name;
        price.text = cartItem.Price.ToString();
        quantity.text = cartItem.Quantity.ToString();
    }

    public void OnQuantityUpdated(CartItem item) {
        if(cartItem == item) {
            quantity.text = item.Quantity.ToString();
            if(item.Quantity <= 0 && lowerQuantityButton.enabled) {
                lowerQuantityButton.enabled = false;
            }
            else if(!lowerQuantityButton.enabled) {
                lowerQuantityButton.enabled = true;
            }
        }
    }
}
