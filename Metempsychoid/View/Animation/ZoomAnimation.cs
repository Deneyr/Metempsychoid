using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public class ZoomAnimation : AAnimation
    {
        public ZoomAnimation(float zoomFrom, float zoomTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(zoomFrom, zoomTo, animationPeriod, type, method)
        {
        }

        public override void Visit(IObject2D parentObject2D)
        {
            parentObject2D.Zoom = this.currentValue;
        }
    }
}
