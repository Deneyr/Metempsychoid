using Astrategia.Animation;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Animation
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

        public override void Visit(IObject parentObject)
        {
            Vector2f newPosition = positionFrom * (1 - this.currentValue) + positionTo * this.currentValue;

            parentObject.Position = newPosition;
        }
    }
}
