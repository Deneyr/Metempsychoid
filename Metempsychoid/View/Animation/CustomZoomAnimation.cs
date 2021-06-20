using Metempsychoid.Animation;
using Metempsychoid.Model;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public class CustomZoomAnimation : AAnimation
    {
        Vector2f zoomFrom;
        Vector2f zoomTo;

        public CustomZoomAnimation(Vector2f zoomFrom, Vector2f zoomTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(0, 1, animationPeriod, type, method)
        {
            this.zoomFrom = zoomFrom;
            this.zoomTo = zoomTo;
        }

        public override void Visit(IObject parentObject)
        {
            if (parentObject is IObject2D)
            {
                Vector2f newZoom = zoomFrom * (1 - this.currentValue) + zoomTo * this.currentValue;

                (parentObject as IObject2D).CustomZoom = newZoom;
            }
        }
    }
}
