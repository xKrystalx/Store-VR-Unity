using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
using Debug = Sisus.Debugging.Debug;

public class ProductsController : MonoBehaviour {
    public delegate void ProductListActivity();
    public ProductListActivity OnProductsLoaded;

    public string apiEndpoint = "http://localhost:3000/products";

    private ProductsList listFromJson;

    private List<Product> list;

    public List<Product> List
    {
        get {
            return list;
        }
        set {
            list = value;
        }
    }

    public void GetProducts(System.Action<bool> callback) {
        StartCoroutine(sendRequest(result => {
            if (result) {
                callback(true);
            }
            else {
                callback(false);
            }
        }));
    }

    IEnumerator sendRequest(System.Action<bool> callback) {

        var request = UnityWebRequest.Get(apiEndpoint);
        request.SetRequestHeader("Authorization", "Bearer " + User.Instance.token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.Log(request.error);
            callback(false);
            yield break;
        }

        var responseString = request.downloadHandler.text;

        listFromJson = new ProductsList();
        listFromJson = JsonUtility.FromJson<ProductsList>(fixJson(responseString));

        Debug.Log(() => responseString);

        list = new List<Product>();

        foreach(ProductInfo productInfo in listFromJson.products) {
            list.Add(new Product(productInfo));
        }

        OnProductsLoaded?.Invoke();
        callback(true);
        yield break;

        /*        if (list.products != null && list.products.Count > 0) {
                    Debug.Log(list.products[0].sku);
                    GameObject[] products = GameObject.FindGameObjectsWithTag("product");
                    foreach (GameObject product in products) {
                        var temp = product.GetComponent<ItemPropertiesScript>();
                        var tempItem = temp.GetItem();
                        foreach (var item in list.products) {
                            if (tempItem.id == Convert.ToInt32(item.sku)) {
                                temp.UpdateItem(Convert.ToInt32(item.sku), Convert.ToSingle(item.price), item.name, item.description);
                                break;
                            }
                        }
                        cartController.GetComponent<CartControllerScript>().UpdateCart();
                    }
                }*/
    }

    public void SearchProducts(string query, System.Action<List<Product>> callback) {
        StartCoroutine(SearchProductsCoroutine(query, result => {
            if (result != null) {
                callback(result);
            }
        }));
    }

    IEnumerator SearchProductsCoroutine(string query, System.Action<List<Product>> callback) {
        List<Product> searchResult = new List<Product>();

        if(query == "") {
            callback(list);
            yield break;
        }

        foreach (Product product in list) {
            if (product.productInfo.name.Contains(query, StringComparison.CurrentCultureIgnoreCase)) {
                searchResult.Add(product);
                continue;
            }
            foreach(ProductCategory category in product.productInfo.categories) {
                if(category.name.Contains(query, StringComparison.CurrentCultureIgnoreCase)) {
                    searchResult.Add(product);
                    continue;
                }
            }
            foreach(ProductTag tag in product.productInfo.tags) {
                if(tag.name.Contains(query, StringComparison.CurrentCultureIgnoreCase)){
                    searchResult.Add(product);
                    continue;
                }
            }
        }

        callback(searchResult);
        yield break;
    }

    public void GetProductImage(string src, System.Action<Texture2D> callback) {
        StartCoroutine(DownloadImage(src, result => {
            if (result) {
                callback(result);
            }
        }));

    }

    IEnumerator DownloadImage(string src, System.Action<Texture2D> callback) {
        Debug.Log("[Download Manager][In Progress] Downloading image: %s", src);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(src);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            callback(myTexture);
        }
    }

    string fixJson(string value) {
        value = "{\"products\":" + value + "}";
        return value;
    }
}