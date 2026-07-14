using OneM.Attributes;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Rotation animation for the local transform.
    /// </summary>
    [AddComponentMenu("Animation/Transform/Rotation")]
    public sealed class RotationAnimation : AbstractCoreAnimation
    {
        [Tooltip("Wether to use animation curves for every axis.")]
        public bool useAnimationCurves;

        [Tooltip("Whether to rotate locally or relative to the Scene in world space.")]
        [ShowIf(nameof(useAnimationCurves), operatorType: LogicalOperatorType.Equals, value: false)]
        public Space relation = Space.Self;
        [Tooltip("The axis used to rotate.")]
        [ShowIf(nameof(useAnimationCurves), operatorType: LogicalOperatorType.Equals, value: false)]
        public Vector3 axisSpeed = Vector3.up;

        [Tooltip("The animation curves for every axis")]
        [ShowIf(nameof(useAnimationCurves))]
        public Vector3Curve axisCurve = new();

        protected override void Reset()
        {
            base.Reset();
            axisCurve.Reset();
        }

        protected override void UpdateAnimation()
        {
            base.UpdateAnimation();

            if (useAnimationCurves)
            {
                var rotation = axisCurve.Evaluate(CurrentTime);
                transform.localEulerAngles = rotation;
            }
            else
            {
                var velocity = GetDeltaTime() * speed * axisSpeed;
                transform.Rotate(velocity, relation);
            }
        }
    }
}