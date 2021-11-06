using Astrategia.Animation;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Player;
using Astrategia.View.Animation;
using Astrategia.View.Layer2D.BoardGameLayer2D;
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
    public class DomainToolTip2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

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

        public override FloatRect Bounds
        {
            get
            {
                return this.bannerShape.GetGlobalBounds();
            }
        }

        public DomainToolTip2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(180, 110));
            //this.bannerShape.Origin = new Vector2f(this.bannerShape.Size.X / 2, this.bannerShape.Size.Y / 2);

            this.bannerShape.FillColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            IAnimation showAnimation = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(showAnimation);

            this.CreateTextParagraph2D(new Vector2f(0, 10), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 60), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 30);

            this.UpdateTextOfParagraph(0, "domain_header_tooltip");
            this.UpdateTextOfParagraph(1, "domain_content_tooltip");

            this.IsActive = false;
        }

        public void DisplayToolTip(CJStarDomain domain, Player player, Player opponent)
        {
            this.SpriteColor = new Color(0, 0, 0, 0);
            this.PlayAnimation(0);

            this.IsActive = true;

            if (domain.PlayerToPoints.TryGetValue(player, out int pointsPlayer))
            {
                this.textParagraph2Ds[1].UpdateParameterText(0, pointsPlayer.ToString());
            }
            else
            {
                this.textParagraph2Ds[1].UpdateParameterText(0, "0");
            }

            if (domain.PlayerToPoints.TryGetValue(opponent, out int pointsOpponent))
            {
                this.textParagraph2Ds[1].UpdateParameterText(1, pointsOpponent.ToString());
            }
            else
            {
                this.textParagraph2Ds[1].UpdateParameterText(1, "0");
            }

            this.textParagraph2Ds[1].UpdateParameterColor(0, player.PlayerColor);
            this.textParagraph2Ds[1].UpdateParameterColor(1, opponent.PlayerColor);

            this.LaunchTextOfParagraphScrolling(0, 100);
            this.LaunchTextOfParagraphScrolling(1, 100);
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
