using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Sequence for parallel animations.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AnimationSequence : MonoBehaviour
    {
        [SerializeField, Tooltip("All the local animations.")]
        private AbstractAnimation[] animations;

        /// <summary>
        /// All the local animations.
        /// </summary>
        public AbstractAnimation[] Animations
        {
            get => animations;
            set => animations = value;
        }

        /// <summary>
        /// Finds all local animations if none is set.
        /// </summary>
        public void TryFindAnimations()
        {
            if (animations == null || animations.Length == 0)
                FindAnimations();
        }

        /// <summary>
        /// Finds all local animations.
        /// </summary>
        public void FindAnimations() => animations = GetComponentsInChildren<AbstractAnimation>();

        /// <summary>
        /// Plays all animations at once.
        /// </summary>
        public void Play()
        {
            foreach (var animation in animations)
            {
                _ = animation.PlayAsync();
            }
        }

        /// <summary>
        /// Stops all animations at once.
        /// </summary>
        public void Stop()
        {
            foreach (var animation in animations)
            {
                animation.Stop();
            }
        }

        /// <summary>
        /// Plays all animations, one after another.
        /// </summary>
        /// <returns>An asynchronous operation.</returns>
        public async Awaitable PlayAsync()
        {
            foreach (var animation in animations)
            {
                // Starts all animations
                animation.PlayFirstFrame();
            }

            foreach (var animation in animations)
            {
                await animation.PlayAsync();
            }
        }

        public void ResetSpeed()
        {
            foreach (var animation in animations)
            {
                animation.ResetSpeed();
            }
        }
    }
}