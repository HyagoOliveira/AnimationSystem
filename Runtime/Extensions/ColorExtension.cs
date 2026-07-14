using UnityEngine;

namespace ActionCode.AnimationSystem
{
    public static class ColorExtension
    {
        public static Color WithAlpha(this Color color, float alpha) =>
            new(color.r, color.g, color.b, alpha);
    }
}