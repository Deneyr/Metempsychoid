using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public class PositionAnimation : AAnimation
    {
        Vector2f positionFrom;
        Vector2f positionTo;

        public PositionAnimation(Vector2f positionFrom, Vector2f positionTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(0, 1, animationPeriod, type, method)
        {
            this.positionFrom = positionFrom;
            this.positionTo = positionTo;
        }

        public override void Visit(IObject2D parentObject2D)
        {
            Vector2f newPosition = positionFrom * (1 - this.currentValue) + positionTo * this.currentValue;

            parentObject2D.Position = newPosition;
        }
    }
}
