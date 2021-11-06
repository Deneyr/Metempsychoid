using Astrategia.Animation;
using Astrategia.Model;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Astrategia.View.Animation
{
    public class FrameAnimation: AAnimation
    {
        private IntRect[] frames;

        public FrameAnimation(IntRect[] frames, Time animationPeriod, AnimationType type, InterpolationMethod method):
            base(0, frames.Count(), animationPeriod, type, method)
        {
            this.frames = frames;
        }       

        public override void Visit(IObject parentObject)
        {
            if (parentObject is IObject2D)
            {
                int index = (int)this.currentValue;

                if (index >= frames.Count())
                {
                    index = frames.Count() - 1;
                }

                (parentObject as IObject2D).Canevas = this.frames[index];
            }
        }
    }
}
