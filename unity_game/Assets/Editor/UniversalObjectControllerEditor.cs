using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UniversalObjectController))]
public class UniversalObjectControllerEditor : Editor
{
    SerializedProperty interactionType;
    SerializedProperty onClick;
    SerializedProperty onHoldStart;
    SerializedProperty onHold;
    SerializedProperty onHoldEnd;

    private void OnEnable() {
        interactionType = serializedObject.FindProperty("interactionType");
        onClick = serializedObject.FindProperty("onClick");
        onHoldStart = serializedObject.FindProperty("onHoldStart");
        onHold = serializedObject.FindProperty("onHold");
        onHoldEnd = serializedObject.FindProperty("onHoldEnd");
    }
    public override void OnInspectorGUI(){
        
        UniversalObjectController controller = (UniversalObjectController)target;

        controller.interactionType = (UniversalObjectController.InteractionType)EditorGUILayout.EnumPopup("Interaction Type", controller.interactionType);

        if (controller.interactionType == UniversalObjectController.InteractionType.Clickable) {
            EditorGUILayout.PropertyField(onClick);
        } else {
            EditorGUILayout.PropertyField(onHoldStart);
            EditorGUILayout.PropertyField(onHold);
            EditorGUILayout.PropertyField(onHoldEnd);
        }

        controller.mouseHoverInfo = EditorGUILayout.TextField("Mouse Hover Info", controller.mouseHoverInfo);

        serializedObject.ApplyModifiedProperties();
    }
}
