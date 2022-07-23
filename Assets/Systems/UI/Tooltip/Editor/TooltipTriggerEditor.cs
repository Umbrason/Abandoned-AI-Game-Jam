using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(TooltipTrigger))]
    public class TooltipTriggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject triggerObj = serializedObject;
            SerializedProperty headerContent = triggerObj.FindProperty("tooltipHeader");
            SerializedProperty bodyContent = triggerObj.FindProperty("tooltipBody");

            EditorGUILayout.PropertyField(headerContent);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Tooltip Content", GUILayout.Width(EditorGUIUtility.labelWidth - 2));
            bodyContent.stringValue = EditorGUILayout.TextArea(bodyContent.stringValue, GUILayout.MinWidth(EditorGUIUtility.fieldWidth));
            GUILayout.EndHorizontal();

            triggerObj.ApplyModifiedProperties();
        }


    }
}