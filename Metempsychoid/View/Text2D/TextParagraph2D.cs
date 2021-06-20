using Metempsychoid.View.Animation;
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
        private Vector2f positionOffsetTopLeft;
        private Vector2f positionOffsetBotRight;

        private float realRotation;
        private float rotationOffset;

        private Color spriteColor;

        private Vector2f customZoom;

        private bool isActive;

        private uint characterSize;

        private Alignment alignment;

        private IntRect realCanevas;

        private List<TextToken2D> textToken2Ds;
        private int tokenCursor;
        private float scrollingSpeed;

        delegate int TextLineHandler(ref Vector2f cursor, List<TextToken2D> tokensInLine, int offsetLine);

        public override Vector2f Position
        {
            get
            {
                return this.realPosition;
            }
            set
            {
                Vector2f newPosition = value + this.positionOffsetTopLeft;
                if(this.realPosition != newPosition)
                {
                    this.realPosition = newPosition;

                    this.AlignTextTokens();
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
                return this.CustomZoom.X;
            }
            set
            {
                this.CustomZoom = new Vector2f(value, value);
            }
        }

        public override Vector2f CustomZoom
        {
            get
            {
                return this.customZoom;
            }
            set
            {
                if (this.customZoom != value)
                {
                    this.customZoom = value;

                    foreach (TextToken2D textToken2D in this.textToken2Ds)
                    {
                        textToken2D.CustomZoom = this.customZoom;
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

                    this.AlignTextTokens();
                }
            }
        }

        public Alignment ParagraphAlignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                if(this.alignment != value)
                {
                    this.alignment = value;

                    this.AlignTextTokens();
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
                int top = (int) Math.Min(value.Top + value.Height, value.Top + this.positionOffsetTopLeft.Y);
                int left = (int) Math.Min(value.Left + value.Width, value.Left + this.positionOffsetTopLeft.X);

                int height = Math.Max(0, value.Top + value.Height - top);
                int width = Math.Max(0, value.Left + value.Width - (int)this.positionOffsetBotRight.X - left);

                IntRect newCanevas = new IntRect(left, top, width, height);

                if(this.realCanevas != newCanevas)
                {
                    this.realCanevas = newCanevas;

                    this.AlignTextTokens();
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
                }
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                IntRect canevas = this.Canevas;
                Vector2f position = this.Position;
                return new FloatRect(position.X, position.Y, canevas.Width, canevas.Height);
            }
        }

        public TextParagraph2D(TextCanevas2D textCanevas2D, Vector2f positionOffsetTopLeft, Vector2f positionOffsetBotRight, Alignment alignment, uint characterSize)
        {
            this.positionOffsetTopLeft = positionOffsetTopLeft;
            this.positionOffsetBotRight = positionOffsetBotRight;
            this.rotationOffset = 0;

            this.isActive = true;

            this.textToken2Ds = new List<TextToken2D>();
            tokenCursor = -1;

            this.Position = textCanevas2D.Position;
            this.Rotation = textCanevas2D.Rotation;

            this.Canevas = textCanevas2D.Canevas;

            this.alignment = alignment;

            this.spriteColor = Color.White;

            this.characterSize = characterSize;
            this.customZoom = new Vector2f(-1, -1);
            this.Zoom = textCanevas2D.Zoom;
        }

        public void UpdateTextTokens(List<TextToken2D> textTokens)
        {
            this.textToken2Ds = textTokens;

            foreach (TextToken2D textToken in this.textToken2Ds)
            {
                textToken.CharacterSize = this.CharacterSize;
                textToken.SpriteColor = this.SpriteColor;
                textToken.CustomZoom = this.CustomZoom;
            }

            this.AlignTextTokens();
        }

        public void LaunchAnimationScrolling(float speed)
        {
            this.scrollingSpeed = speed;

            foreach(TextToken2D textToken2D in this.textToken2Ds)
            {
                AObject2D.animationManager.StopAnimation(textToken2D);
                textToken2D.TextCursor = 0;
            }
            tokenCursor = 0;

            this.LaunchAnimationScrollingOnToken();
        }

        private void LaunchAnimationScrollingOnToken()
        {
            if (this.tokenCursor < this.textToken2Ds.Count)
            {
                TextToken2D textToken2D = this.textToken2Ds[this.tokenCursor];

                TextScrollingAnimation textScrollingAnimation = new TextScrollingAnimation(0, textToken2D.FullText.Length, Time.FromSeconds(textToken2D.FullText.Length * 1 / this.scrollingSpeed), Metempsychoid.Animation.AnimationType.ONETIME, Metempsychoid.Animation.InterpolationMethod.LINEAR);
                textToken2D.PlayAnimation(textScrollingAnimation);
            }
        }

        public void UpdateGraphics(Time deltaTime)
        {
            if(this.tokenCursor >= 0 && this.tokenCursor < this.textToken2Ds.Count)
            {
                if(this.textToken2Ds[this.tokenCursor].IsAnimationRunning() == false)
                {
                    this.tokenCursor++;

                    this.LaunchAnimationScrollingOnToken();
                }
            }
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

        private int AlignLeftLineTextTokens(ref Vector2f cursor, List<TextToken2D> tokensInLine, int offsetLine)
        {
            int maxHeight = 0;
            IntRect tokenCanevas = new IntRect();
            int i = 0;
            foreach (TextToken2D textToken2D in tokensInLine)
            {
                tokenCanevas = textToken2D.Canevas;

                textToken2D.Position = cursor;
                cursor.X += tokenCanevas.Width;

                if (tokenCanevas.Height > maxHeight)
                {
                    maxHeight = tokenCanevas.Height;
                }

                if (i < tokensInLine.Count - 1)
                {
                    cursor.X += AObject2DFactory.GetWidthFromTextToken(textToken2D);
                }

                i++;
            }

            return maxHeight;
        }

        private int AlignRightLineTextTokens(ref Vector2f cursor, List<TextToken2D> tokensInLine, int offsetLine)
        {
            cursor.X += offsetLine;
            int maxHeight = 0;
            IntRect tokenCanevas = new IntRect();
            int i = 0;
            foreach (TextToken2D textToken2D in tokensInLine)
            {
                tokenCanevas = textToken2D.Canevas;

                textToken2D.Position = cursor;
                cursor.X += tokenCanevas.Width;

                if (tokenCanevas.Height > maxHeight)
                {
                    maxHeight = tokenCanevas.Height;
                }

                if (i < tokensInLine.Count - 1)
                {
                    cursor.X += AObject2DFactory.GetWidthFromTextToken(textToken2D);
                }

                i++;
            }

            return maxHeight;
        }

        private int AlignCenterLineTextTokens(ref Vector2f cursor, List<TextToken2D> tokensInLine, int offsetLine)
        {
            cursor.X += offsetLine / 2;
            int maxHeight = 0;
            IntRect tokenCanevas = new IntRect();
            int i = 0;
            foreach (TextToken2D textToken2D in tokensInLine)
            {
                tokenCanevas = textToken2D.Canevas;

                textToken2D.Position = cursor;
                cursor.X += tokenCanevas.Width;

                if (tokenCanevas.Height > maxHeight)
                {
                    maxHeight = tokenCanevas.Height;
                }

                if (i < tokensInLine.Count - 1)
                {
                    cursor.X += AObject2DFactory.GetWidthFromTextToken(textToken2D);
                }

                i++;
            }

            return maxHeight;
        }

        private int AlignJustifyLineTextTokens(ref Vector2f cursor, List<TextToken2D> tokensInLine, int offsetLine)
        {
            if (tokensInLine.Count > 1)
            {
                offsetLine /= (tokensInLine.Count - 1);
            }

            int maxHeight = 0;
            IntRect tokenCanevas = new IntRect();
            int i = 0;
            foreach (TextToken2D textToken2D in tokensInLine)
            {
                tokenCanevas = textToken2D.Canevas;

                if(i != 0)
                {
                    cursor.X += offsetLine;
                }

                textToken2D.Position = cursor;
                cursor.X += tokenCanevas.Width;

                if (tokenCanevas.Height > maxHeight)
                {
                    maxHeight = tokenCanevas.Height;
                }

                if (i < tokensInLine.Count - 1)
                {
                    cursor.X += AObject2DFactory.GetWidthFromTextToken(textToken2D);
                }

                i++;
            }

            return maxHeight;
        }

        private void AlignTextTokens()
        {
            FloatRect bounds = this.Bounds;

            Vector2f startCursor = new Vector2f(bounds.Left, bounds.Top);
            Vector2f maxCursor = new Vector2f(bounds.Left + bounds.Width, bounds.Top + bounds.Height);
            Vector2f currentCursor = startCursor;

            int maxHeight = 0;

            List<TextToken2D> tokensInLine = new List<TextToken2D>();

            IEnumerator<TextToken2D> tokenEnumerator = this.textToken2Ds.GetEnumerator();
            int lineWidth = 0;
            int maxWidth = (int) (maxCursor.X - startCursor.X);

            bool notReachEnd = true;

            IntRect tokenCanevas = new IntRect();

            TextLineHandler LineHandler = this.AlignLeftLineTextTokens;
            switch (this.ParagraphAlignment)
            {
                case Alignment.LEFT:
                    LineHandler = this.AlignLeftLineTextTokens;
                    break;
                case Alignment.RIGHT:
                    LineHandler = this.AlignRightLineTextTokens;
                    break;
                case Alignment.CENTER:
                    LineHandler = this.AlignCenterLineTextTokens;
                    break;
                case Alignment.JUSTIFY:
                    LineHandler = this.AlignJustifyLineTextTokens;
                    break;
            }

            while (notReachEnd)
            {
                notReachEnd = tokenEnumerator.MoveNext();

                if (notReachEnd)
                {
                    tokenCanevas = tokenEnumerator.Current.Canevas;
                }

                if (notReachEnd == false
                    || lineWidth + tokenCanevas.Width > maxWidth 
                    || tokenEnumerator.Current.FullText == "\n")
                {
                    int diffWidth = maxWidth - lineWidth;
                    lineWidth = tokenCanevas.Width;

                    if (notReachEnd)
                    {
                        lineWidth += AObject2DFactory.GetWidthFromTextToken(tokenEnumerator.Current);
                    }

                    if (tokensInLine.Count > 0)
                    {
                        diffWidth += AObject2DFactory.GetWidthFromTextToken(tokensInLine.Last());

                        maxHeight = LineHandler(ref currentCursor, tokensInLine, diffWidth);
                    }
                    else
                    {
                        maxHeight = (int)this.characterSize;
                    }

                    currentCursor.X = startCursor.X;
                    currentCursor.Y += maxHeight;

                    tokensInLine.Clear();
                }
                else
                {
                    lineWidth += tokenCanevas.Width + AObject2DFactory.GetWidthFromTextToken(tokenEnumerator.Current);
                }

                if (notReachEnd
                    && tokenEnumerator.Current.FullText != "\n")
                {
                    tokensInLine.Add(tokenEnumerator.Current);
                }
            }
        }

        public enum Alignment
        {
            LEFT,
            RIGHT,
            CENTER,
            JUSTIFY
        }
    }
}
