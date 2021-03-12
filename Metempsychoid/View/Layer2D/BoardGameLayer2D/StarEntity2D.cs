using Metempsychoid.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.View.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class StarEntity2D : AEntity2D
    {
        internal static Color DEFAULT_COLOR = Color.Blue;

        RenderStates render;

        Clock timer = new Clock();

        public StarState StarEntityState
        {
            get;
            private set;
        }

        protected bool isActive;

        public override bool IsActive
        {
            get
            {
                return this.StarEntityState != StarState.NOT_ACTIVE;
            }
            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;

                    if(this.StarEntityState == StarState.TRANSITIONING)
                    {
                        this.PlayAnimation(this.CreateTransitioningAnimation());
                    }
                }
            }
        }

        public StarEntity2D(IObject2DFactory factory, ALayer2D parentLayer, StarEntity entity):
            base(parentLayer, entity)
        {

            Shader shader = new Shader(null, null, @"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\StarFrag.frag");

            Texture distortionMap = factory.GetTextureByIndex(1);
            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            shader.SetUniform("distortionMapTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.SetCardSocketed(entity.CardSocketed);

            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            // Active animation
            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.LOOP);

            IAnimation anim = new ZoomAnimation(1, 1.5f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(1.5f, 1, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(2, anim);

            this.animationsList.Add(sequence);

            // Start : Transitioning active animation
            Random rand = new Random();
            float startTime = (float) (rand.NextDouble() * 2);
            sequence = new SequenceAnimation(Time.FromSeconds(startTime + 2), AnimationType.ONETIME);

            anim = new ZoomAnimation(0, 0, Time.FromSeconds(startTime), AnimationType.ONETIME, InterpolationMethod.STEP);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(0f, 1f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(startTime, anim);

            this.animationsList.Add(sequence);

            this.InitializeState(entity);
        }

        public void SetCardSocketed(CardEntity cardEntity)
        {
            if (cardEntity != null)
            {
                this.ObjectSprite.Color = cardEntity.Card.Player.PlayerColor;
            }
            else
            {
                this.ObjectSprite.Color = DEFAULT_COLOR;
            }
        }

        private IAnimation CreateTransitioningAnimation()
        {
            IAnimation result;
            if (this.isActive)
            {
                float time = Math.Abs(1 - this.Zoom) * 2;
                result = new ZoomAnimation(this.Zoom, 1f, Time.FromSeconds(time), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            }
            else
            {
                float time = Math.Abs(this.Zoom) * 2;
                result = new ZoomAnimation(this.Zoom, 0f, Time.FromSeconds(time), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            }

            return result;
        }

        private void InitializeState(StarEntity entity)
        {
            this.isActive = entity.IsActive;
            if (this.isActive)
            {
                this.PlayAnimation(1);

                this.StarEntityState = StarState.TRANSITIONING;
            }
            else
            {
                this.StartNotActiveState();
            }
        }

        private void StartTransitioningState()
        {
            this.StarEntityState = StarState.TRANSITIONING;

            this.PlayAnimation(this.CreateTransitioningAnimation());
        }

        private void StartNotActiveState()
        {
            this.StarEntityState = StarState.NOT_ACTIVE;
        }

        private void StartActiveState()
        {
            this.StarEntityState = StarState.ACTIVE;

            this.PlayAnimation(0);
        }

        private void UpdateTransitioning(Time deltaTime)
        {
            if (this.IsAnimationRunning() == false)
            {
                if(this.isActive)
                {
                    this.StartActiveState();
                }
                else
                {
                    this.StartNotActiveState();
                }
            }
        }

        private void UpdateActive(Time deltaTime)
        {
            if (this.isActive == false)
            {
                this.StartTransitioningState();
            }
        }

        private void UpdateNotActive(Time deltaTime)
        {
            if (this.isActive)
            {
                this.StartTransitioningState();
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.StarEntityState)
            {
                case StarState.ACTIVE:
                    this.UpdateActive(deltaTime);
                    break;
                case StarState.TRANSITIONING:
                    this.UpdateTransitioning(deltaTime);
                    break;
                case StarState.NOT_ACTIVE:
                    this.UpdateNotActive(deltaTime);
                    break;
            }

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite, this.render);
        }

        public enum StarState
        {
            NOT_ACTIVE,
            TRANSITIONING,
            ACTIVE
        }
    }
}
