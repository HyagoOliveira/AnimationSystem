using UnityEngine;

namespace ActionCode.AnimationSystem
{
    [System.Serializable]
    public sealed class Vector3Curve
    {
        public AnimationCurve curveX = new();
        public AnimationCurve curveY = new();
        public AnimationCurve curveZ = new();

        public void Reset(float value = 0f)
        {
            curveX.Reset(value);
            curveY.Reset(value);
            curveZ.Reset(value);
        }

        public Vector3 Evaluate(float time) => new(
            curveX.Evaluate(time),
            curveY.Evaluate(time),
            curveZ.Evaluate(time)
        );

        public bool IsFinished(float time) =>
            curveX.IsFinished(time) &&
            curveY.IsFinished(time) &&
            curveZ.IsFinished(time);
    }
}