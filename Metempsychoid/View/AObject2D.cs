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

        protected static ZoomAnimationManager zoomAnimationManager;

        protected static RectangleShape filter;

        protected Sprite sprite;

        protected List<IAnimation> animationsList;


        public Sprite ObjectSprite
        {
            get
            {
                return this.sprite;
            }

            protected set
            {
                this.sprite = value;
            }
        }

        public Vector2f Position
        {
            get
            {
                return this.ObjectSprite.Position;
            }

            set
            {
                this.ObjectSprite.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public float Rotation
        {
            get
            {
                return this.ObjectSprite.Rotation;
            }

            set
            {
                this.ObjectSprite.Rotation = value;
            }
        }

        public virtual FloatRect Bounds
        {
            get
            {
                return this.ObjectSprite.GetGlobalBounds();
            }
        }

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();

            AObject2D.zoomAnimationManager = new ZoomAnimationManager();

            AObject2D.filter = new RectangleShape(new Vector2f(MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW));
        }

        public AObject2D()
        {
            this.sprite = new Sprite();

            this.animationsList = new List<IAnimation>();
        }

        public virtual void Dispose()
        {
            // To override
        }

        public virtual void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite);
        }

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

            if (animation is ZoomAnimation)
            {
                AObject2D.zoomAnimationManager.PlayAnimation(this, animation as ZoomAnimation);
            }
            else
            {
                AObject2D.animationManager.PlayAnimation(this, animation);
            }
        }

        public static void StopAnimationManager()
        {
            AObject2D.animationManager.Play = false;
        }

        public static void UpdateZoomAnimationManager(Time deltaTime)
        {
            AObject2D.zoomAnimationManager.Run(deltaTime);
        }

        public virtual void SetCanevas(IntRect newCanevas)
        {
            this.sprite.TextureRect = newCanevas;
        }

        public void SetZoom(float newZoom)
        {
            this.sprite.Scale = new Vector2f(newZoom, newZoom);
        }
    }
}
