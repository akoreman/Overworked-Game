using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddObjectToHandler))]
[CanEditMultipleObjects]

public class AddObjectToHandlerEditor : Editor
{
    SerializedProperty interactionType;

    void OnEnable()
    {
        interactionType = serializedObject.FindProperty("interactionType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(interactionType);
        serializedObject.ApplyModifiedProperties();
    }
}
