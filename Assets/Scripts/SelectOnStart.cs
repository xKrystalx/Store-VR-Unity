using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
public class SelectOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SelectField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectField() {
        GetComponent<TMP_InputField>().Select();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SelectOnStart))]
public class SelectEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        SelectOnStart script = target as SelectOnStart;

        if (GUILayout.Button("Click")) {
            script.SelectField();
        }
    }
}
#endif
