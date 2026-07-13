using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Scale animation for the local transform.
    /// <para>
    /// Use the <see cref="scaleCurve"/> curve to animate the scale.
    /// </para>
    /// </summary>
    [AddComponentMenu("Animation/Transform/Scale")]
    public sealed class ScaleAnimation : AbstractCoreAnimation
    {
        [Space]
        [SerializeField, Tooltip("The curve driving the scale animation.")]
        private AnimationCurve scaleCurve;

        public void SetScale(float value) => transform.localScale = Vector3.one * value;

        protected override void UpdateAnimation(float time)
        {
            base.UpdateAnimation(time);
            var scale = scaleCurve.Evaluate(CurrentTime * Speed);

            SetScale(scale);
            CheckStopCondition(scaleCurve);
        }
    }
}