using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.InputSystem;

public class Timelapse : MonoBehaviour
{
    // --- Global settings ---
    [Tooltip("Total duration of the cycle in seconds.")]
    public float cycleDuration = 60f;

    [Tooltip("Curve controlling the global speed of the timelapse. X from 0 to 1 (time progress), Y from 0 to 1 (adjusted progress).")]
    public AnimationCurve speedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Tooltip("Input key to start the timelapse.")]
    public Key startKey = Key.Space;

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
    public string selectedPresetName = "";
    
    private VolumetricClouds clouds;
    private float timer = 0f;
    private float lastCurveValue = 0f;

    private Vector3 initialCloudOffset;
    private float initialWaterMultiplier;

    private bool hasStarted = false;

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
        if (Keyboard.current != null && Keyboard.current[startKey].wasPressedThisFrame)
        {
            if (!hasStarted)
            {
                StartTimelapse();
            }
            else
            {
                ResetTimelapse();
            }
        }
    
        if (hasStarted)
        {
            RunTimelapse();
        }
    }
    
    private void StartTimelapse()
    {
        hasStarted = true;
        timer = 0f;
        lastCurveValue = 0f;
        Debug.Log("[Timelapse] Started");
    }

    private void ResetTimelapse()
    {
        hasStarted = false;
        if (enableWater && water != null)
        {
            water.timeMultiplier = initialWaterMultiplier;
        }
        Debug.Log("[Timelapse] Reset");
    }

    private void RunTimelapse()
    {
        timer += Time.deltaTime;
        float rawT = Mathf.Clamp01(timer / cycleDuration);
        float curveValue = speedCurve.Evaluate(rawT);

        // Progress for position
        float t = curveValue;

        // Derivative for speed
        float curveSpeed = (curveValue - lastCurveValue) / Time.deltaTime;
        lastCurveValue = curveValue;

        // --- Directional Light ---
        if (enableSun && sun != null)
        {
            float angle = Mathf.Lerp(sunRotationStart.x, sunRotationEnd.x, t);
            sun.transform.localRotation = Quaternion.Euler(angle, sunRotationStart.y, sunRotationStart.z);
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
            if (Mathf.Approximately(timer, 0f))
            {
                water.timeMultiplier = waterTimeMultiplier;
            }

            float targetSpeed = waterTimeMultiplier * curveSpeed;
            water.timeMultiplier = Mathf.Max(initialWaterMultiplier, targetSpeed);
        }

        // --- Reset cycle ---
        if (loop && timer >= cycleDuration)
        {
            timer = 0f;
            lastCurveValue = 0f;
        }
        else if (!loop && timer >= cycleDuration)
        {
            if (enableWater && water != null)
                water.timeMultiplier = initialWaterMultiplier;
        }
    }
}
