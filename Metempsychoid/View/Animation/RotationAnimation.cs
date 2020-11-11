﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public class RotationAnimation : AAnimation
    {
        public RotationAnimation(float rotationFrom, float rotationTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(rotationFrom, rotationTo, animationPeriod, type, method)
        {
        }

        public override void Visit(IObject2D parentObject2D)
        {
            parentObject2D.Rotation = this.currentValue;
        }
    }
}
