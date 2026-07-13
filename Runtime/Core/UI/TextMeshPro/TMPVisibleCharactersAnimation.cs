using System.Threading;
using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Visible Characters animation for Text Mesh Pro.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Animation/UI/TextMesh Pro/Visible Characters")]
    public sealed class TMPVisibleCharactersAnimation : AbstractAnimation
    {
#if UNITY_TMP
        [SerializeField, Tooltip("The Text to animate the visible characters.")]
        private TMPro.TMP_Text target;
        [SerializeField, Tooltip("The animation duration in seconds."), Min(0f)]
        private float duration = 0.4f;

        protected override void Reset()
        {
            base.Reset();
            target = GetComponent<TMPro.TMP_Text>();
        }

        public override void Play()
        {
            base.Play();
            _ = PlayAsync(destroyCancellationToken);
        }

        protected override async Awaitable PlayAsync(CancellationToken token)
        {
            target.ForceMeshUpdate();

            var currentTime = 0f;
            var totalCharacters = target.textInfo.characterCount;

            target.maxVisibleCharacters = 0;

            while (CanPlayAsync(token) && currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                var progress = currentTime / duration;

                target.maxVisibleCharacters = (int)(progress * totalCharacters);

                await Awaitable.NextFrameAsync(token);
            }

            target.maxVisibleCharacters = totalCharacters;
        }
#endif
    }
}
