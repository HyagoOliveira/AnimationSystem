using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Abstract animation using Update function.
    /// </summary>
    public abstract class AbstractCoreAnimation : AbstractAnimation
    {
        [Tooltip("If enabled, animation will play even if Time.deltaTime = 0")]
        public bool useUnscaledTime;

        private void Update() => UpdateAnimation(GetDeltaTime());

        public override void Play()
        {
            base.Play();
            enabled = true;
        }

        public override void Stop()
        {
            base.Stop();
            enabled = false;
        }

        protected abstract void UpdateAnimation(float time);

        private float GetDeltaTime() => useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
    }
}