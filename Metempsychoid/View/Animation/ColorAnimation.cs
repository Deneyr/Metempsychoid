using Metempsychoid.Animation;
using Metempsychoid.Model;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public class ColorAnimation : AAnimation
    {
        private Color colorFrom;
        private Color colorTo;

        public ColorAnimation(Color colorFrom, Color colorTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(0, 1, animationPeriod, type, method)
        {
            this.colorFrom = colorFrom;
            this.colorTo = colorTo;
        }

        public override void Visit(IObject parentObject)
        {
            if (parentObject is IObject2D)
            {
                Color newColor = new Color(
                    (byte) (colorFrom.R * (1 - this.currentValue) + colorTo.R * this.currentValue),
                    (byte) (colorFrom.G * (1 - this.currentValue) + colorTo.G * this.currentValue),
                    (byte) (colorFrom.B * (1 - this.currentValue) + colorTo.B * this.currentValue),
                    (byte) (colorFrom.A * (1 - this.currentValue) + colorTo.A * this.currentValue));

                (parentObject as IObject2D).SpriteColor = newColor;
            }
        }
    }
}
