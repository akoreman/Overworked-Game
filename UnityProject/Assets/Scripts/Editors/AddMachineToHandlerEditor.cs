using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AddMachineToHandler))]
[CanEditMultipleObjects]

public class AddMachineToHandlerEditor : Editor
{
    SerializedProperty placementPosition;
    SerializedProperty interactionType;
    SerializedProperty interactionTime;
    SerializedProperty outputObject;
    SerializedProperty hasOutput;
    SerializedProperty destroyMachineOnCompletion;
    SerializedProperty hasAnimation;
    SerializedProperty animation;


    void OnEnable()
    {
        placementPosition = serializedObject.FindProperty("placementPosition");
        interactionType = serializedObject.FindProperty("interactionType");
        interactionTime = serializedObject.FindProperty("interactionTime");

        hasOutput = serializedObject.FindProperty("hasOutput");
        outputObject = serializedObject.FindProperty("outputObject");

        hasAnimation = serializedObject.FindProperty("hasAnimation");
        animation = serializedObject.FindProperty("animation");

        destroyMachineOnCompletion = serializedObject.FindProperty("destroyMachineOnCompletion");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(placementPosition);
        EditorGUILayout.PropertyField(interactionType);
        EditorGUILayout.PropertyField(interactionTime);
        
        EditorGUILayout.PropertyField(hasOutput);

        if (hasOutput.boolValue)
        {
            EditorGUILayout.PropertyField(outputObject);
        }

        EditorGUILayout.PropertyField(hasAnimation);



        EditorGUILayout.PropertyField(destroyMachineOnCompletion);

        serializedObject.ApplyModifiedProperties();
    }
}