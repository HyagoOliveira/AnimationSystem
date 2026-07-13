using UnityEngine;

namespace ActionCode.AnimationSystem
{
    [System.Serializable]
    public sealed class Vector3Curve
    {
        public AnimationCurve curveX = new();
        public AnimationCurve curveY = new();
        public AnimationCurve curveZ = new();

        public void Reset()
        {
            curveX = AnimationCurve.Constant(timeStart: 0f, timeEnd: 1f, value: 0f);
            curveY = AnimationCurve.Constant(timeStart: 0f, timeEnd: 1f, value: 0f);
            curveZ = AnimationCurve.Constant(timeStart: 0f, timeEnd: 1f, value: 0f);
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

            return
                currentTime >= AbstractCoreAnimation.GetDuration(curveX) ||
                currentTime >= AbstractCoreAnimation.GetDuration(curveY) ||
                currentTime >= AbstractCoreAnimation.GetDuration(curveZ);
        }
    }
}