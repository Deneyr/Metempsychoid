using Astrategia.Model;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Astrategia.Animation
{
    public class AnimationManager
    {
        private Dictionary<IObject, IAnimation> animationsToPlay;

        public AnimationManager()
        {
            this.animationsToPlay = new Dictionary<IObject, IAnimation>();
        }

        public void Run(Time deltaTime)
        {
            List<IObject> finishedAnimation = new List<IObject>();

            foreach (KeyValuePair<IObject, IAnimation> keyValuePair in this.animationsToPlay)
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

            foreach (IObject obj in finishedAnimation)
            {
                this.animationsToPlay.Remove(obj);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject obj)
        {

            IAnimation animation = null;

            if (this.animationsToPlay.ContainsKey(obj))
            {
                animation = this.animationsToPlay[obj];
            }

            return animation;
        }

        public void PlayAnimation(IObject obj, IAnimation animation)
        {
            animation.Reset();

            if (this.animationsToPlay.ContainsKey(obj))
            {
                this.animationsToPlay[obj] = animation;
            }
            else
            {
                this.animationsToPlay.Add(obj, animation);
            }
        }

        public void StopAnimation(IObject obj)
        {
            this.animationsToPlay.Remove(obj);
        }


    }
}
