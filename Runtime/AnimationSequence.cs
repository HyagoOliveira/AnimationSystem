using UnityEngine;
using System.Threading;

namespace ActionCode.AnimationSystem
{
    [DisallowMultipleComponent]
    public sealed class AnimationSequence : AbstractAnimation
    {
        public AbstractAnimation[] animations;

        internal override async Awaitable StartPlayAsync(CancellationToken token)
        {
            foreach (var animation in animations)
            {
                await animation.StartPlayAsync(token);
            }
        }

        public override void Stop()
        {
            foreach (var animation in animations)
            {
                animation.Stop();
            }
        }
    }
}