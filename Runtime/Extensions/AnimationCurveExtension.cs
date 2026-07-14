using UnityEngine;

namespace ActionCode.AnimationSystem
{
    /// <summary>
    /// Extension class for AnimationCurves.
    /// </summary>
    public static class AnimationCurveExtension
    {
        #region CORE
        /// <summary>
        /// Resets the animation curve to a flat constant curve with a single keyframe at value 0.
        /// </summary>
        /// <remarks>Same as <code>AnimationCurve.Constant(timeStart: 0f, timeEnd: 1f, value);</code></remarks>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="value">The keyframe value.</param>
        public static void Reset(this AnimationCurve curve, float value = 0f)
        {
            if (curve == null) return;

            curve.keys = new[]
            {
                new Keyframe(0f, value),
                new Keyframe(1f, value)
            };
        }

        /// <summary>
        /// Returns wether the animation curve is in loop.
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <returns>Wether the animation curve is in loop.</returns>
        public static bool IsLoop(this AnimationCurve curve) => curve.postWrapMode is WrapMode.Loop or WrapMode.PingPong;

        /// <summary>
        /// Returns wether the animation curve is playing.
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="time"><inheritdoc cref="AnimationCurve.Evaluate(float)" path="/param[@name='time']"/></param>
        /// <returns>Wether the animation curve is playing.</returns>
        public static bool IsPlaying(this AnimationCurve curve, float time) => !IsFinished(curve, time);

        /// <summary>
        /// Returns wether the animation curve has finish.
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="time"><inheritdoc cref="AnimationCurve.Evaluate(float)" path="/param[@name='time']"/></param>
        /// <returns>Wether the animation curve has finish.</returns>
        public static bool IsFinished(this AnimationCurve curve, float time)
        {
            if (IsLoop(curve)) return false;
            return time >= GetDuration(curve);
        }

        /// <summary>
        /// Returns the curve duration in seconds.
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <returns>The curve duration.</returns>
        public static float GetDuration(this AnimationCurve curve) => curve != null && curve.keys.Length > 0 ? curve.keys[^1].time : 0f;

        /// <summary>
        /// Reshapes the animation curve into a preset shape based on the provided <see cref="AnimationEase"/>.
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="ease">The predefined ease style type to apply to the curve.</param>
        public static void SetAs(this AnimationCurve curve, AnimationEase ease)
        {
            switch (ease)
            {
                case AnimationEase.Sine: curve.SetAsSineWave(); break;
                case AnimationEase.Cosine: curve.SetAsCosineWave(); break;
                case AnimationEase.Elastic: curve.SetAsElastic(); break;
                case AnimationEase.ElasticIn: curve.SetAsElasticIn(); break;
                case AnimationEase.ElasticOut: curve.SetAsElasticOut(); break;
                case AnimationEase.Bounce: curve.SetAsBounce(); break;
                case AnimationEase.BounceIn: curve.SetAsBounceIn(); break;
                case AnimationEase.BounceOut: curve.SetAsBounceOut(); break;
            }
        }
        #endregion

        #region SINE AND COSINE
        /// <summary>
        /// Reshapes the animation curve into a Sine wave.
        /// </summary>
        /// <param name="curve">The Target AnimationCurve instance.</param>
        /// <param name="frequency">The number of wave cycles to fit within the curve timeline.</param>
        /// <param name="resolution">The total number of keyframes used to shape the wave.</param>
        public static void SetAsSineWave(this AnimationCurve curve, float frequency = 1f, int resolution = 30)
        {
            if (curve == null) return;
            var keys = new Keyframe[resolution];

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                var value = Mathf.Sin(time * frequency * 2f * Mathf.PI);
                keys[i] = new Keyframe(time, value);
            }

            curve.keys = keys;

            for (int i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }

        /// <summary>
        /// Reshapes the animation curve into a Cosine wave.
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="frequency"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='frequency']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsCosineWave(this AnimationCurve curve, float frequency = 1f, int resolution = 30)
        {
            if (curve == null) return;
            var keys = new Keyframe[resolution];

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                var value = Mathf.Cos(time * frequency * 2f * Mathf.PI);
                keys[i] = new Keyframe(time, value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }
        #endregion

        #region ELASTIC
        /// <summary>
        /// Reshapes the animation curve into an Elastic In wave (EaseInElastic).
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsElasticIn(this AnimationCurve curve, int resolution = 60)
        {
            if (curve == null) return;

            var keys = new Keyframe[resolution];
            const float c4 = (2f * Mathf.PI) / 3f;

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                float value;

                if (time <= 0f) value = 0f;
                else if (time >= 1f) value = 1f;
                else value = -Mathf.Pow(2f, 10f * time - 10f) * Mathf.Sin((time * 10f - 10.75f) * c4);

                keys[i] = new Keyframe(time, value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }

        /// <summary>
        /// Reshapes the animation curve into an Elastic Out wave (EaseOutElastic).
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsElasticOut(this AnimationCurve curve, int resolution = 60)
        {
            if (curve == null) return;

            var keys = new Keyframe[resolution];
            const float c4 = (2f * Mathf.PI) / 3f;

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                float value;

                if (time <= 0f) value = 0f;
                else if (time >= 1f) value = 1f;
                else
                {
                    // Standard EaseOutElastic formula: 2^(-10x) * sin((x * 10 - 0.75) * c4) + 1
                    value = Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * 10f - 0.75f) * c4) + 1f;
                }

                keys[i] = new Keyframe(time, value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }

        /// <summary>
        /// Reshapes the animation curve into an Elastic In Out wave (EaseInOutElastic).
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsElastic(this AnimationCurve curve, int resolution = 80)
        {
            if (curve == null) return;

            var keys = new Keyframe[resolution];
            const float c5 = (2f * Mathf.PI) / 4.5f;

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                float value;

                if (time <= 0f) value = 0f;
                else if (time >= 1f) value = 1f;
                else if (time < 0.5f) value = -(Mathf.Pow(2f, 20f * time - 10f) * Mathf.Sin((20f * time - 11.125f) * c5)) / 2f;
                else value = (Mathf.Pow(2f, -20f * time + 10f) * Mathf.Sin((20f * time - 11.125f) * c5)) / 2f + 1f;

                keys[i] = new Keyframe(time, value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }
        #endregion

        #region BOUNCE
        /// <summary>
        /// Reshapes the animation curve into a Bounce In wave (EaseInBounce).
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsBounceIn(this AnimationCurve curve, int resolution = 60)
        {
            if (curve == null) return;
            var keys = new Keyframe[resolution];

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                var bounceTime = 1f - time;
                float value;

                if (bounceTime < 1f / 2.75f) value = 7.5625f * bounceTime * bounceTime;
                else if (bounceTime < 2f / 2.75f) value = 7.5625f * (bounceTime -= 1.5f / 2.75f) * bounceTime + 0.75f;
                else if (bounceTime < 2.5f / 2.75f) value = 7.5625f * (bounceTime -= 2.25f / 2.75f) * bounceTime + 0.9375f;
                else value = 7.5625f * (bounceTime -= 2.625f / 2.75f) * bounceTime + 0.984375f;

                keys[i] = new Keyframe(time, 1f - value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }

        /// <summary>
        /// Reshapes the animation curve into a Bounce Out wave (EaseOutBounce).
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsBounceOut(this AnimationCurve curve, int resolution = 60)
        {
            if (curve == null) return;
            var keys = new Keyframe[resolution];

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                float value;

                if (time < 1f / 2.75f) value = 7.5625f * time * time;
                else if (time < 2f / 2.75f) value = 7.5625f * (time -= 1.5f / 2.75f) * time + 0.75f;
                else if (time < 2.5f / 2.75f) value = 7.5625f * (time -= 2.25f / 2.75f) * time + 0.9375f;
                else value = 7.5625f * (time -= 2.625f / 2.75f) * time + 0.984375f;

                keys[i] = new Keyframe((float)i / (resolution - 1), value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }

        /// <summary>
        /// Reshapes the animation curve into a Bounce In Out wave (EaseInOutBounce).
        /// </summary>
        /// <param name="curve"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='curve']"/></param>
        /// <param name="resolution"><inheritdoc cref="SetAsSineWave(AnimationCurve, float, int)" path="/param[@name='resolution']"/></param>
        public static void SetAsBounce(this AnimationCurve curve, int resolution = 80)
        {
            if (curve == null) return;
            var keys = new Keyframe[resolution];

            for (var i = 0; i < resolution; i++)
            {
                var time = (float)i / (resolution - 1);
                float value;

                if (time < 0.5f)
                {
                    var bounceTime = 1f - (time * 2f);
                    if (bounceTime < 1f / 2.75f) value = 7.5625f * bounceTime * bounceTime;
                    else if (bounceTime < 2f / 2.75f) value = 7.5625f * (bounceTime -= 1.5f / 2.75f) * bounceTime + 0.75f;
                    else if (bounceTime < 2.5f / 2.75f) value = 7.5625f * (bounceTime -= 2.25f / 2.75f) * bounceTime + 0.9375f;
                    else value = 7.5625f * (bounceTime -= 2.625f / 2.75f) * bounceTime + 0.984375f;

                    value = (1f - value) * 0.5f;
                }
                else
                {
                    var bounceTime = time * 2f - 1f;
                    if (bounceTime < 1f / 2.75f) value = 7.5625f * bounceTime * bounceTime;
                    else if (bounceTime < 2f / 2.75f) value = 7.5625f * (bounceTime -= 1.5f / 2.75f) * bounceTime + 0.75f;
                    else if (bounceTime < 2.5f / 2.75f) value = 7.5625f * (bounceTime -= 2.25f / 2.75f) * bounceTime + 0.9375f;
                    else value = 7.5625f * (bounceTime -= 2.625f / 2.75f) * bounceTime + 0.984375f;

                    value = value * 0.5f + 0.5f;
                }

                keys[i] = new Keyframe(time, value);
            }

            curve.keys = keys;

            for (var i = 0; i < resolution; i++)
            {
                curve.SmoothTangents(i, 0f);
            }
        }
        #endregion
    }
}