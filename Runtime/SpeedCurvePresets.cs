using UnityEngine;

public class SpeedCurvePresets
{
    public static readonly (string name, AnimationCurve curve)[] Presets = new (string, AnimationCurve)[]
    {
        ("Linear", new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 1f),
            new Keyframe(1f, 1f, 1f, 0f)
        )),

        ("Ease In|Out", new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(1f, 1f, 0f, 0f)
        )),

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
        ))
    };
}
