using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Rotation animation for the local transform.
    /// </summary>
    [AddComponentMenu("Animation/Transform/Rotation")]
    public sealed class RotationAnimation : AbstractCoreAnimation
    {
        public Space relation = Space.Self;
        public Vector3 speed = Vector3.up * 15f;

        protected override void UpdateAnimation(float time)
        {
            base.UpdateAnimation(time);
            var velocity = speed * time;
            transform.Rotate(velocity, relation);
        }
    }
}