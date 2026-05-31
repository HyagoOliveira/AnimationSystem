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
        public bool playOnAwake = true;

        public string Identifier => identifier;
        public virtual bool IsPaused { get; private set; }
        public virtual bool IsPlaying { get; private set; }

        private CancellationTokenSource cancelationSource;

        private void OnEnable()
        {
            if (playOnAwake) Play();
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

        protected virtual async Awaitable PlayAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && IsPlaying) await Awaitable.NextFrameAsync(token);
        }

        private void Cancel()
        {
            cancelationSource?.Cancel();
            cancelationSource?.Dispose();
            cancelationSource = null;
        }
    }
}