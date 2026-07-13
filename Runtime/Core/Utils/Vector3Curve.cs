using UnityEngine;

namespace ActionCode.AnimationSystem
{
    public sealed class Vector3Curve
    {
        public AnimationCurve curveX = new();
        public AnimationCurve curveY = new();
        public AnimationCurve curveZ = new();

        public Vector3 Evaluate(float time) => new(
            curveX.Evaluate(time),
            curveY.Evaluate(time),
            curveZ.Evaluate(time)
        );
    }
}