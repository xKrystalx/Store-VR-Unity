using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product
{
    public ProductInfo productInfo;
    public Sprite thumbnail;

    public Product(ProductInfo info, Sprite thumb = null) {
        productInfo = info;
        thumbnail = thumb;
    }
}
