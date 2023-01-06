using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WarUnitEditor : Editor
{
    public void override void OnInspectorGUI()
    {
        WarUnitSet WUS = (WarUnitSet)target;
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("You can reset unit data", MessageType.Error);

        if(GUILayout.Button("Reset Unit", GUILayout.Height(50))
        {
            WUS.UnitTypeProcess();
        }
    }
}
