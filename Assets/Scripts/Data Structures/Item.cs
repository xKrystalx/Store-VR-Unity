using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public delegate void ItemUpdate(Item item);
    public ItemUpdate OnItemUpdated;

    public delegate void ImageUpdate(Sprite img);
    public ImageUpdate OnImageUpdated;

    public int id;
    public string sku;
    public float price;
    public string title;
    public string description;

    public GameObject modelPrefab;

    [SerializeReference]
    private Sprite thumbnail;
    public Sprite Thumbnail
    {
        get {
            return thumbnail;
        }
        set {
            thumbnail = value;
            OnImageUpdated?.Invoke(thumbnail);
        }
    }

    [SerializeReference]
    private Product product;
    public Product Product
    {
        get {
            return product;
        }
        set {
            product = value;
        }
    }

    public Item(int itemID, string itemSKU, float itemPrice, string itemTitle, string itemDescription, Sprite thumb = null) {
        id = itemID;
        sku = itemSKU;
        price = itemPrice;
        title = itemTitle;
        description = itemDescription;
        thumbnail = thumb;
    }

    public void UpdateItem(Product product) {
        id = product.productInfo.id;
        sku = product.productInfo.sku;
        price = float.Parse(product.productInfo.price);
        title = product.productInfo.name;
        description = product.productInfo.description;
        Thumbnail = product.thumbnail;

        this.Product = product;

        OnItemUpdated?.Invoke(this);
    }
}