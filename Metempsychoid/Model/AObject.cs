using Astrategia.Animation;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model
{
    public abstract class AObject: IObject, IUpdatable, IDisposable
    {
        protected static AnimationManager animationManager;

        protected List<IAnimation> animationsList;

        public abstract Vector2f Position
        {
            get;
            set;
        }

        public abstract float Rotation
        {
            get;
            set;
        }

        static AObject()
        {
            AObject.animationManager = new AnimationManager();
        }

        public AObject()
        {
            this.animationsList = new List<IAnimation>();
        }

        public virtual void UpdateLogic(World world, Time deltaTime)
        {
            // To override;
        }

        public void PlayAnimation(int index)
        {
            IAnimation animation = this.animationsList[index];

            AObject.animationManager.PlayAnimation(this, animation);
        }

        public void PlayAnimation(IAnimation animation)
        {
            AObject.animationManager.PlayAnimation(this, animation);
        }

        public void AddAnimation(IAnimation animation)
        {
            this.animationsList.Add(animation);
        }

        public static void UpdateAnimationManager(Time deltaTime)
        {
            AObject.animationManager.Run(deltaTime);
        }

        public virtual void Dispose()
        {
            AObject.animationManager.StopAnimation(this);
        }
    }
}
