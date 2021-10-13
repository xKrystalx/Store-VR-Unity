using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OrderController : MonoBehaviour
{
    public enum OrderState {
        Processing,
        Completed,
        Error,
    }

    public string orderUrl = "localhost:3000/order";

    public delegate void OrderStatus(string message, OrderState state);
    public OrderStatus OnOrderStatusChanged;

    
    public void PlaceOrder() {
        StartCoroutine(OrderRequest());
    }

    IEnumerator OrderRequest() {
        string status = "";

        OrderState state = OrderState.Processing;

        if (Cart.Instance.cart.Count <= 0) {
            state = OrderState.Error;
            status = "Cart is empty";
            OnOrderStatusChanged?.Invoke(status, state);
            yield break;
        }

        status = "Processing...";

        OnOrderStatusChanged?.Invoke(status, state);

        WWWForm form = new WWWForm();
        Cart.Instance.CreateJsonCart();
        Debug.Log(JsonUtility.ToJson(Cart.Instance.jsonCart));
        form.AddField("cart", JsonUtility.ToJson(Cart.Instance.jsonCart));

        UnityWebRequest www = UnityWebRequest.Post(orderUrl, form);
        www.SetRequestHeader("Authorization", "Bearer " + User.Instance.token);
        yield return www.SendWebRequest();

        if(www.responseCode == 500) {
            status = www.downloadHandler.text;
            state = OrderState.Error;
        }
        else if (www.result != UnityWebRequest.Result.Success) {
            status = "Error: " + www.error;
            state = OrderState.Error;
        }
        else {
            status = "Order placed!";
            state = OrderState.Completed;
            Cart.Instance.ClearCart();
        }
        
        OnOrderStatusChanged?.Invoke(status, state);
        yield break;
    }
}
