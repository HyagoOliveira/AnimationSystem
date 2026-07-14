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

        protected override async Awaitable UpdateAnimationAsync(CancellationToken cancellationToken)
        {
            target.ForceMeshUpdate();
            var totalCharacters = target.textInfo.characterCount;

            target.maxVisibleCharacters = 0;

            while (CanPlay(cancellationToken) && CurrentTime < duration)
            {
                UpdateCurrentTime();
                var progress = CurrentTime / duration;

                target.maxVisibleCharacters = (int)(progress * totalCharacters);

                await Awaitable.NextFrameAsync(cancellationToken);
            }

            target.maxVisibleCharacters = totalCharacters;
        }
#endif
    }
}
