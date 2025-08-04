using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Timelapse : MonoBehaviour
{
    [Header("Directional Light")]
    public Light sun;
    public Vector3 sunRotationStart = new Vector3(180f, -30f, 0f);
    public Vector3 sunRotationEnd = new Vector3(0f, -30f, 0f);

    [Header("Volumetric Clouds")]
    public Volume volume;
    public Vector3 cloudOffsetStart = Vector3.zero;
    public Vector3 cloudOffsetEnd = new Vector3(100f, 0f, 0f);

    [Header("Timing")]
    public float cycleDuration = 60f;
    public bool loop = true;

    // --- Internals ---
    private VolumetricClouds clouds;
    private float timer = 0f;
    private Vector3 initialCloudOffset;

    void Start()
    {
        if (volume != null && volume.profile.TryGet(out clouds))
        {
            initialCloudOffset = clouds.shapeOffset.value;
        }
        else
        {
            Debug.LogWarning("No Volumetric Clouds assigned.");
        }
    }

    void Update()
    {
        if (sun == null || clouds == null) return;

        // Progression (0 → 1)
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / cycleDuration);

        // Directional interpolation
        Quaternion startRot = Quaternion.Euler(sunRotationStart);
        Quaternion endRot = Quaternion.Euler(sunRotationEnd);
        sun.transform.localRotation = Quaternion.Slerp(startRot, endRot, t);

        // Clouds interpolation
        Vector3 offset = Vector3.Lerp(cloudOffsetStart, cloudOffsetEnd, t);
        clouds.shapeOffset.value = initialCloudOffset + offset;

        // Loop
        if (loop && timer >= cycleDuration)
        {
            timer = 0f;
        }
    }
}
