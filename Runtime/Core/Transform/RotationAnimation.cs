using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Rotation animation for the local transform.
    /// </summary>
    [AddComponentMenu("Animation/Transform/Rotation")]
    public sealed class RotationAnimation : AbstractCoreAnimation
    {
        [Tooltip("Whether to rotate locally or relative to the Scene in world space.")]
        public Space relation = Space.Self;
        [Tooltip("The axis used to rotate.")]
        public Vector3 axis = Vector3.up;

        protected override void UpdateAnimation(float time)
        {
            base.UpdateAnimation(time);
            var velocity = Speed * time * axis;
            transform.Rotate(velocity, relation);
        }
    }
}