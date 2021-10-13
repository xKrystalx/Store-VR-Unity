using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchProducts : MonoBehaviour
{
    public ProductsController productsController;
    private ShelfController shelfController;
    private TMPro.TMP_InputField inputField = null;
    // Start is called before the first frame update
    void Start()
    {
        shelfController = transform.GetComponentInChildren<ShelfController>();
        inputField = transform.GetComponentInChildren<TMPro.TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Search() {
        if (inputField == null) {
            return;
        }
        string query = inputField.text;
        productsController.SearchProducts(query, result => {
            if (result != null) {
                shelfController.LoadProducts(result);
            }
        });
    }
}
