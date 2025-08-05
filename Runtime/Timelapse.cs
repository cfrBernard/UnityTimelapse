using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Timelapse : MonoBehaviour
{
    // --- Global settings ---
    [Tooltip("Total duration of the cycle in seconds.")]
    public float cycleDuration = 60f;

    [Tooltip("Curve controlling the global speed of the timelapse. X from 0 to 1 (time progress), Y from 0 to 1 (adjusted progress).")]
    public AnimationCurve speedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Tooltip("Restart cycle when reaching the end.")]
    public bool loop = true;

    // --- Directional Light --- 
    public bool enableSun = true;
    public Light sun;
    public Vector3 sunRotationStart = new Vector3(180f, -30f, 0f);
    public Vector3 sunRotationEnd = new Vector3(0f, -30f, 0f);

    // --- Clouds ---
    public bool enableClouds = true;
    public Volume volume;
    public Vector3 cloudOffsetStart = Vector3.zero;
    public Vector3 cloudOffsetEnd = new Vector3(100f, 0f, 0f);

    // --- Water ---
    public bool enableWater = false;
    public WaterSurface water;

    [Tooltip("Fixed multiplier for water simulation speed during timelapse.")]
    public float waterTimeMultiplier = 2f;

    // --- Internals ---
    private VolumetricClouds clouds;
    private float timer = 0f;

    private Vector3 initialCloudOffset;
    private float initialWaterMultiplier;

    void Start()
    {
        // --- Clouds ---
        if (enableClouds && volume != null && volume.profile.TryGet(out clouds))
        {
            initialCloudOffset = clouds.shapeOffset.value;
        }
        else if (enableClouds)
        {
            Debug.LogWarning("[Timelapse] Volumetric Clouds reference missing.");
        }

        // --- Water ---
        if (enableWater && !loop && water != null)
        {
            initialWaterMultiplier = water.timeMultiplier;
            water.timeMultiplier = waterTimeMultiplier;
        }
        else if (enableWater && loop)
        {
            Debug.LogWarning("[Timelapse] Water is disabled when Loop is active.");
        }
        else if (enableWater)
        {
            Debug.LogWarning("[Timelapse] WaterSurface reference missing.");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float rawT = Mathf.Clamp01(timer / cycleDuration);
        float t = speedCurve.Evaluate(rawT);

        // --- Directional Light ---
        if (enableSun && sun != null)
        {
            Quaternion startRot = Quaternion.Euler(sunRotationStart);
            Quaternion endRot = Quaternion.Euler(sunRotationEnd);
            sun.transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
        }

        // --- Clouds ---
        if (enableClouds && clouds != null)
        {
            Vector3 offset = Vector3.Lerp(cloudOffsetStart, cloudOffsetEnd, t);
            clouds.shapeOffset.value = initialCloudOffset + offset;
        }

        // --- Water ---
        if (!loop && enableWater && water != null)
        {
            water.timeMultiplier = waterTimeMultiplier;
        }

        // --- Reset cycle ---
        if (loop && timer >= cycleDuration)
        {
            timer = 0f;
        }
        else if (!loop && timer >= cycleDuration)
        {
            if (enableWater && water != null)
                water.timeMultiplier = initialWaterMultiplier;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Timelapse))]
    public class TimelapseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Timelapse script = (Timelapse)target;

            // --- Global settings ---
            EditorGUILayout.LabelField("Global Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cycleDuration"));
            // Dropdown Presets
            string[] presetNames = SpeedCurvePresets.Presets.Select(p => p.name).ToArray();
            int selectedPreset = EditorGUILayout.Popup("Speed Curve Preset", -1, presetNames);
            if (selectedPreset >= 0)
            {
                script.speedCurve = SpeedCurvePresets.Presets[selectedPreset].curve;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedCurve"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));
            EditorGUILayout.Space();

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
#endif
}
