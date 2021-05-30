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

        internal static float SPEED_LINK = 0.4f;

        internal static float SPEED_FILL_RATIO = 0.8f;

        protected StarEntity2D starEntityFrom;
        protected Color currentColorFrom;
        protected float ratioLinkFrom;
        protected float ratioColorFrom;

        protected StarEntity2D starEntityTo;
        protected Color currentColorTo;
        protected float ratioLinkTo;
        protected float ratioColorTo;

        //private float fillRatio;
        private bool isFocused;

        protected RenderStates render;

        //protected Clock timer = new Clock();

        protected StarLinkState starLinkState;

        protected bool isActive;

        //public TargetedFillRatioState FillRatioState
        //{
        //    get;
        //    set;
        //}

        //public float FillRatio
        //{
        //    get
        //    {
        //        return this.fillRatio;
        //    }
        //    protected set
        //    {
        //        if (this.fillRatio != value)
        //        {
        //            this.fillRatio = value;

        //            render.Shader.SetUniform("fillRatio", this.fillRatio);
        //        }
        //    }
        //}

        public bool IsFocused
        {
            get
            {
                return this.isFocused;
            }
            set
            {
                if (this.isFocused != value)
                {
                    this.isFocused = value;

                    render.Shader.SetUniform("isFocused", this.isFocused);
                }
            }
        }

        public float RatioColorFrom
        {
            get
            {
                return this.ratioColorFrom;
            }
            protected set
            {
                if (this.ratioColorFrom != value)
                {
                    this.ratioColorFrom = value;

                    render.Shader.SetUniform("ratioColorFrom", this.ratioColorFrom);
                }
            }
        }

        public float RatioColorTo
        {
            get
            {
                return this.ratioColorTo;
            }
            protected set
            {
                if (this.ratioColorTo != value)
                {
                    this.ratioColorTo = value;

                    render.Shader.SetUniform("ratioColorTo", this.ratioColorTo);
                }
            }
        }


        public Color CurrentColorFrom
        {
            get
            {
                return this.currentColorFrom;
            }
            protected set
            {
                if(this.currentColorFrom != value)
                {
                    this.currentColorFrom = value;

                    render.Shader.SetUniform("rFrom", this.currentColorFrom.R / 255f);
                    render.Shader.SetUniform("gFrom", this.currentColorFrom.G / 255f);
                    render.Shader.SetUniform("bFrom", this.currentColorFrom.B / 255f);
                }
            }
        }

        public Color CurrentColorTo
        {
            get
            {
                return this.currentColorTo;
            }
            protected set
            {
                if (this.currentColorTo != value)
                {
                    this.currentColorTo = value;

                    render.Shader.SetUniform("rTo", this.currentColorTo.R / 255f);
                    render.Shader.SetUniform("gTo", this.currentColorTo.G / 255f);
                    render.Shader.SetUniform("bTo", this.currentColorTo.B / 255f);
                }
            }
        }

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
                }
            }
        }

        public StarLinkEntity2D(ALayer2D parentLayer, StarLinkEntity entity) :
            base(parentLayer, entity)
        {

        }

        public StarLinkEntity2D(ALayer2D layer2D, IObject2DFactory factory, StarLinkEntity entity) :
            base(layer2D, entity)
        {
            this.currentColorFrom = Color.Black;
            this.currentColorTo = Color.Black;
            this.ratioColorFrom = -1;
            this.ratioLinkTo = -1;

            this.starEntityFrom = layer2D.GetEntity2DFromEntity(entity.StarFrom) as StarEntity2D;
            this.starEntityTo = layer2D.GetEntity2DFromEntity(entity.StarTo) as StarEntity2D;

            //this.fillRatio = 0;
            this.isFocused = true;

            this.ObjectSprite.Color = Color.Blue;

            Shader shader = new Shader(null, null, @"Assets\Graphics\Shaders\LinkSimpleFrag.frag");

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
            this.CurrentColorFrom = this.starEntityFrom.ObjectSprite.Color;
            this.RatioColorFrom = 0.33f;
            this.CurrentColorTo = this.starEntityTo.ObjectSprite.Color;
            this.RatioColorTo = 0.33f;

            //this.FillRatio = 0;
            //this.FillRatioState = TargetedFillRatioState.STOP;
            this.IsFocused = false;

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

            this.ratioLinkFrom = 0;
            this.ratioLinkTo = 0;

            this.UpdateRatioShader();
        }

        private void StartActiveState()
        {
            this.starLinkState = StarLinkState.ACTIVE;

            this.ratioLinkFrom = 0.5f;
            this.ratioLinkTo = 0.5f;

            this.UpdateRatioShader();
        }

        private void UpdateTransitioning(Time deltaTime)
        {
            if(this.ratioLinkFrom + this.ratioLinkTo >= 1
                && this.starEntityFrom.StarEntityState == StarEntity2D.StarState.ACTIVE 
                && this.starEntityTo.StarEntityState == StarEntity2D.StarState.ACTIVE)
            {
                this.StartActiveState();
            }
            else if(this.ratioLinkFrom + this.ratioLinkTo <= 0
                && this.starEntityFrom.IsActive == false
                && this.starEntityTo.IsActive == false)
            {
                this.StartNotActiveState();
            }
            else
            {
                if(this.ratioLinkFrom < 0.5
                    && this.starEntityFrom.StarEntityState == StarEntity2D.StarState.ACTIVE
                    && this.isActive)
                {
                    this.ratioLinkFrom += SPEED_LINK * deltaTime.AsSeconds();

                    if(this.ratioLinkFrom > 0.5)
                    {
                        this.ratioLinkFrom = 0.5f;
                    }
                }

                if (this.ratioLinkTo < 0.5
                    && this.starEntityTo.StarEntityState == StarEntity2D.StarState.ACTIVE
                    && this.isActive)
                {
                    this.ratioLinkTo += SPEED_LINK * deltaTime.AsSeconds();

                    if (this.ratioLinkTo > 0.5)
                    {
                        this.ratioLinkTo = 0.5f;
                    }
                }


                if (this.ratioLinkFrom > 0
                    && (this.starEntityFrom.IsActive == false || this.isActive == false))
                {
                    this.ratioLinkFrom -= SPEED_LINK * deltaTime.AsSeconds();

                    if (this.ratioLinkFrom < 0)
                    {
                        this.ratioLinkFrom = 0;
                    }
                }

                if (this.ratioLinkTo > 0
                    && (this.starEntityTo.IsActive == false || this.isActive == false))
                {
                    this.ratioLinkTo -= SPEED_LINK * deltaTime.AsSeconds();

                    if (this.ratioLinkTo < 0)
                    {
                        this.ratioLinkTo = 0;
                    }
                }
            }

            this.UpdateRatioShader();
        }

        private void UpdateRatioShader()
        {
            render.Shader.SetUniform("ratioFrom", this.ratioLinkFrom);
            render.Shader.SetUniform("ratioTo", this.ratioLinkTo);
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

            //this.UpdateLinkFillRatio(deltaTime);

            this.UpdateLinkColor(deltaTime);

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        //protected void UpdateLinkFillRatio(Time deltaTime)
        //{
        //    float currentFillRatio = this.FillRatio;
        //    switch (this.FillRatioState)
        //    {
        //        case TargetedFillRatioState.DOWN:
        //            currentFillRatio -=  SPEED_FILL_RATIO * deltaTime.AsSeconds();

        //            if(currentFillRatio < 0)
        //            {
        //                currentFillRatio = 0;
        //                this.FillRatioState = TargetedFillRatioState.STOP;
        //            }

        //            break;
        //        case TargetedFillRatioState.UP:
        //            currentFillRatio += SPEED_FILL_RATIO * deltaTime.AsSeconds();

        //            if (currentFillRatio > 1)
        //            {
        //                currentFillRatio = 1;
        //                this.FillRatioState = TargetedFillRatioState.STOP;
        //            }
        //            break;
        //    }

        //    this.FillRatio = currentFillRatio;
        //}

        protected void UpdateLinkColor(Time deltaTime)
        {
            if (this.RatioColorFrom < 0.33)
            {
                this.RatioColorFrom += SPEED_LINK * deltaTime.AsSeconds();

                if(this.RatioColorFrom > 0.33)
                {
                    this.RatioColorFrom = 0.33f;
                }
            }

            if (this.ratioColorTo < 0.33)
            {
                this.RatioColorTo += SPEED_LINK * deltaTime.AsSeconds();

                if (this.RatioColorTo > 0.33)
                {
                    this.RatioColorTo = 0.33f;
                }
            }

            if (this.starEntityFrom.ObjectSprite.Color != this.currentColorFrom)
            {
                this.CurrentColorFrom = this.starEntityFrom.ObjectSprite.Color;
                this.RatioColorFrom = 0;
            }

            if (this.starEntityTo.ObjectSprite.Color != this.currentColorTo)
            {
                this.CurrentColorTo = this.starEntityTo.ObjectSprite.Color;
                this.RatioColorTo = 0;
            }
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

        //public enum TargetedFillRatioState
        //{
        //    UP,
        //    DOWN,
        //    STOP
        //}
    }
}
