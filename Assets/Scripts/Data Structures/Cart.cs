using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : Singleton<Cart>
{
    public delegate void CartChange(CartItem data);
    public CartChange OnCartItemAdded;
    public CartChange OnCartItemRemoved;
    public CartChange OnCartItemQuantityUpdated;

    public delegate void CartChangeListEvent(List<CartItem> data);
    public CartChangeListEvent OnCartChanged;

    public delegate void CartChangeEventNoData();
    public CartChangeEventNoData OnCartCleared;

    public List<CartItem> cart;
    public JsonCart jsonCart;
    public List<JsonCartItem> jsonList;


    private void Start() {
        cart = new List<CartItem>();
        jsonCart = new JsonCart();
        jsonList = new List<JsonCartItem>();
    }

    public void AddItem(CartItem item) {
        CartItem match = cart.Find(x => x.Id == item.Id);

        if(match != null) {
            UpdateItemQuantity(match, 1);
            return;
        }

        cart.Add(item);
        OnCartItemAdded?.Invoke(item);
        OnCartChanged?.Invoke(cart);
    }

    public void RemoveItem(CartItem item) {

        CartItem match = cart.Find(x => x.Id == item.Id);

        if(match != null) {
            cart.Remove(match);
            OnCartItemRemoved?.Invoke(match);
            OnCartChanged?.Invoke(cart);
        }
    }

    public void ClearCart() {
        jsonList.Clear();
        cart.Clear();

        OnCartCleared?.Invoke();
        OnCartChanged?.Invoke(cart);
    }

    public void UpdateItemQuantity(CartItem item, int change) {
        CartItem match = cart.Find(x => x.Id == item.Id);

        if(match != null) {
            match.Quantity += change;
            OnCartItemQuantityUpdated?.Invoke(match);
            OnCartChanged?.Invoke(cart);
        }
    }

    public void CreateJsonCart() {
        jsonList.Clear();
        foreach(CartItem item in cart) {
            JsonCartItem jsonItem = new JsonCartItem();
            jsonItem.product_id = item.Id;
            jsonItem.quantity = item.Quantity;
            jsonList.Add(jsonItem);
        }
        jsonCart.cart = jsonList;
    }
}
