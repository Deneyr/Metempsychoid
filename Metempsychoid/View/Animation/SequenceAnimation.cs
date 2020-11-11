using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Metempsychoid.View.Animation
{
    public class SequenceAnimation : IAnimation
    {
        public SequenceAnimation()
        {
            this.State = AnimationState.STARTING;
        }

        public AnimationState State
        {
            get;
            private set;
        }

        public void Reset()
        {
            
        }

        public void Run(Time deltaTime)
        {
            
        }

        public void Stop(bool reset)
        {
            
        }

        public void Visit(IObject2D parentObject2D)
        {
            
        }
    }
}
