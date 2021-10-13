using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelProperties : MonoBehaviour
{
    public string SKU;
    public ItemPropertiesScript attachedProduct;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProduct(ItemPropertiesScript product) {
        attachedProduct = product;
    }
}
