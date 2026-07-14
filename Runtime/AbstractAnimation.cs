using System.Threading;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Abstract component for an animation.
    /// </summary>
    public abstract class AbstractAnimation : MonoBehaviour
    {
        [Tooltip("The animation identifier. Use to differentiate in GameObjects with multiple animations.")]
        public string identifier;

        [Space]
        [Tooltip("Wether to play when component is enabled.")]
        public bool playOnEnable = true;
        [Tooltip("If enabled, animation will play even if Time.deltaTime = 0")]
        public bool useUnscaledTime;

        [Space]
        [Tooltip("The animation speed.")]
        public float speed = 1f;

        public bool IsPaused { get; private set; }
        public bool IsPlaying { get; private set; }
        public float CurrentTime { get; private set; }

        protected virtual void Reset() => SetIdentifier();

        private void OnEnable()
        {
            if (playOnEnable) Play();
        }

        private void OnDisable() => Stop();

        public void Restart()
        {
            Stop();
            Play();
        }

        public void Pause() => IsPaused = true;

        public void Play()
        {
            EnablePlayMode();
            _ = PlayAsync(destroyCancellationToken);
        }

        public async Awaitable PlayAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                EnablePlayMode();
                StartPlay();
                await UpdateAnimationAsync(cancellationToken);
            }
            catch (System.OperationCanceledException) { } // implementations may cancel animation
            catch (System.Exception e) { Debug.LogException(e); }
            finally { Stop(); }
        }

        public void Stop()
        {
            IsPaused = false;
            IsPlaying = false;
            CurrentTime = 0f;
        }

        public override string ToString()
        {
            var hasIdentifier = !string.IsNullOrEmpty(identifier);
            return hasIdentifier ? identifier : base.ToString();
        }

        protected virtual void SetIdentifier() => identifier = GetType().Name;
        protected virtual void StartPlay() => CurrentTime = 0f;

        protected virtual async Awaitable UpdateAnimationAsync(CancellationToken cancellationToken)
        {
            while (CanPlay(cancellationToken))
            {
                UpdateAnimation();
                UpdateCurrentTime();
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }

        protected virtual void UpdateAnimation() { }

        protected void CancelAnimation() => throw new System.OperationCanceledException();
        protected void UpdateCurrentTime() => CurrentTime += GetDeltaTime() * speed;
        protected bool CanPlay(CancellationToken cancellationToken) => !cancellationToken.IsCancellationRequested && IsPlaying;
        protected float GetDeltaTime() => useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        private void EnablePlayMode()
        {
            enabled = true;
            IsPaused = false;
            IsPlaying = true;
        }
    }
}