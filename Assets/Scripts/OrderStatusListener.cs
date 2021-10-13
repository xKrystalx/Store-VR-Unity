using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderStatusListener : MonoBehaviour
{
    public OrderController orderController;

    private void Start() {
        if(orderController == null) {
            return;
        }
        orderController.OnOrderStatusChanged += OnStatusChanged;
    }

    private void OnStatusChanged(string status, OrderController.OrderState state) {
        GetComponent<TMPro.TextMeshProUGUI>().text = status;
    }
}
