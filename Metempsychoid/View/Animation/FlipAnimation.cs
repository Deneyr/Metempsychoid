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
    public class FlipAnimation : ZoomAnimation
    {
        private float length;

        public FlipAnimation(float zoomFrom, float zoomTo, Time animationPeriod, AnimationType type, InterpolationMethod method, float length) :
            base(zoomFrom, zoomTo, animationPeriod, type, method)
        {
            this.length = length;
        }

        public override void Visit(IObject parentObject)
        {
            if (parentObject is IObject2D)
            {
                IObject2D object2D = parentObject as IObject2D;
                object2D.CustomZoom = new Vector2f((float) (Math.Cos(this.currentValue) * this.length), object2D.CustomZoom.Y);
            }
        }
    }
}
