using UnityEngine;

namespace ActionCode.AnimationSystem
{
    public static class ColorExtension
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}