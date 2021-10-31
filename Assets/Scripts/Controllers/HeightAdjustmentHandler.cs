using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeightAdjustmentHandler : MonoBehaviour
{

    public TMPro.TextMeshProUGUI textComponent;
    public PlayerController playerController;

    private float heightValue = 1.80f;

    // Start is called before the first frame update
    void Start()
    {
        heightValue = playerController.DefaultHeight;
        textComponent.text = heightValue.ToString("0.00") + " m";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeHeight(float value) {
        heightValue += value;
        textComponent.text = heightValue.ToString("0.00") + " m";
    }

    public void updateHeight() {
        playerController.UpdatePlayermodelHeight(heightValue);
    }
}
