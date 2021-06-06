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
        protected Text text2D;

        private int textCursor;

        private IntRect canevas;

        private string fullText;

        public int ParameterIndex
        {
            get;
            private set;
        }

        public string FullText
        {
            get
            {
                return fullText;
            }
            set
            {
                if (this.fullText != value)
                {
                    this.fullText = value;
                    this.textCursor = 0;
                    this.TextCursor = this.fullText.Count();

                    this.UpdateCanevas();
                }
            }
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
                this.text2D.Position = new Vector2f((int) (this.canevas.Width / 2 + value.X), (int) (this.canevas.Height / 2 + value.Y)) * MainWindow.MODEL_TO_VIEW;
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
                if (this.text2D.CharacterSize != value)
                {
                    this.text2D.CharacterSize = value;

                    this.UpdateCanevas();
                }
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
                return this.canevas;
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
            get
            {
                return this.FullText != "\n";
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                return this.text2D.GetGlobalBounds();
            }
        }

        public TextToken2D(string text, Color fillColor)
        {
            this.fullText = text;

            this.textCursor = -1;

            this.text2D = new Text();

            if(text.StartsWith("{") && text.EndsWith("}") && int.TryParse(text.Substring(1, text.Length - 2), out int parameterIndex))
            {
                this.ParameterIndex = parameterIndex;
            }
            else
            {
                this.ParameterIndex = -1;
            }

            this.TextCursor = this.FullText.Count();
            this.text2D.Font = AObject2DFactory.GetFontByName("Sans");

            this.text2D.FillColor = fillColor;
            this.text2D.OutlineThickness = 2;
            this.text2D.OutlineColor = Color.Black;

            this.UpdateCanevas();
        }

        private void UpdateCanevas()
        {
            int currentCursor = this.TextCursor;

            this.TextCursor = this.FullText.Count();
            FloatRect canevas = this.Bounds;
            this.canevas = new IntRect((int)canevas.Left, (int)canevas.Left, (int)canevas.Width, (int)canevas.Height);

            this.TextCursor = currentCursor;

            this.text2D.Origin = new Vector2f((int) (canevas.Width / 2), (int) (canevas.Height / 2));
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
