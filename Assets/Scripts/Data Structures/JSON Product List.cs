using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductInfo {
    public int id;
    public string sku;
    public string name;
    public string description;
    public string short_description;
    public float price;
    public bool featured;
    public List<ProductImage> images;
    public List<ProductCategory> categories;
    public List<ProductTag> tags;
}

[System.Serializable]
public class ProductImage {
    public int id;
    public string src;
}

[System.Serializable]
public class ProductCategory {
    public int id;
    public string name;
}

[System.Serializable]
public class ProductTag {
    public int id;
    public string name;
}

[System.Serializable]
 public class ProductsList {
     public List<ProductInfo> products;
 }