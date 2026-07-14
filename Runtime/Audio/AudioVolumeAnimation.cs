using System.Threading;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Animates the volume of an AudioSource, creating a smooth fade transition between an initial and a final value over a specified duration.
    /// </summary>
    [AddComponentMenu("Animation/Audio/Volume")]
    public sealed class AudioVolumeAnimation : AbstractAnimation
    {
        [Tooltip("The local AudioSource component.")]
        public AudioSource audioSource;

        [Space]
        [Range(0f, 1f), Tooltip("The starting volume of the fade animation.")]
        public float initialVolume = 0f;
        [Range(0f, 1f), Tooltip("The target volume of the fade animation.")]
        public float finalVolume = 1f;

        [Space]
        [Tooltip("The total duration of the fade animation in seconds.")]
        public float duration = 1f;

        protected override void Reset()
        {
            base.Reset();
            audioSource = GetComponent<AudioSource>();
        }

        protected override async Awaitable UpdateAnimationAsync(CancellationToken cancellationToken)
        {
            audioSource.volume = initialVolume;

            while (CanPlay(cancellationToken) && CurrentTime < duration)
            {
                UpdateCurrentTime();
                var progress = CurrentTime / duration;

                audioSource.volume = Mathf.Lerp(initialVolume, finalVolume, progress);

                await Awaitable.NextFrameAsync(cancellationToken);
            }

            audioSource.volume = finalVolume;
        }
    }
}