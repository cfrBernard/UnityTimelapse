using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(Timelapse))]
public class TimelapseEditor : Editor
{
    private bool showAdvanced = false;
    
    public override void OnInspectorGUI()
    {
        Timelapse script = (Timelapse)target;

        // --- Global settings ---
        EditorGUILayout.LabelField("Global Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cycleDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startKey"));

        // --- Advanced Properties (Foldout) ---
        showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced Properties");
        if (showAdvanced)
        {
            EditorGUI.indentLevel++;

            // Dropdown Presets
            string[] presetNames = SpeedCurvePresets.Presets.Select(p => p.name).ToArray();
            int selectedPreset = EditorGUILayout.Popup("Speed Curve Preset", -1, presetNames);
            if (selectedPreset >= 0)
            {
                script.speedCurve = SpeedCurvePresets.Presets[selectedPreset].curve;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedCurve"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));

            EditorGUI.indentLevel--;
        }

        // --- Directional Light ---
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Directional Light", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enableSun"));
        if (script.enableSun)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sun"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sunRotationStart"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sunRotationEnd"));
        }

        // --- Clouds ---
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Volumetric Clouds", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enableClouds"));
        if (script.enableClouds)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("volume"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cloudOffsetStart"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cloudOffsetEnd"));
        }

        // --- Water ---
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Water Surface", EditorStyles.boldLabel);
        if (!script.loop)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enableWater"));
            if (script.enableWater)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("water"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("waterTimeMultiplier"));
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Water control is disabled when Loop is active.", MessageType.Info);
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}
