using System.Threading;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Punch position animation for the local transform.
    /// </summary>
    /// <remarks>
    /// Applies a directional impact force to the transform, making it oscillate like a physical spring. 
    /// The motion follows a precise decaying sine wave along a specific axis, making it ideal for weapon recoil or UI button clicks.
    /// </remarks>
    [AddComponentMenu("Animation/Transform/Position (Punch)")]
    public sealed class PositionPunchAnimation : AbstractAnimation
    {
        [Tooltip("The direction and maximum distance of the initial impact force.")]
        public Vector3 punchDirection = Vector3.right;
        [Min(0f), Tooltip("The total duration of the punch animation in seconds.")]
        public float duration = 0.5f;
        [Min(0f), Tooltip("How fast the object oscillates back and forth.")]
        public float frequency = 5f;
        [Min(0f), Tooltip("How quickly the impact loses energy. Higher values mean a faster stabilization.")]
        public float decay = 4.5f;

        public override void Play()
        {
            base.Play();
            _ = PlayAsync(destroyCancellationToken);
        }

        protected override async Awaitable PlayAsync(CancellationToken token)
        {
            var elapsedTime = 0f;
            var initialLocalPosition = transform.localPosition;

            while (CanPlayAsync(token) && elapsedTime < duration)
            {
                elapsedTime += GetDeltaTime();
                var time = elapsedTime / duration;
                // Mathematical formula for a decaying sine wave (simulating a spring shock)
                var decayFactor = Mathf.Exp(-decay * time);
                var sineFactor = Mathf.Sin(time * frequency * 2f * Mathf.PI);
                var offset = punchDirection * (decayFactor * sineFactor);

                transform.localPosition = initialLocalPosition + offset;

                await Awaitable.NextFrameAsync(token);
            }

            transform.localPosition = initialLocalPosition;
        }
    }
}