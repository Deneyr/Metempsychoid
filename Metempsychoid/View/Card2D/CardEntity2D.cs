using Metempsychoid.Model.Card;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardEntity2D: AEntity2D
    {
        private int WIDTH_BORDER = 15;

        internal static float SPEED_LINK = 0.6f;

        private Sprite canevasSprite;

        private bool isSocketed;

        private Color playerColor;

        protected float ratioColor;

        RenderStates render;

        Clock timer = new Clock();

        public float RatioColor
        {
            get
            {
                return this.ratioColor;
            }
            protected set
            {
                if (this.ratioColor != value)
                {
                    this.ratioColor = value;

                    render.Shader.SetUniform("ratioColor", this.ratioColor);
                }
            }
        }

        public Color PlayerColor
        {
            get
            {
                return this.playerColor;
            }
            protected set
            {
                if (this.playerColor != value)
                {
                    this.playerColor = value;

                    render.Shader.SetUniform("playerColor", new SFML.Graphics.Glsl.Vec4(this.playerColor.R / 255f, this.playerColor.G / 255f, this.playerColor.B / 255f, 1));

                    if (this.isSocketed)
                    {
                        this.RatioColor = 0;
                    }
                }
            }
        }

        public override Vector2f Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;

                this.canevasSprite.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }

            set
            {
                base.Rotation = value;

                this.canevasSprite.Rotation = value;
            }
        }

        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {
                base.Zoom = value;

                this.canevasSprite.Scale = new Vector2f(value, value);
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return this.canevasSprite.TextureRect;
            }
            set
            {
                base.Canevas = value;

                this.canevasSprite.TextureRect = new IntRect(0, 0, this.ObjectSprite.TextureRect.Width + 2 * WIDTH_BORDER, this.ObjectSprite.TextureRect.Height + 2 * WIDTH_BORDER);
            }
        }

        public bool IsSocketed
        {
            get
            {
                return this.isSocketed;
            }

            set
            {
                if(this.isSocketed != value)
                {
                    this.isSocketed = value;

                    render.Shader.SetUniform("isSocketed", this.isSocketed);

                    this.RatioColor = 0;
                }
            }
        }


        public CardEntity2D(IObject2DFactory factory, CardEntity entity) :
            base()
        {
            this.ObjectSprite.Texture = factory.GetTextureByIndex(2);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            this.canevasSprite = new Sprite();

            this.canevasSprite.TextureRect = new IntRect(0, 0, this.ObjectSprite.TextureRect.Width + 2 * WIDTH_BORDER, this.ObjectSprite.TextureRect.Height + 2 * WIDTH_BORDER);

            this.canevasSprite.Origin = new SFML.System.Vector2f(this.canevasSprite.TextureRect.Width / 2, this.canevasSprite.TextureRect.Height / 2);

            this.Position = entity.Position;
            this.Rotation = entity.Rotation;

            this.canevasSprite.Color = Color.Black;
            this.playerColor = Color.Black;
            this.isSocketed = !entity.IsSocketed;
            this.ratioColor = -1;

            Shader shader = new Shader(null, null, @"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\cardCanevas.frag");

            Texture distortionMap = factory.GetTextureByIndex(0);
            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            this.canevasSprite.Texture = distortionMap;
            shader.SetUniform("distortionMapTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.Initialize(entity);

            this.UpdateScaling();
        }

        private void Initialize(CardEntity entity)
        {
            this.PlayerColor = entity.Card.Player.PlayerColor;
            this.IsSocketed = entity.IsSocketed;
            this.RatioColor = 1;
        }

        protected virtual void UpdateScaling()
        {
            render.Shader.SetUniform("widthRatio", ((float) this.canevasSprite.TextureRect.Width) / this.canevasSprite.Texture.Size.X);
            render.Shader.SetUniform("heightRatio", ((float) this.canevasSprite.TextureRect.Height) / this.canevasSprite.Texture.Size.Y);

            render.Shader.SetUniform("margin", ((float) (WIDTH_BORDER - 5)) / this.canevasSprite.Texture.Size.X);
            render.Shader.SetUniform("outMargin", 5f/ this.canevasSprite.Texture.Size.X);
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            this.UpdateColorRatio(deltaTime);

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        private void UpdateColorRatio(Time deltaTime)
        {
            if (this.RatioColor < 1)
            {
                this.RatioColor += SPEED_LINK * deltaTime.AsSeconds();

                if (this.RatioColor > 1)
                {
                    this.RatioColor = 1f;
                }
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite);

            window.Draw(this.canevasSprite, this.render);
        }
    }
}
