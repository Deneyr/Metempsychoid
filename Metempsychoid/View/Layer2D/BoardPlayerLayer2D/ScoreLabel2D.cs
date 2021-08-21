using Metempsychoid.Animation;
using Metempsychoid.Model.Player;
using Metempsychoid.View.Animation;
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
    public class ScoreLabel2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        private int score;

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                if (this.score != value)
                {
                    this.score = value;

                    //this.CreateTextOfParagraph(2, this.score.ToString(), "BannerTitle", Color.White);

                    this.textParagraph2Ds[2].UpdateParameterText(0, this.score.ToString());

                    this.PlayAnimation(0);
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
                this.textParagraph2Ds[2].CustomZoom = value;
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                return this.bannerShape.GetGlobalBounds();
            }
        }

        public ScoreLabel2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.score = -1;
            this.bannerShape = new RectangleShape(new Vector2f(200, 170));

            this.SpriteColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            IAnimation labelChangedAnimation = new ZoomAnimation(2, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(labelChangedAnimation);

            this.CreateTextParagraph2D(new Vector2f(0, 10), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 110), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 30), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 70);

            this.UpdateTextOfParagraph(2, "field_title");

            this.IsActive = false;
        }

        public void DisplayScore(int indexPlayer, Player player)
        {
            if (indexPlayer == 0)
            {
                this.UpdateTextOfParagraph(0, "score_player_name", player.PlayerName);
            }
            else
            {
                this.UpdateTextOfParagraph(0, "score_opponent_name", player.PlayerName);
            }
            this.textParagraph2Ds[0].UpdateParameterColor(0, player.PlayerColor);

            if (indexPlayer == 0)
            {
                this.UpdateTextOfParagraph(1, "score_player_content");
            }
            else
            {
                this.UpdateTextOfParagraph(1, "score_opponent_content");
            }

            this.IsActive = true;
        }

        public void HideScore()
        {
            this.IsActive = false;
        }

        public void ResetScore()
        {
            this.score = 0;
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
