using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class TurnBanner2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        private int currentTurnCount;

        private int maxTurnCount;

        public int CurrentTurnCount
        {
            get
            {
                return this.currentTurnCount;
            }
            private set
            {
                if(this.currentTurnCount != value)
                {
                    this.currentTurnCount = value;

                    this.textParagraph2Ds[0].UpdateParameterText(0, this.currentTurnCount.ToString());
                }
            }
        }

        public int MaxTurnCount
        {
            get
            {
                return this.maxTurnCount;
            }
            private set
            {
                if (this.maxTurnCount != value)
                {
                    this.maxTurnCount = value;

                    this.textParagraph2Ds[0].UpdateParameterText(1, this.maxTurnCount.ToString());
                }
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.bannerShape.Position;
            }
            set
            {
                Vector2f realPosition = new Vector2f(value.X - this.bannerShape.Size.X / 2, value.Y - this.bannerShape.Size.Y / 2);

                base.Position = realPosition;

                this.bannerShape.Position = realPosition * MainWindow.MODEL_TO_VIEW;
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return this.bannerShape.FillColor;
            }
            set
            {
                base.SpriteColor = value;

                this.bannerShape.FillColor = new Color(value.R, value.G, value.B, (byte)(value.A / 2));
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return new IntRect(0, 0, (int)this.bannerShape.Size.X, (int)this.bannerShape.Size.Y);
            }

            set
            {
                Vector2f newSize = new Vector2f(value.Width, value.Height);

                if (this.bannerShape.Size != newSize)
                {
                    this.bannerShape.Size = newSize;

                    IntRect newCanevas = this.Canevas;
                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.Canevas = newCanevas;
                    }
                }
            }
        }

        public override Vector2f CustomZoom
        {
            get
            {
                return base.CustomZoom;
            }

            set
            {
                this.textParagraph2Ds[0].CustomZoom = value;
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                return this.bannerShape.GetGlobalBounds();
            }
        }

        public TurnBanner2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(200, 50));

            this.SpriteColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            IAnimation labelChangedAnimation = new ZoomAnimation(2, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(labelChangedAnimation);

            this.CreateTextParagraph2D(new Vector2f(0, 10), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);

            this.UpdateTextOfParagraph(0, "turn_banner");

            this.currentTurnCount = -1;
            this.maxTurnCount = -1;

            this.IsActive = true;
        }

        public void UpdateTurnCount(int currentTurnCount, int maxTurnCount)
        {
            this.CurrentTurnCount = currentTurnCount;

            this.MaxTurnCount = maxTurnCount;

            this.PlayAnimation(0);
        }

        public void HideTurn()
        {

            this.IsActive = false;
        }

        public void ResetTurn(int maxTurnCount)
        {
            this.CurrentTurnCount = 1;

            this.MaxTurnCount = maxTurnCount;

            this.IsActive = true;
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.bannerShape);
            }

            base.DrawIn(window, deltaTime);
        }
    }
}
