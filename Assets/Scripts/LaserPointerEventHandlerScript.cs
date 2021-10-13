/* LaserPointerEventHandlerScript.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using Valve.VR;
using TMPro;

public class LaserPointerEventHandlerScript : MonoBehaviour{
    public SteamVR_LaserPointer laserPointer;
    public GameObject keyboardController;
    protected KeyboardScript keyboardScript;

    void Awake(){
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    private void Start() {
        keyboardScript = keyboardController.GetComponent<KeyboardScript>();
    }

    public void PointerClick(object sender, PointerEventArgs e){
        switch(e.target.name){
            case "UsernameField":{
                    //keyboardScript.ShowKeyboard(e.target.GetComponent<TMP_InputField>(), "Username", false);
                    e.target.GetComponent<TMP_InputField>().ActivateInputField();
                break;
            }
            case "PasswordField":{
                    //keyboardScript.ShowKeyboard(e.target.GetComponent<TMP_InputField>(), "Password", true);
                    e.target.GetComponent<TMP_InputField>().ActivateInputField();
                    break;
            }
            case "LoginButton":{
                    Debug.Log(e.target.name);
                    Button button = e.target.GetComponent<Button>();
                    if (button.interactable) {
                        button.onClick.Invoke();
                    }
                break;
            }
            default:{
                return;
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e){
        switch(e.target.name){
            case "UsernameField":{
                break;
            }
            case "PasswordField":{
                break;
            }
            case "LoginButton":{
                break;
            }
            default:{
                return;
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e){
        switch(e.target.name){
            case "UsernameField":{
                break;
            }
            case "PasswordField":{
                break;
            }
            case "LoginButton":{
                break;
            }
            default:{
                return;
            }
        }
    }
}