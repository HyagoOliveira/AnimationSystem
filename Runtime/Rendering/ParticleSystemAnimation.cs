using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Animation for Particle Systems.
    /// </summary>
    /// <para>
    /// Use it as an adapter component to play ParticleSystem using an <see cref="AbstractAnimation"/> reference.
    /// </para>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleSystem))]
    [AddComponentMenu("Animation/Rendering/Particle System Animation")]
    public sealed class ParticleSystemAnimation : AbstractAnimation
    {
        [SerializeField] private ParticleSystem particleSystem;

        protected override void Reset()
        {
            base.Reset();
            particleSystem = GetComponent<ParticleSystem>();
        }

        public override void Play()
        {
            EnablePlayMode();
            particleSystem.Play();
        }

        public override void Pause()
        {
            base.Pause();
            particleSystem.Pause();
        }

        public override void Stop()
        {
            base.Stop();
            particleSystem.Stop();
        }
    }
}