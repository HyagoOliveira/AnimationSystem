using System.Threading;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Abstract component for an animation.
    /// </summary>
    public abstract class AbstractAnimation : MonoBehaviour
    {
        [Tooltip("The animation identifier.")]
        public string identifier;
        [Tooltip("Wether to play when component is enabled.")]
        public bool playOnEnable = true;

        public virtual bool IsPaused { get; private set; }
        public virtual bool IsPlaying { get; private set; }

        private CancellationTokenSource cancelationSource;

        protected virtual void Reset() => SetIdentifier();

        private void OnEnable()
        {
            if (playOnEnable) Play();
        }

        private void OnDisable() => Stop();

        public virtual void Restart()
        {
            Stop();
            Play();
        }

        public virtual void Pause() => IsPaused = true;

        public virtual void Play()
        {
            IsPaused = false;
            IsPlaying = true;
        }

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
            Cancel();
            cancelationSource = cancellation ?? new CancellationTokenSource();

            try
            {
                Play();
                await PlayAsync(cancelationSource.Token);
            }
            catch (System.OperationCanceledException) { }
            catch (System.Exception e) { Debug.LogException(e); }
            finally { Stop(); }
        }

        public virtual void Stop()
        {
            IsPaused = false;
            IsPlaying = false;
            Cancel();
        }

        public override string ToString()
        {
            var hasIdentifier = !string.IsNullOrEmpty(identifier);
            return hasIdentifier ? identifier : base.ToString();
        }

        protected virtual void SetIdentifier() => identifier = GetType().Name;

        protected virtual async Awaitable PlayAsync(CancellationToken token)
        {
            while (CanPlayAsync(token)) await Awaitable.NextFrameAsync(token);
        }

        protected bool CanPlayAsync(CancellationToken token) => !token.IsCancellationRequested && IsPlaying;

        private void Cancel()
        {
            cancelationSource?.Cancel();
            cancelationSource?.Dispose();
            cancelationSource = null;
        }
    }
}