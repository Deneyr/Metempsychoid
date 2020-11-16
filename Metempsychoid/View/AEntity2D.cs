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
    public class AEntity2D: AObject2D
    {
        protected Sprite sprite;

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

        public override Vector2f Position
        {
            get
            {
                return this.sprite.Position;
            }

            set
            {
                this.sprite.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public override float Rotation
        {
            get
            {
                return this.sprite.Rotation;
            }

            set
            {
                this.sprite.Rotation = value;
            }
        }

        public override float Zoom
        {
            get
            {
                return this.sprite.Scale.X;
            }
            set
            {
                this.sprite.Scale = new Vector2f(value, value);
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return this.sprite.TextureRect;
            }
            set
            {
                if (this.sprite.TextureRect != value)
                {
                    this.sprite.TextureRect = value;
                }
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                return this.ObjectSprite.GetGlobalBounds();
            }
        }

        public AEntity2D()
        {
            this.sprite = new Sprite();
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite);
        }
    }
}
