using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartCanvasController : MonoBehaviour
{

    public GameObject itemRowPrefab;

    public GameObject itemSection;

    private void Start() {
        if (Cart.Instance) {
            Cart.Instance.OnCartItemAdded += CreateItem;
            Cart.Instance.OnCartCleared += ClearChildren;
        }
        ClearChildren();
    }

    private void CreateItem(CartItem item) {
        GameObject newRow = Instantiate(itemRowPrefab, itemSection.transform);
        newRow.GetComponent<CartItemRowInfo>().SetItem(item);
    }

    private void ClearChildren() {
        foreach(Transform child in itemSection.transform) {
            Destroy(child.gameObject);
        }
    }
}
