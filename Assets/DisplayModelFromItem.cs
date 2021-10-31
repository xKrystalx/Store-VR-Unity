using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayModelFromItem : MonoBehaviour
{
    public GameObject bigSlot;
    public GameObject smallSlot;

    [Tooltip("Tag to look for, when deciding whether to display object in small slot or big slot")]
    public string bigSlotTag = "big";
    // Start is called before the first frame update
    

    public void DisplayItem(GameObject item) {
        ItemPropertiesScript properties = item.GetComponent<ItemPropertiesScript>();

        Product itemObj = properties.GetProduct();

        if (!itemObj.modelPrefab) {
            return;
        }

        foreach (ProductTag tag in itemObj.productInfo.tags) {
            if (tag.name.Contains(bigSlotTag)) {
                GameObject newObj = Instantiate(itemObj.modelPrefab, bigSlot.transform);
                newObj.tag = "product_model_big";
                return;
            }
        }
        GameObject obj = Instantiate(itemObj.modelPrefab, smallSlot.transform);
        obj.tag = "product_model";
    }
}
