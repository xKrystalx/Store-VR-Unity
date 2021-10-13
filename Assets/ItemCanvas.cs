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
    void Start() {
        properties = root.GetComponent<ItemPropertiesScript>();
        if (properties.GetItem() == null) {
            return;
        }
        properties.GetItem().OnItemUpdated += OnItemUpdated;
        properties.GetItem().OnImageUpdated += OnImageUpdated;
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnItemUpdated(Item item) {
        name.text = item.title;
        description.text = item.description;
        price.text = item.price.ToString();
    }

    public void OnImageUpdated(Sprite thumb) {
        //Debug.Log("[Event][ItemCanvas] Event Fired: OnImageUpdated");
        thumbnail.sprite = thumb;
    }
}
