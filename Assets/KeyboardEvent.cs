using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardEvent : MonoBehaviour
{
    public KeyboardScript script;
    public TMPro.TMP_InputField field;
    public string description = "";
    public bool password = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(field == null) {
            field = GetComponent<TMPro.TMP_InputField>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestKeyboard() {
        script.ShowKeyboard(field, description);
    }
}
