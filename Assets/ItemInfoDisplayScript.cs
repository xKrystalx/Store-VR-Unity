using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDisplayScript : MonoBehaviour
{
    public GameObject itemObject;
    protected Item item;
    // Start is called before the first frame update
    void Start()
    {
        item = itemObject.GetComponent<ItemPropertiesScript>().GetItem();
        if(itemObject == null || item == null){
            return;
        }
        
        if(item == null){
            return;
        }
        GameObject canvas = gameObject.transform.Find("ProductCanvas").gameObject;
        canvas.transform.GetChild(0).gameObject.GetComponent<Text>().text = item.title.ToString();
        canvas.transform.GetChild(1).gameObject.GetComponent<Text>().text = item.description.ToString();
        canvas.transform.GetChild(2).gameObject.GetComponent<Text>().text = item.price.ToString() + " PLN";
    }

    // Update is called once per frame
    void Update()
    {
        //if (item != null && item.isUpdateRequired) {
        //    GameObject canvas = gameObject.transform.Find("ProductCanvas").gameObject;
        //    Item item = itemObject.GetComponent<ItemPropertiesScript>().GetItem();
        //    canvas.transform.GetChild(0).gameObject.GetComponent<Text>().text = item.title;
        //    canvas.transform.GetChild(1).gameObject.GetComponent<Text>().text = item.description;
        //    canvas.transform.GetChild(2).gameObject.GetComponent<Text>().text = item.price.ToString() + " PLN";
        //    itemInfo.isUpdateRequired = false;
        //}
    }
}
