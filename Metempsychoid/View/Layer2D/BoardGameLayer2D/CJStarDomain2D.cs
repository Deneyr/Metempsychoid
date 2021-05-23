using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.View.Animation;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class CJStarDomain2D : AEntity2D
    {
        private static int NB_MAX_POINTS = 20;
        private static int MARGIN_DOMAIN = 200;

        protected DomainState domainState;

        protected RenderStates render;

        protected Clock timer = new Clock();

        protected List<Vector2f> domainPoints;
        protected List<StarEntity2D> domainStars;

        //private float widthRatio;

        //private float heightRatio;

        private Color targetedColor;

        public override bool IsActive
        {
            get
            {
                return this.domainState != DomainState.NOT_ACTIVE;
            }
            set
            {

            }
        }

        //public float WidthRatio
        //{
        //    get
        //    {
        //        return this.widthRatio;
        //    }
        //    private set
        //    {
        //        if(this.widthRatio != value)
        //        {
        //            this.widthRatio = value;

        //            render.Shader.SetUniform("widthRatio", value);
        //        }
        //    }
        //}

        //public float HeightRatio
        //{
        //    get
        //    {
        //        return this.heightRatio;
        //    }
        //    private set
        //    {
        //        if (this.heightRatio != value)
        //        {
        //            this.heightRatio = value;

        //            render.Shader.SetUniform("heightRatio", value);
        //        }
        //    }
        //}

        public Color TargetedColor
        {
            get
            {
                return this.targetedColor;
            }
            set
            {
                if (this.targetedColor != value)
                {
                    this.targetedColor = value;

                    this.PlayAnimation(new ColorAnimation(base.SpriteColor, this.TargetedColor, Time.FromSeconds(2), Metempsychoid.Animation.AnimationType.ONETIME, Metempsychoid.Animation.InterpolationMethod.LINEAR));
                }
            }
        }

        public List<Vector2f> DomainPoints
        {
            get
            {
                return this.domainPoints;
            }
            private set
            {
                this.domainPoints = value;

                Vec2[] points = new Vec2[NB_MAX_POINTS];
                int i = 0;
                foreach(Vector2f point in this.domainPoints)
                {
                    points[i] = new Vec2((point.X - this.Position.X) / this.ObjectSprite.Texture.Size.X, (point.Y - this.Position.Y) / this.ObjectSprite.Texture.Size.Y);
                    i++;
                }

                this.render.Shader.SetUniformArray("points", points);
                this.render.Shader.SetUniform("pointsLen", this.domainPoints.Count);
            }
        }

        public CJStarDomain2D(ALayer2D layer2D, IObject2DFactory factory, CJStarDomain entity) :
            base(layer2D, entity)
        {
            //this.widthRatio = 0;
            //this.heightRatio = 0;

            this.targetedColor = Color.Black;

            Shader shader = new Shader(null, null, @"Assets\Graphics\Shaders\StarDomain.frag");

            Texture distortionMap = factory.GetTextureByIndex(0);
            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);
            this.ObjectSprite.Texture.Repeated = true;

            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.Priority = -1;

            this.domainStars = entity.Domain.Select(pElem => layer2D.GetEntity2DFromEntity(pElem) as StarEntity2D).ToList();
            shader.SetUniform("isFilled", entity.IsFilled);

            this.UpdateScaling(entity);

            this.StartNotActiveState();
        }

        protected virtual void UpdateScaling(CJStarDomain entity)
        {
            Vector2f topLeft = new Vector2f(0, 0);
            Vector2f bottomRight = new Vector2f(0, 0);

            bool firstPoint = true;

            foreach(StarEntity starEntity in entity.Domain)
            {
                if(firstPoint || topLeft.X > starEntity.Position.X)
                {
                    topLeft.X = starEntity.Position.X;
                }

                if (firstPoint || topLeft.Y > starEntity.Position.Y)
                {
                    topLeft.Y = starEntity.Position.Y;
                }

                if (firstPoint || bottomRight.X < starEntity.Position.X)
                {
                    bottomRight.X = starEntity.Position.X;
                }

                if (firstPoint || bottomRight.Y < starEntity.Position.Y)
                {
                    bottomRight.Y = starEntity.Position.Y;
                }

                firstPoint = false;
            }

            float width = bottomRight.X - topLeft.X + MARGIN_DOMAIN * 2;
            float height = bottomRight.Y - topLeft.Y + MARGIN_DOMAIN * 2;
            topLeft -= new Vector2f(MARGIN_DOMAIN, MARGIN_DOMAIN);

            this.ObjectSprite.TextureRect = new IntRect(0, 0, (int) width, (int) height);
            this.ObjectSprite.Position = topLeft;

            //this.WidthRatio = ((float)width) / this.ObjectSprite.Texture.Size.X;
            //this.HeightRatio = ((float)height) / this.ObjectSprite.Texture.Size.Y;

            this.DomainPoints = entity.Domain.Select(pElem => pElem.Position).ToList();

            this.render.Shader.SetUniform("margin", ((float)MARGIN_DOMAIN) / this.ObjectSprite.Texture.Size.X);
        }

        private void StartNotActiveState()
        {
            this.domainState = DomainState.NOT_ACTIVE;

            Color newColor = this.TargetedColor;
            newColor.A = 0;
            this.TargetedColor = newColor;
        }

        private void StartActiveState()
        {
            this.domainState = DomainState.ACTIVE;

            Color newColor = this.TargetedColor;
            newColor.A = 255;
            this.TargetedColor = newColor;
        }

        private void UpdateNotActive(Time deltaTime)
        {
            StarEntity2D starNotActive = this.domainStars.FirstOrDefault(pElem => pElem.StarEntityState != StarEntity2D.StarState.ACTIVE);

            if(starNotActive == null)
            {
                this.StartActiveState();
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());

            switch (this.domainState)
            {
                case DomainState.ACTIVE:

                    break;
                case DomainState.NOT_ACTIVE:
                    this.UpdateNotActive(deltaTime);
                    break;
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.ObjectSprite, this.render);
            }
        }

        public enum DomainState
        {
            NOT_ACTIVE,
            ACTIVE
        }
    }
}
