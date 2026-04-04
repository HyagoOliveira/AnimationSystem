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
        public AbstractAnimation[] Animations => animations;

        /// <summary>
        /// Finds all local animations.
        /// </summary>
        public void FindAnimations() => animations = GetComponentsInChildren<AbstractAnimation>();

        /// <summary>
        /// Plays all animations parallely.
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
    }
}