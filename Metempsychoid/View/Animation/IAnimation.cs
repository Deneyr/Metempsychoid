using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public interface IAnimation
    {
        AnimationState State
        {
            get;
        }

        void Run(Time deltaTime);

        void Reset();

        void Stop(bool reset);

        void Visit(IObject2D parentObject2D);
    }

    public enum AnimationState
    {
        STARTING,
        RUNNING,
        FINALIZING,
        ENDING
    }

    public enum AnimationType
    {
        ONETIME,
        LOOP
    }

    public enum InterpolationMethod
    {
        STEP,
        LINEAR,
        SQUARE_ACC,
        SQUARE_DEC
    }
}
