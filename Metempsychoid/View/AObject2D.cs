using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public abstract class AObject2D: IObject2D
    {
        protected static AnimationManager animationManager;

        protected IObject2DFactory parentFactory;

        protected List<IAnimation> animationsList;

        public abstract float Zoom
        {
            get;
            set;
        }

        public abstract Vector2f CustomZoom
        {
            get;
            set;
        }

        public abstract IntRect Canevas
        {
            get;
            set;
        }

        public abstract FloatRect Bounds
        {
            get;
        }

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

        public abstract Color SpriteColor
        {
            get;
            set;
        }

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();
        }

        public AObject2D(IObject2DFactory parentFactory)
        {
            this.parentFactory = parentFactory;

            this.animationsList = new List<IAnimation>();
        }

        public virtual void Dispose()
        {
            AObject2D.animationManager.StopAnimation(this);

            this.parentFactory = null;
        }

        public abstract void DrawIn(RenderWindow window, Time deltaTime);

        // Part animations.
        public static IntRect[] CreateAnimation(int leftStart, int topStart, int width, int height, int nbFrame)
        {
            IntRect[] result = new IntRect[nbFrame];

            for (int i = 0; i < nbFrame; i++)
            {
                result[i] = new IntRect(leftStart + i * width, topStart, width, height);
            }

            return result;
        }

        public void PlayAnimation(int index)
        {
            IAnimation animation = this.animationsList[index];

            AObject2D.animationManager.PlayAnimation(this, animation);
        }

        public void PlayAnimation(IAnimation animation)
        {
            AObject2D.animationManager.PlayAnimation(this, animation);
        }

        public virtual bool IsAnimationRunning()
        {
            IAnimation animation = AObject2D.animationManager.GetAnimationFromAObject2D(this);

            return animation != null;
        }

        public static void UpdateAnimationManager(Time deltaTime)
        {
            AObject2D.animationManager.Run(deltaTime);
        }
    }
}
