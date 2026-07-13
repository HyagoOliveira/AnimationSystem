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
        private Vector3Curve positionCurve;

        private Vector3 originalPosition;

        protected override void Reset()
        {
            base.Reset();
            positionCurve.Reset();
        }

        private void Awake() => originalPosition = transform.position;

        protected override void UpdateAnimation(float time)
        {
            base.UpdateAnimation(time);

            var position = positionCurve.Evaluate(CurrentTime * Speed);
            transform.position = originalPosition + position;

            CheckStopCondition(positionCurve);
        }
    }
}