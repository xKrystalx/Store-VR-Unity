using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;
using System;

public class KeyboardScript : MonoBehaviour
{
    protected TMP_InputField inputField;
    string text = "";
    static bool keyboardShowing;
    static KeyboardScript activeKeyboard = null;
    public bool minimalMode;
    void OnEnable() {
        SteamVR_Events.System(EVREventType.VREvent_KeyboardCharInput).Listen(OnKeyboard);
        SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Listen(OnKeyboardClosed);
        SteamVR_Events.System(EVREventType.VREvent_KeyboardDone).Listen(OnKeyboardDone);
    }

    private void OnDestroy() {
        SteamVR_Events.System(EVREventType.VREvent_KeyboardCharInput).Remove(OnKeyboard);
        SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Remove(OnKeyboardClosed);
        SteamVR_Events.System(EVREventType.VREvent_KeyboardDone).Remove(OnKeyboardDone);
    }

    private void OnKeyboard(VREvent_t args) {
        if(activeKeyboard != this) {
            return;
        }

        VREvent_Keyboard_t keyboard = args.data.keyboard;
        string input = keyboard.cNewInput;

        if (minimalMode) {
            if (input == "\b") {
                if (text.Length > 0) {
                    text = text.Substring(0, text.Length - 1);
                }
            }
            else if (input == "\x1b") {
                // Close the keyboard
                var vr = SteamVR.instance;
                vr.overlay.HideKeyboard();
                keyboardShowing = false;
            }
            else {
                text += input;
            }
            inputField.text = text;
        }
        else {
            System.Text.StringBuilder textBuilder = new System.Text.StringBuilder(1024);
            uint size = SteamVR.instance.overlay.GetKeyboardText(textBuilder, 1024);
            text = textBuilder.ToString();
            inputField.text = text;
        }
    }

    private void OnKeyboardClosed(VREvent_t args) {
        if(activeKeyboard != this) {
            return;
        }
        keyboardShowing = false;
        activeKeyboard = null;
    }

    private void OnKeyboardDone(VREvent_t args) {
    }

    public void ShowKeyboard(TMP_InputField target, string description, bool isPassword = false) {
        Debug.Log("Showing");
        if (!keyboardShowing) {
            keyboardShowing = true;
            activeKeyboard = this;

            inputField = target;
            int inputMode = isPassword ? (int)EGamepadTextInputMode.k_EGamepadTextInputModePassword : (int)EGamepadTextInputMode.k_EGamepadTextInputModeNormal;
            int lineMode = (int)EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine;

            SteamVR.instance.overlay.ShowKeyboard(inputMode, lineMode, Convert.ToUInt32(minimalMode), description, 256, target.text, 0);

            target.ActivateInputField();
        }
    }
}
