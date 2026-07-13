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

        public bool HasCurveFinished(float currentTime)
        {
            var isLoop =
                AbstractCoreAnimation.IsLoop(curveX.postWrapMode) ||
                AbstractCoreAnimation.IsLoop(curveY.postWrapMode) ||
                AbstractCoreAnimation.IsLoop(curveZ.postWrapMode);
            if (isLoop) return false;

            var minDuration = Mathf.Min(
                AbstractCoreAnimation.GetDuration(curveX),
                AbstractCoreAnimation.GetDuration(curveY),
                AbstractCoreAnimation.GetDuration(curveZ)
            );

            return currentTime >= minDuration;
        }
    }
}