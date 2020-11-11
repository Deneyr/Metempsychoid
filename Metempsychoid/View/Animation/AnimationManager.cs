using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Metempsychoid.View.Animation
{
    public class AnimationManager
    {
        private Dictionary<IObject2D, IAnimation> animationsToPlay;

        public AnimationManager()
        {
            this.animationsToPlay = new Dictionary<IObject2D, IAnimation>();
        }

        public void Run(Time deltaTime)
        {
            List<IObject2D> finishedAnimation = new List<IObject2D>();

            foreach (KeyValuePair<IObject2D, IAnimation> keyValuePair in this.animationsToPlay)
            {
                if (keyValuePair.Value.State == AnimationState.ENDING)
                {
                    finishedAnimation.Add(keyValuePair.Key);
                }
                else
                {
                    keyValuePair.Value.Run(deltaTime);

                    keyValuePair.Value.Visit(keyValuePair.Key);
                }
            }

            foreach (IObject2D object2D in finishedAnimation)
            {
                this.animationsToPlay.Remove(object2D);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject2D object2D)
        {

            IAnimation animation = null;

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                animation = this.animationsToPlay[object2D];
            }

            return animation;
        }

        public void PlayAnimation(IObject2D object2D, IAnimation animation)
        {
            animation.Reset();

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                this.animationsToPlay[object2D] = animation;
            }
            else
            {
                this.animationsToPlay.Add(object2D, animation);
            }
        }


    }
}
