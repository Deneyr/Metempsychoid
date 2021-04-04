using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Text2D
{
    public class TextToken2D: AObject2D
    {
        private Text text2D;

        private int textCursor;

        public string FullText
        {
            get;
            private set;
        }

        public int TextCursor
        {
            get
            {
                return this.textCursor;
            }
            set
            {
                if (this.textCursor != value)
                {
                    this.textCursor = value;

                    this.Text = this.FullText.Substring(0, this.TextCursor);
                }
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.text2D.Position;
            }
            set
            {
                this.text2D.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public override float Rotation
        {
            get
            {
                return this.text2D.Rotation;
            }

            set
            {
                this.text2D.Rotation = value;
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return this.text2D.FillColor;
            }
            set
            {
                this.text2D.FillColor = new Color(this.text2D.FillColor.R, this.text2D.FillColor.G, this.text2D.FillColor.B, value.A);
                this.text2D.OutlineColor = new Color(this.text2D.OutlineColor.R, this.text2D.OutlineColor.G, this.text2D.OutlineColor.B, value.A);
            }
        }

        public override float Zoom
        {
            get
            {
                return this.text2D.Scale.X;
            }
            set
            {
                this.text2D.Scale = new Vector2f(value, value);
            }
        }

        public uint CharacterSize
        {
            get
            {
                return this.text2D.CharacterSize;
            }
            set
            {
                this.text2D.CharacterSize = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text2D.DisplayedString;
            }
            set
            {
                this.text2D.DisplayedString = value;
            }
        }

        public override IntRect Canevas
        {
            get
            {
                FloatRect canevas = this.Bounds;
                return new IntRect((int) canevas.Left, (int) canevas.Left, (int) canevas.Width, (int) canevas.Height);
            }
            set
            {
                
            }
        }

        public Font TextFont
        {
            get
            {
                return this.text2D.Font;
            }

            set
            {
                this.text2D.Font = value;
            }
        }

        public virtual bool IsActive
        {
            get;
            set;
        }

        public override FloatRect Bounds
        {
            get
            {
                return this.text2D.GetGlobalBounds();
            }
        }

        public TextToken2D(string text)
        {
            this.FullText = text;

            this.textCursor = -1;

            this.text2D = new Text();

            this.TextCursor = this.FullText.Count();
            //this.text.Font = AObject2DFactory.GetFontByName("Protector");

            //this.Position = new Vector2f(0, 0);
            //this.text.FillColor = Color.White;
            //this.CharacterSize = 80;
            //this.text.OutlineThickness = 2;
            //this.text.OutlineColor = Color.Black;
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.text2D);
            }
        }
    }
}
