using OneM.Attributes;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Scale animation for the local transform.
    /// </summary>
    [AddComponentMenu("Animation/Transform/Scale")]
    public sealed class ScaleAnimation : AbstractCoreAnimation
    {
        [SerializeField, Tooltip("Wether to separate each axis scale curve.")]
        private bool separeteAxis;

        [SerializeField, Tooltip("The curve driving all axis in the scale animation.")]
        [ShowIf(nameof(separeteAxis), operatorType: LogicalOperatorType.Equals, value: false)]
        private AnimationCurve uniqueCurve;
        [SerializeField, Tooltip("The curve driving each axis in the scale animation.")]
        [ShowIf(nameof(separeteAxis))]
        private Vector3Curve separateCurve;

        protected override void Reset()
        {
            base.Reset();
            uniqueCurve.Reset(1f);
            separateCurve.Reset(1f);
        }

        protected override void UpdateAnimation(float time)
        {
            base.UpdateAnimation(time);

            if (separeteAxis)
            {
                transform.localScale = separateCurve.Evaluate(CurrentTime * Speed);
                CheckStopCondition(separateCurve);
            }
            else
            {
                transform.localScale = Vector3.one * uniqueCurve.Evaluate(CurrentTime * Speed);
                CheckStopCondition(uniqueCurve);
            }
        }
    }
}