using System.Threading;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Shake position animation for the local transform.
    /// </summary>
    /// <remarks>
    /// Vibrates the transform by applying chaotic, randomized offsets in a specified space. 
    /// The vibration intensity decays over time, making it ideal for explosions, damage feedback, or camera shakes.
    /// </remarks>
    [AddComponentMenu("Animation/Transform/Position (Shake)")]
    public sealed class PositionShakeAnimation : AbstractAnimation
    {
        [Min(0f), Tooltip("The total duration (in seconds) of the shake effect.")]
        public float duration = 0.5f;
        [Min(0f), Tooltip("The maximum distance the transform can move from its initial position.")]
        public float strength = 0.1f;

        protected override async Awaitable UpdateAnimationAsync(CancellationToken cancellationToken)
        {
            var initialPosition = transform.localPosition;

            while (CanPlay(cancellationToken) && CurrentTime < duration)
            {
                UpdateCurrentTime();

                var damper = 1f - (CurrentTime / duration);
                var randomOffset = damper * strength * Random.insideUnitSphere;

                transform.localPosition = initialPosition + randomOffset;

                await Awaitable.NextFrameAsync(cancellationToken);
            }

            transform.localPosition = initialPosition;
        }
    }
}