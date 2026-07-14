using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Position animation for the local transform.
    /// <para>
    /// Use the <see cref="positionCurve"/> curve to animate the scale.
    /// </para>
    /// </summary>
    [AddComponentMenu("Animation/Transform/Position")]
    public sealed class PositionAnimation : AbstractCoreAnimation
    {
        [SerializeField, Tooltip("The curve driving the position animation.")]
        private Vector3Curve positionCurve = new();

        private Vector3 originalPosition;

        protected override void Reset()
        {
            base.Reset();
            positionCurve.Reset();
        }

        private void Awake() => originalPosition = transform.localPosition;

        protected override void UpdateAnimation()
        {
            base.UpdateAnimation();

            var position = positionCurve.Evaluate(CurrentTime);
            transform.localPosition = originalPosition + position;

            CheckStopCondition(positionCurve);
        }
    }
}