using Astrategia.Animation;
using Astrategia.View.Animation;
using Astrategia.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    class CardsToPlaceBanner2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        private int cardsToPlaceCount;

        public int CardsToPlaceCount
        {
            get
            {
                return this.cardsToPlaceCount;
            }
            private set
            {
                if (this.cardsToPlaceCount != value)
                {
                    this.cardsToPlaceCount = value;

                    this.textParagraph2Ds[0].UpdateParameterText(0, this.cardsToPlaceCount.ToString());
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

        public CardsToPlaceBanner2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(400, 50));

            this.SpriteColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            IAnimation labelChangedAnimation = new ZoomAnimation(2, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(labelChangedAnimation);

            this.CreateTextParagraph2D(new Vector2f(0, 10), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(0, "turn_banner_socket_cards");

            this.cardsToPlaceCount = -1;

            this.IsActive = true;
        }

        public void UpdateCardsToPlaceCount(int cardsToPlace)
        {
            this.CardsToPlaceCount = cardsToPlace;

            this.PlayAnimation(0);
        }

        public void ResetTurn()
        {
            this.CardsToPlaceCount = 0;

            this.IsActive = true;
        }

        public void HideTurn()
        {
            this.IsActive = false;
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
