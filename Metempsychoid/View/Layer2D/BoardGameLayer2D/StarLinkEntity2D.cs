using Metempsychoid.Animation;
using Metempsychoid.Maths;
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
    public class StarLinkEntity2D: AEntity2D
    {
        internal static int WIDTH_LINK = 50;

        internal static float SPEED_LINK = 0.5f;

        protected StarEntity2D starEntityFrom;
        protected float ratioFrom;

        protected StarEntity2D starEntityTo;
        protected float ratioTo;

        protected RenderStates render;

        protected Clock timer = new Clock();

        protected StarLinkState starLinkState;

        protected bool isActive;

        public override bool IsActive
        {
            get
            {
                return this.starLinkState != StarLinkState.NOT_ACTIVE;
            }
            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;

                    if (this.starLinkState == StarLinkState.TRANSITIONING)
                    {

                    }
                }
            }
        }

        public StarLinkEntity2D(StarLinkEntity entity) :
            base(entity)
        {

        }

        public StarLinkEntity2D(ALayer2D layer2D, IObject2DFactory factory, StarLinkEntity entity) :
            base(entity)
        {

            this.starEntityFrom = layer2D.GetEntity2DFromEntity(entity.StarFrom) as StarEntity2D;
            this.starEntityTo = layer2D.GetEntity2DFromEntity(entity.StarTo) as StarEntity2D;

            this.ObjectSprite.Color = Color.Blue;

            Shader shader = new Shader(null, null, @"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\LinkSimpleFrag.frag");

            Texture distortionMap = factory.GetTextureByIndex(0);
            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);
            this.ObjectSprite.Texture.Repeated = true;

            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            shader.SetUniform("distTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.UpdateScaling();

            this.Priority = 9;

            this.InitializeState(entity);
        }

        protected virtual void UpdateScaling()
        {
            if (this.starEntityTo != null && this.starEntityFrom != null)
            {
                int width = (int)(this.starEntityTo.Position - this.starEntityFrom.Position).Len();

                this.ObjectSprite.TextureRect = new IntRect(0, 0, width, StarLinkEntity2D.WIDTH_LINK);

                this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

                render.Shader.SetUniform("widthRatio", ((float) width) / this.ObjectSprite.Texture.Size.X);
            }
        }

        public override Vector2f Position
        {
            set
            {
                base.Position = value;

                this.UpdateScaling();
            }
        }

        public override float Rotation
        {
            set
            {
                base.Rotation = value;

                this.UpdateScaling();
            }
        }

        protected void InitializeState(StarLinkEntity entity)
        {
            this.isActive = entity.IsActive;

            this.StartNotActiveState();
        }

        private void StartTransitioningState()
        {
            this.starLinkState = StarLinkState.TRANSITIONING;
        }

        private void StartNotActiveState()
        {
            this.starLinkState = StarLinkState.NOT_ACTIVE;

            this.ratioFrom = 0;
            this.ratioTo = 0;

            this.UpdateRatioShader();
        }

        private void StartActiveState()
        {
            this.starLinkState = StarLinkState.ACTIVE;

            this.ratioFrom = 0.5f;
            this.ratioTo = 0.5f;

            this.UpdateRatioShader();
        }

        private void UpdateTransitioning(Time deltaTime)
        {
            if(this.ratioFrom + this.ratioTo >= 1
                && this.starEntityFrom.StarEntityState == StarEntity2D.StarState.ACTIVE 
                && this.starEntityTo.StarEntityState == StarEntity2D.StarState.ACTIVE)
            {
                this.StartActiveState();
            }
            else if(this.ratioFrom + this.ratioTo <= 0
                && this.starEntityFrom.IsActive == false
                && this.starEntityTo.IsActive == false)
            {
                this.StartNotActiveState();
            }
            else
            {
                if(this.ratioFrom < 0.5
                    && this.starEntityFrom.StarEntityState == StarEntity2D.StarState.ACTIVE
                    && this.isActive)
                {
                    this.ratioFrom += SPEED_LINK * deltaTime.AsSeconds();

                    if(this.ratioFrom > 0.5)
                    {
                        this.ratioFrom = 0.5f;
                    }
                }

                if (this.ratioTo < 0.5
                    && this.starEntityTo.StarEntityState == StarEntity2D.StarState.ACTIVE
                    && this.isActive)
                {
                    this.ratioTo += SPEED_LINK * deltaTime.AsSeconds();

                    if (this.ratioTo > 0.5)
                    {
                        this.ratioTo = 0.5f;
                    }
                }


                if (this.ratioFrom > 0
                    && (this.starEntityFrom.IsActive == false || this.isActive == false))
                {
                    this.ratioFrom -= SPEED_LINK * deltaTime.AsSeconds();

                    if (this.ratioFrom < 0)
                    {
                        this.ratioFrom = 0;
                    }
                }

                if (this.ratioTo > 0
                    && (this.starEntityTo.IsActive == false || this.isActive == false))
                {
                    this.ratioTo -= SPEED_LINK * deltaTime.AsSeconds();

                    if (this.ratioTo < 0)
                    {
                        this.ratioTo = 0;
                    }
                }
            }

            this.UpdateRatioShader();
        }

        private void UpdateRatioShader()
        {
            render.Shader.SetUniform("ratioFrom", this.ratioFrom);
            render.Shader.SetUniform("ratioTo", this.ratioTo);
        }

        private void UpdateActive(Time deltaTime)
        {
            if (this.isActive == false
                || this.starEntityFrom.IsActive == false
                || this.starEntityTo.IsActive == false)
            {
                this.StartTransitioningState();

                render.Shader.SetUniform("isActive", false);
            }
        }

        private void UpdateNotActive(Time deltaTime)
        {
            if (this.isActive)
            {
                if(this.starEntityFrom.StarEntityState == StarEntity2D.StarState.ACTIVE
                    || this.starEntityTo.StarEntityState == StarEntity2D.StarState.ACTIVE)
                {
                    this.StartTransitioningState();

                    render.Shader.SetUniform("isActive", true);
                }
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.starLinkState)
            {
                case StarLinkState.ACTIVE:
                    this.UpdateActive(deltaTime);
                    break;
                case StarLinkState.TRANSITIONING:
                    this.UpdateTransitioning(deltaTime);
                    break;
                case StarLinkState.NOT_ACTIVE:
                    this.UpdateNotActive(deltaTime);
                    break;
            }

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite, this.render);
        }

        public enum StarLinkState
        {
            NOT_ACTIVE,
            TRANSITIONING,
            ACTIVE
        }

    }
}
