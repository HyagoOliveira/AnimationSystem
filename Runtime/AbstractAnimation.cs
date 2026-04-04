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

        private void OnDisable() => Stop();

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
        /// You can stop the animation using the <see cref="Stop"/> function or disable/destroy this instance.
        /// </remarks>
        /// <param name="cancellation">The cancellation source to cancel this animation.</param>
        /// <returns>An asynchronous operation.</returns>
        public async Awaitable PlayAsync(CancellationTokenSource cancellation = null)
        {
            if (IsPlayingAnimation) Stop();

            Cancel();
            cancelationSource = cancellation ?? new CancellationTokenSource();

            try
            {
                IsPlayingAnimation = true;
                await StartPlayAsync(cancelationSource.Token);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
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
    }
}