using Astrategia.Animation;
using Astrategia.Model.Card;
using Astrategia.View.Animation;
using Astrategia.View.Card2D;
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
    public class CardToolTip2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        public CardEntity2D CardFocused
        {
            get;
            private set;
        }

        public override Vector2f Position
        {
            get
            {
                return this.bannerShape.Position;
            }
            set
            {
                this.bannerShape.Position = value * MainWindow.MODEL_TO_VIEW;

                value.X = (int)value.X;
                value.Y = (int)value.Y;

                base.Position = value;
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

                this.bannerShape.FillColor = new Color(value.R, value.G, value.B, (byte) (value.A / 2));
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

        public override FloatRect Bounds
        {
            get
            {
                return this.bannerShape.GetGlobalBounds();
            }
        }

        public CardToolTip2D(ALayer2D parentLayer) 
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(200, 300));
            //this.bannerShape.Origin = new Vector2f(this.bannerShape.Size.X / 2, this.bannerShape.Size.Y / 2);

            this.bannerShape.FillColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            this.CardFocused = null;

            IAnimation showAnimation = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(showAnimation);

            this.CreateTextParagraph2D(new Vector2f(0, 10), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 60), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 14);
            this.CreateTextParagraph2D(new Vector2f(0, 200), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 14);
            this.IsActive = false;
        }

        public void DisplayToolTip(Card card, CardEntity2D cardFocused)
        {
            this.CardFocused = cardFocused;

            this.SpriteColor = new Color(0, 0, 0, 0);
            this.PlayAnimation(0);

            this.IsActive = true;
            this.UpdateTextOfParagraph(0, card.NameIdLoc);
            this.UpdateTextOfParagraph(1, card.PoemIdLoc);
            this.UpdateTextOfParagraph(2, card.EffectIdLoc);

            this.LaunchTextOfParagraphScrolling(1, 100);
            this.LaunchTextOfParagraphScrolling(2, 100);
        }

        public void HideToolTip()
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
