using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;
using System;

public class KeyboardScript : MonoBehaviour
{
    protected TMP_InputField textEntry;
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
        Debug.Log("yo");
        if(activeKeyboard != this) {
            return;
        }

        VREvent_Keyboard_t keyboard = args.data.keyboard;
        byte[] inputBytes = new byte[] { keyboard.cNewInput0, keyboard.cNewInput1, keyboard.cNewInput2, keyboard.cNewInput3, keyboard.cNewInput4, keyboard.cNewInput5, keyboard.cNewInput6, keyboard.cNewInput7 };
        int len = 0;
        for (; inputBytes[len] != 0 && len < 7; len++) ;
        string input = System.Text.Encoding.UTF8.GetString(inputBytes, 0, len);

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
            textEntry.text = text;
        }
        else {
            textEntry.text = text;
            System.Text.StringBuilder textBuilder = new System.Text.StringBuilder(1024);
            uint size = SteamVR.instance.overlay.GetKeyboardText(textBuilder, 1024);
            text = textBuilder.ToString();
            textEntry.text = text;
        }
    }

    private void OnKeyboardClosed(VREvent_t args) {
        if(activeKeyboard != this) {
            return;
        }
        keyboardShowing = false;
        activeKeyboard = null;

        Debug.Log("KeyboardClosed");
    }

    private void OnKeyboardDone(VREvent_t args) {
        Debug.Log("KeyboardDone");
    }

    public void ShowKeyboard(TMP_InputField target, string description, bool isPassword = false) {
        if (!keyboardShowing) {
            keyboardShowing = true;
            activeKeyboard = this;

            textEntry = target;
            int inputMode = isPassword ? (int)EGamepadTextInputMode.k_EGamepadTextInputModePassword : (int)EGamepadTextInputMode.k_EGamepadTextInputModeNormal;
            int lineMode = (int)EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine;

            SteamVR.instance.overlay.ShowKeyboard(inputMode, lineMode, Convert.ToUInt32(minimalMode), description, 256, target.text, 0);

            target.ActivateInputField();
        }
    }
}
