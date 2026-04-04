using UnityEngine;
using System.Threading;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Abstract component for an animation.
    /// </summary>
    public abstract class AbstractAnimation : MonoBehaviour
    {
        [SerializeField, Tooltip("The animation identifier.")]
        private string identifier;

        /// <summary>
        /// The animation identifier.
        /// </summary>
        public string Identifier => identifier;

        /// <summary>
        /// Whether is currently playing this animation.
        /// </summary>
        public bool IsPlayingAnimation { get; private set; }

        private CancellationTokenSource cancelationSource;

        public virtual void Stop()
        {
            Cancel();
            IsPlayingAnimation = false;
        }

        public virtual void Restart()
        {
            Stop();
            Play();
        }

        public virtual void Play() => _ = PlayAsync();

        /// <summary>
        /// Plays this animation asynchronously.
        /// </summary>
        /// <remarks>
        /// You can stop the animation using <see cref="Stop"/>.<br/>
        /// The animation will be stopped automatically if GameObject is destroyed.
        /// </remarks>
        /// <returns>An asynchronous operation.</returns>
        public async Awaitable PlayAsync()
        {
            if (IsPlayingAnimation) return;

            InitializeCancelationSource();

            try
            {
                IsPlayingAnimation = true;
                await StartPlayAsync(cancelationSource.Token);
            }
            finally
            {
                Stop();
            }
        }

        public override string ToString()
        {
            var hasIdentifier = !string.IsNullOrEmpty(identifier);
            return hasIdentifier ? identifier : base.ToString();
        }

        protected abstract Awaitable StartPlayAsync(CancellationToken token);

        private void Cancel()
        {
            cancelationSource?.Cancel();
            cancelationSource?.Dispose();
            cancelationSource = null;
        }

        private void InitializeCancelationSource()
        {
            Cancel();
            cancelationSource = new CancellationTokenSource();

            // Link the manual token with the built-in destroyCancellationToken
            // If this GameObject is destroyed, the animation is cancelled.
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cancelationSource.Token,
                destroyCancellationToken
            );
        }
    }
}