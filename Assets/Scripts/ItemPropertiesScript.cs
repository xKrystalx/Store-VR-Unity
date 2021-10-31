using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPropertiesScript : MonoBehaviour
{
    public delegate void ProductChange(Product previous, Product current);
    public ProductChange OnProductChanged;

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
    public Product product;


    // Start is called before the first frame update
    void Start()
    {
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

    public Product GetProduct(){
        return product;
    }

    public void SetProduct(Product product) {
        OnProductChanged?.Invoke(this.product, product);
        this.product = product;
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
