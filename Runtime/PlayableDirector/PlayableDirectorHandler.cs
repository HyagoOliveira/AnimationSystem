#if UNITY_DIRECTOR
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace ActionCode.AnimationSystem
{
    [DisallowMultipleComponent]
    public sealed class PlayableDirectorHandler : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        [SerializeField] private uint activationFrames = 60;

        [Space]
        public UnityEvent OnStopped;

        private void Reset() => director = GetComponentInChildren<PlayableDirector>();
        private void Awake() => DeactivateDirector();
        private void Start() => _ = ExecuteAfterFrames(activationFrames, ActivateDirector);
        private void OnEnable() => director.stopped += HandleStopped;
        private void OnDisable() => director.stopped -= HandleStopped;

        private void HandleStopped(PlayableDirector _) => OnStopped?.Invoke();
        private void DeactivateDirector() => director.gameObject.SetActive(false);
        private void ActivateDirector() => director.gameObject.SetActive(true);

        private static async Awaitable ExecuteAfterFrames(uint frames, Action action)
        {
            uint currentFrames = 0;
            do
            {
                await Awaitable.NextFrameAsync();
            } while (++currentFrames < frames);

            action?.Invoke();
        }
    }
}
#endif