using Astrategia.Animation;
using Astrategia.View.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BackgroundLayer2D
{
    public class ImageBackgroundObject2D : AEntity2D
    {
        protected string nextImageId;

        public override bool IsActive
        {
            get
            {
                return this.ObjectSprite.Texture != null;
            }
        }

        protected TransitionState ImageTransitionState
        {
            get;
            set;
        }

        public ImageBackgroundObject2D(ALayer2D parentLayer) 
            : base(parentLayer, null, true)
        {
            this.nextImageId = null;

            this.ImageTransitionState = TransitionState.FINISHED;

            this.ObjectSprite.Texture = null;
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.ImageTransitionState)
            {
                case TransitionState.STARTING:
                    this.UpdateStarting(deltaTime);
                    break;
                case TransitionState.ENDING:
                    this.UpdateEnding(deltaTime);
                    break;
                case TransitionState.FINISHED:
                    this.UpdateFinished(deltaTime);
                    break;
            }
        }

        public void DisplayImage(string newImage)
        {
            this.nextImageId = newImage;

            this.InitStarting();
        }

        protected virtual void InitStarting()
        {
            if (this.ObjectSprite.Texture != null)
            {
                this.SpriteColor = new Color(255, 255, 255);

                IAnimation startingAnimation = new ColorAnimation(this.SpriteColor, new Color(255, 255, 255, 0), Time.FromSeconds(0.5f), AnimationType.ONETIME, InterpolationMethod.LINEAR);

                this.PlayAnimation(startingAnimation);

                this.ImageTransitionState = TransitionState.STARTING;
            }
            else
            {
                this.InitEnding();
            }
        }

        protected virtual void InitEnding()
        {
            if (string.IsNullOrEmpty(this.nextImageId) == false)
            {
                if (this.parentLayer.TryGetTarget(out ALayer2D parentLayer))
                {
                    Texture nextTexture = parentLayer.GetLayerTextureFromId(this.nextImageId);
                    this.ObjectSprite.Texture = nextTexture;

                    this.ObjectSprite.Origin = new Vector2f(this.Canevas.Width / 2, this.Canevas.Height / 2);

                    if (this.ObjectSprite.Texture != null)
                    {
                        this.SpriteColor = new Color(255, 255, 255, 0);

                        IAnimation endingAnimation = new ColorAnimation(this.SpriteColor, new Color(255, 255, 255), Time.FromSeconds(0.5f), AnimationType.ONETIME, InterpolationMethod.LINEAR);

                        this.PlayAnimation(endingAnimation);
                    }
                }

                this.nextImageId = null;

                this.ImageTransitionState = TransitionState.ENDING;
            }
            else
            {
                this.InitFinished();
            }
        }

        protected virtual void InitFinished()
        {
            this.SpriteColor = new Color(255, 255, 255);

            this.ImageTransitionState = TransitionState.ENDING;
        }

        protected virtual void UpdateStarting(Time deltaTime)
        {
            if(this.IsAnimationRunning() == false)
            {
                this.InitEnding();
            }
        }

        protected virtual void UpdateEnding(Time deltaTime)
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitEnding();
            }
        }

        protected virtual void UpdateFinished(Time deltaTime)
        {
            // nothing to do.
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.ObjectSprite);
            }
        }

        protected enum TransitionState
        {
            STARTING,
            ENDING,
            FINISHED
        }
    }
}
