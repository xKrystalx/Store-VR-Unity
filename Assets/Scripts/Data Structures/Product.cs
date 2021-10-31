using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product
{
    public delegate void ItemUpdate(Product product);
    public ItemUpdate OnItemUpdated;

    public delegate void ImageUpdate(Sprite img);
    public ImageUpdate OnImageUpdated;

    public GameObject modelPrefab;
    public ProductInfo productInfo;

    [SerializeReference]
    private Sprite _thumbnail;
    public Sprite Thumbnail
    {
        get {
            return _thumbnail;
        }
        set {
            _thumbnail = value;
            OnImageUpdated?.Invoke(value);
        }
    }
    public Product() { }
    public Product(ProductInfo info) {
        productInfo = info;
    }

    public void UpdateProductInfo(ProductInfo info) {
        productInfo = info;
        OnItemUpdated?.Invoke(this);
    }
}
