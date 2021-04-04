using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Text2D
{
    public class TextParagraph2D: AObject2D
    {
        private Vector2f realPosition;
        private Vector2f positionOffset;

        private float realRotation;
        private float rotationOffset;

        private Color spriteColor;

        private float zoom;

        private bool isActive;

        private uint characterSize;

        private IntRect realCanevas;

        private List<TextToken2D> textToken2Ds;

        public override Vector2f Position
        {
            get
            {
                return this.realPosition;
            }
            set
            {
                Vector2f newPosition = value + this.positionOffset;
                if(this.realPosition != newPosition)
                {
                    this.realPosition = newPosition;

                    this.UpdateTextTokensToFitCanevas();
                }
            }
        }

        public override float Rotation
        {
            get
            {
                return this.realRotation;
            }
            set
            {
                float newRotation = value + this.rotationOffset;
                if (this.realRotation != newRotation)
                {
                    this.realRotation = newRotation;
                }
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return this.spriteColor;
            }
            set
            {
                if (this.spriteColor != value)
                {
                    this.spriteColor = value;

                    foreach (TextToken2D textToken2D in this.textToken2Ds)
                    {
                        textToken2D.SpriteColor = this.spriteColor;
                    }
                }
            }
        }

        public override float Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                if (this.zoom != value)
                {
                    this.zoom = value;

                    foreach (TextToken2D textToken2D in this.textToken2Ds)
                    {
                        textToken2D.Zoom = this.zoom;
                    }
                }
            }
        }

        public uint CharacterSize
        {
            get
            {
                return this.characterSize;
            }
            set
            {
                if (this.characterSize != value)
                {
                    this.characterSize = value;

                    foreach (TextToken2D textToken2D in this.textToken2Ds)
                    {
                        textToken2D.CharacterSize = this.characterSize;
                    }

                    this.UpdateTextTokensToFitCanevas();
                }
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return this.realCanevas;
            }
            set
            {
                int top = (int) Math.Min(value.Top + value.Height, value.Top + this.positionOffset.Y);
                int left = (int) Math.Min(value.Left + value.Width, value.Left + this.positionOffset.X);

                int height = value.Top + value.Height - top;
                int width = value.Left + value.Width - left;

                IntRect newCanevas = new IntRect(left, top, width, height);

                if(this.realCanevas != newCanevas)
                {
                    this.realCanevas = newCanevas;

                    this.UpdateTextTokensToFitCanevas();
                }
            }
        }

        public virtual bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if(this.isActive != value)
                {
                    this.isActive = value;

                    foreach (TextToken2D textToken2D in this.textToken2Ds)
                    {
                        textToken2D.IsActive = this.isActive;
                    }
                }
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                IntRect canevas = this.Canevas;
                Vector2f position = this.Position;
                return new FloatRect(position.X + canevas.Left, position.Y + canevas.Top, canevas.Width, canevas.Height);
            }
        }

        public TextParagraph2D(TextCanevas2D textCanevas2D, Vector2f positionOffset, float rotationOffset, uint characterSize)
        {
            this.positionOffset = positionOffset;
            this.rotationOffset = rotationOffset;

            this.isActive = true;

            this.textToken2Ds = new List<TextToken2D>();

            this.Position = textCanevas2D.Position;
            this.Rotation = textCanevas2D.Rotation;

            this.Canevas = textCanevas2D.Canevas;

            this.CharacterSize = characterSize;
            this.Zoom = textCanevas2D.Zoom;
        }

        public void UpdateTextTokens(List<TextToken2D> textTokens)
        {
            this.textToken2Ds = textTokens;

            foreach (TextToken2D textToken in this.textToken2Ds)
            {
                textToken.CharacterSize = this.CharacterSize;
                textToken.SpriteColor = this.SpriteColor;
            }

            this.UpdateTextTokensToFitCanevas();
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                foreach(TextToken2D textToken2D in this.textToken2Ds)
                {
                    textToken2D.DrawIn(window, deltaTime);
                }
            }
        }

        private void UpdateTextTokensToFitCanevas()
        {
            FloatRect bounds = this.Bounds;

            Vector2f startCursor = new Vector2f(bounds.Left, bounds.Top);
            Vector2f maxCursor = new Vector2f(bounds.Left + bounds.Width, bounds.Top + bounds.Height);

            Vector2f currentCursor = startCursor;

            foreach (TextToken2D textToken2D in this.textToken2Ds)
            {
                IntRect tokenCanevas = textToken2D.Canevas;

                if(currentCursor.X + tokenCanevas.Width > maxCursor.X)
                {
                    currentCursor.X = startCursor.X;
                    currentCursor.Y += this.CharacterSize;

                    textToken2D.Position = currentCursor;
                }
            }
        }
    }
}
