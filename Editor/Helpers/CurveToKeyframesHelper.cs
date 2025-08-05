using UnityEditor;
using UnityEngine;

public class CurveToKeyframesHelper : EditorWindow
{
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private Vector2 scrollPos;

    [MenuItem("Tools/Curve To Keyframes")]
    static void Init()
    {
        CurveToKeyframesHelper window = (CurveToKeyframesHelper)EditorWindow.GetWindow(typeof(CurveToKeyframesHelper));
        window.titleContent = new GUIContent("Curve To Keyframes");
        window.Show();
    }

    void OnGUI()
    {
        curve = EditorGUILayout.CurveField(curve, Color.green, new Rect(0, 0, 1, 1));

        GUILayout.Space(10);

        if (GUILayout.Button("Generate C# Keyframes"))
        {
            string keyframeCode = GenerateKeyframeCode(curve);
            EditorGUIUtility.systemCopyBuffer = keyframeCode; // copy to clipboard
            Debug.Log("Keyframes copied to clipboard :\n" + keyframeCode);
        }

        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.TextArea(GenerateKeyframeCode(curve));
        EditorGUILayout.EndScrollView();
    }

    string GenerateKeyframeCode(AnimationCurve c)
    {
        string code = "new AnimationCurve(\n";
        for (int i = 0; i < c.keys.Length; i++)
        {
            Keyframe k = c.keys[i];
            code += $"    new Keyframe({k.time}f, {k.value}f, {k.inTangent}f, {k.outTangent}f)";
            if (i < c.keys.Length - 1) code += ",\n";
        }
        code += "\n);";
        return code;
    }
}

