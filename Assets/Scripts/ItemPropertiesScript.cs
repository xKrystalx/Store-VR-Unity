using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPropertiesScript : MonoBehaviour
{
    public string initialSKU;
    public bool isUpdateRequired;

    protected Vector3 initialPosition;
    protected Quaternion initialRotation;

    public GameObject modelDisplaySlot;
    public GameObject infoDisplayCanvas;

    private bool state = false;

    public enum DisplayStates {
        INFO,
        MODEL
    }

    [SerializeReference]
    public Item item;


    // Start is called before the first frame update
    void Start()
    {
        if (item == null) {
            item = new Item(0, initialSKU, 0.0f, "", "");
        }
        // GameObject canvas = gameObject.transform.Find("ProductCanvas").gameObject;
        // canvas.transform.GetChild(0).gameObject.GetComponent<Text>().text = itemTitle.ToString();
        // canvas.transform.GetChild(1).gameObject.GetComponent<Text>().text = itemDescription.ToString();
        // canvas.transform.GetChild(2).gameObject.GetComponent<Text>().text = itemPrice.ToString() + " PLN";

        initialPosition = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item GetItem(){
        return item;
    }

    public void UpdateItem(Product product){
        if(item == null) {
            return;
        }
        item.UpdateItem(product);
        isUpdateRequired = true;
    }

    public void ResetPosition(){
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
    }

    public void ToggleDisplayState() {
        state = !state;

        if (state) {
            modelDisplaySlot.SetActive(true);
            modelDisplaySlot.GetComponent<RotateOverTime>().StartAnimation();
            infoDisplayCanvas.SetActive(false);
        }
        else {
            modelDisplaySlot.SetActive(false);
            modelDisplaySlot.GetComponent<RotateOverTime>().StopAnimation();
            infoDisplayCanvas.SetActive(true);
        }
    }
}
