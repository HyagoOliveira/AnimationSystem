using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Alpha animation for the local UI Components.
    /// <para>
    /// Use the <see cref="opacityCurve"/> curve to animate the alpha from a UI component.
    /// </para>
    /// </summary>
    public sealed class AlphaAnimation : AbstractCoreAnimation
    {
        [Space]
        [SerializeField, Tooltip("The curve driving the opacity animation.")]
        private AnimationCurve opacityCurve;

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private SpriteRenderer spriteRenderer;
#if UNITY_UGUI
        [SerializeField] private UnityEngine.UI.Graphic graphic;
        [SerializeField] private UnityEngine.UI.Shadow shadow;
#endif

        private void OnDisable() => Stop();

        public void SetOpacity(float opacity)
        {
            if (canvasGroup) canvasGroup.alpha = opacity;
            if (spriteRenderer) spriteRenderer.color.WithAlpha(opacity);
#if UNITY_UGUI
            if (graphic) graphic.color = graphic.color.WithAlpha(opacity);
            if (shadow) shadow.effectColor = shadow.effectColor.WithAlpha(opacity);
#endif
        }

        protected override void UpdateAnimation(float time)
        {
            base.UpdateAnimation(time);
            var opacity = opacityCurve.Evaluate(CurrentTime);

            SetOpacity(opacity);
            CheckStopCondition(opacityCurve);
        }
    }
}