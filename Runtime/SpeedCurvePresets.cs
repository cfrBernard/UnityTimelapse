using UnityEngine;

public class SpeedCurvePresets
{
    public static readonly (string name, AnimationCurve curve)[] Presets = new (string, AnimationCurve)[]
    {
        ("Linear", AnimationCurve.Linear(0f, 0f, 1f, 1f)),

        ("Ease In|Out", AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)),

        ("Slow Out", new AnimationCurve(
            new Keyframe(0f, 0f, 2f, 2f),
            new Keyframe(1f, 1f, 0f, 0f)
        )),

        ("Slow In", new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(1f, 1f, 2f, 2f)
        )),

        ("Custom", new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 1f),
            new Keyframe(1f, 1f, 1f, 0f)
        )),
    };
}
