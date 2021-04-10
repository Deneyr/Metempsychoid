using Metempsychoid.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class CardToolTip : TextCanevas2D
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
                base.Position = value;

                this.bannerShape.Position = value * MainWindow.MODEL_TO_VIEW;
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

                this.bannerShape.FillColor = value;
                this.bannerShape.FillColor = new Color(this.bannerShape.FillColor.R, this.bannerShape.FillColor.G, this.bannerShape.FillColor.B, (byte) (this.bannerShape.FillColor.A / 2));
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

        public CardToolTip(ALayer2D parentLayer) 
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(200, 400));
            //this.bannerShape.Origin = new Vector2f(this.bannerShape.Size.X / 2, this.bannerShape.Size.Y / 2);

            this.bannerShape.FillColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            this.CardFocused = null;

            IAnimation showAnimation = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(showAnimation);
        }

        public void DisplayToolTip(Card card, CardEntity2D cardFocused)
        {
            this.CardFocused = cardFocused;

            this.PlayAnimation(0);

            this.IsActive = true;
            this.UpdateTextOfParagraph(0, card.NameIdLoc);

            this.UpdateTextOfParagraph(1, card.PoemIdLoc);
            this.LaunchTextOfParagraphScrolling(1, 20);

            this.UpdatePosition();
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

        public void UpdatePosition()
        {
            if(this.CardFocused != null)
            {
                Vector2f cardFocusedPosition = this.CardFocused.Position;

                cardFocusedPosition.X -= this.Canevas.Width / 2;

                cardFocusedPosition.Y -= this.CardFocused.Canevas.Height /2 + this.Canevas.Height;

                this.Position = cardFocusedPosition;
            }
        }
    }
}
