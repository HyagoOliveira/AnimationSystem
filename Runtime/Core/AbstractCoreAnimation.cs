using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Abstract animation using Update function.
    /// </summary>
    public abstract class AbstractCoreAnimation : AbstractAnimation
    {
        [Tooltip("The animation speed.")]
        public float speed = 1f;

        /// <summary>
        /// The animation current time.
        /// </summary>
        public float CurrentTime { get; private set; }

        private void Update() => UpdateAnimation();

        public override void Play()
        {
            base.Play();
            enabled = true;
        }

        public override void Stop()
        {
            base.Stop();
            enabled = false;
            CurrentTime = 0f;
        }

        protected virtual void UpdateAnimation() => CurrentTime += GetDeltaTime() * speed;

        protected void CheckStopCondition(AnimationCurve curve)
        {
            if (HasCurveFinished(curve, CurrentTime)) Stop();
        }

        protected void CheckStopCondition(Vector3Curve curve)
        {
            if (curve.HasCurveFinished(CurrentTime)) Stop();
        }

        public static bool IsLoop(WrapMode mode) => mode is WrapMode.Loop or WrapMode.PingPong;

        public static bool HasCurveFinished(AnimationCurve curve, float currentTime)
        {
            var isLoop = IsLoop(curve.postWrapMode);
            if (isLoop) return false;
            return currentTime >= GetDuration(curve);
        }

        public static float GetDuration(AnimationCurve curve) => curve.keys.Length > 0 ? curve.keys[^1].time : 0f;
    }
}