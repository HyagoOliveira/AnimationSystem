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
    public sealed class PositionAnimation : AbstractAnimation
    {
        [SerializeField, Tooltip("The curve driving the position animation.")]
        private Vector3Curve positionCurve = new();

        private Vector3 originalPosition;

        protected override void Reset()
        {
            base.Reset();
            positionCurve.Reset();
        }

        protected override void StartPlay()
        {
            base.StartPlay();
            originalPosition = transform.localPosition;
        }

        protected override void UpdateAnimation()
        {
            base.UpdateAnimation();

            var position = positionCurve.Evaluate(CurrentTime);
            transform.localPosition = originalPosition + position;

            if (positionCurve.IsFinished(CurrentTime)) CancelAnimation();
        }
    }
}