using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Alpha animation for the local UI Components.
    /// <para>
    /// Use the <see cref="opacityCurve"/> curve to animate the alpha from a UI component.
    /// </para>
    /// </summary>
    [AddComponentMenu("Animation/Rendering/Alpha")]
    public sealed class AlphaAnimation : AbstractAnimation
    {
        [Space]
        [SerializeField, Tooltip("The curve driving the opacity animation.")]
        private AnimationCurve opacityCurve = new();

        [Space]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private SpriteRenderer spriteRenderer;
#if UNITY_UGUI
        [SerializeField] private UnityEngine.UI.Graphic graphic;
        [SerializeField] private UnityEngine.UI.Shadow shadow;
#endif

        protected override void Reset()
        {
            base.Reset();
            opacityCurve.Reset(1f);

            canvasGroup = GetComponent<CanvasGroup>();
            spriteRenderer = GetComponent<SpriteRenderer>();

#if UNITY_UGUI
            UnityEngine.UI.Graphic graphic = GetComponent<UnityEngine.UI.Graphic>();
            UnityEngine.UI.Shadow shadow = GetComponent<UnityEngine.UI.Shadow>();
#endif
        }

        public void SetOpacity(float opacity)
        {
            if (canvasGroup) canvasGroup.alpha = opacity;
            if (spriteRenderer) spriteRenderer.color = spriteRenderer.color.WithAlpha(opacity);
#if UNITY_UGUI
            if (graphic) graphic.color = graphic.color.WithAlpha(opacity);
            if (shadow) shadow.effectColor = shadow.effectColor.WithAlpha(opacity);
#endif
        }

        protected override void UpdateAnimation()
        {
            base.UpdateAnimation();
            SetOpacity(opacityCurve.Evaluate(CurrentTime));
            if (opacityCurve.IsFinished(CurrentTime)) CancelAnimation();
        }
    }
}