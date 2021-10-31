using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCanvas : MonoBehaviour {
    public GameObject root;
    public TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI price;
    public Image thumbnail;

    private ItemPropertiesScript properties;
    // Start is called before the first frame update
    void Awake() {
        properties = root.GetComponent<ItemPropertiesScript>();
        properties.OnProductChanged += OnProductChanged;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnProductChanged(Product prev, Product curr) {
        curr.OnItemUpdated += OnItemUpdated;
        curr.OnImageUpdated += OnImageUpdated;
        
        //Unsubscribe from old delegates
        if(prev != null) {
            prev.OnItemUpdated -= OnItemUpdated;
            prev.OnImageUpdated -= OnImageUpdated;
        }

        OnItemUpdated(curr);
    }

    public void OnItemUpdated(Product item) {
        name.text = item.productInfo.name;
        description.text = item.productInfo.description;
        price.text = item.productInfo.price.ToString();
        OnImageUpdated(item.Thumbnail);
    }

    public void OnImageUpdated(Sprite thumb) {
        //Debug.Log("[Event][ItemCanvas] Event Fired: OnImageUpdated");
        thumbnail.sprite = thumb;
    }

    private void OnDestroy() {
        properties.product.OnImageUpdated -= OnImageUpdated;
        properties.product.OnItemUpdated -= OnItemUpdated;
    }
}
