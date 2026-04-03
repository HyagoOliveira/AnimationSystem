using UnityEngine;
using System.Threading;

namespace ActionCode.AnimationSystem
{
    public abstract class AbstractAnimation : MonoBehaviour
    {
        [SerializeField] private string identifier;

        public string Identifier => identifier;
        public bool IsPlaying { get; private set; }

        private CancellationTokenSource cancelationSource;

        public virtual void Stop()
        {
            Cancel();
            IsPlaying = false;
        }

        public async Awaitable RestartAsync()
        {
            Stop();
            await PlayAsync();
        }

        public async Awaitable PlayAsync()
        {
            if (IsPlaying) return;

            InitializeCancelationSource();

            try
            {
                IsPlaying = true;
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

        internal abstract Awaitable StartPlayAsync(CancellationToken token);

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