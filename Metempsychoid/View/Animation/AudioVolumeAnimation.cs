using Astrategia.Animation;
using Astrategia.Model;
using Astrategia.View.SoundsManager;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Animation
{
    public class AudioVolumeAnimation : AAnimation
    {
        public AudioVolumeAnimation(float volumeFrom, float volumeTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(volumeFrom, volumeTo, animationPeriod, type, method)
        {
        }

        public override void Visit(IObject parentObject)
        {
            if (parentObject is AAudioObject2D)
            {
                (parentObject as AAudioObject2D).Volume = this.currentValue;
            }
        }
    }
}
