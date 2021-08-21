using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
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

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class ScoreDomainPlayerLabel2D : TextCanevas2D
    {
        private int score;

        private float offsetX;
        private float offsetY;

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
                }
            }
        }

        public override Vector2f Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                IntRect canevas = this.Canevas;
                Vector2f realPosition = new Vector2f(value.X - canevas.Width / 2 + this.offsetX, value.Y - canevas.Height / 2 + this.offsetY);

                base.Position = realPosition;
            }
        }

        public ScoreDomainPlayerLabel2D(ALayer2D parentLayer, int indexPlayer, Player player)
            : base(parentLayer)
        {
            this.score = -1;

            this.SpriteColor = new Color(0, 0, 0, 255);

            this.Canevas = new IntRect(0, 0, 200, 200);

            if (indexPlayer == 0)
            {
                this.offsetX = -200;
            }
            else
            {
                this.offsetX = 200;
            }
            this.offsetY = 40;

            this.CreateTextParagraph2D(new Vector2f(0, 10), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 80), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 30), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 40);
            this.CreateTextParagraph2D(new Vector2f(0, 40), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 25);

            this.UpdateTextOfParagraph(2, "field_title");

            if (indexPlayer == 0)
            {
                this.UpdateTextOfParagraph(0, "score_domain_player_label", player.PlayerName);
            }
            else
            {
                this.UpdateTextOfParagraph(0, "score_domain_opponent_label", player.PlayerName);
            }
            this.textParagraph2Ds[0].UpdateParameterColor(0, player.PlayerColor);

            if (indexPlayer == 0)
            {
                this.UpdateTextOfParagraph(1, "score_domain_player_content");
            }
            else
            {
                this.UpdateTextOfParagraph(1, "score_domain_opponent_content");
            }

            this.UpdateTextOfParagraph(3, "score_domain_temporary_content");

            this.IsActive = false;

            IAnimation anim = new PositionAnimation(this.Position, new Vector2f(-this.offsetX, 0), Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(anim);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(1), AnimationType.ONETIME);
            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new PositionAnimation(this.Position, this.Position + new Vector2f(0, 200), Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0.001f, anim);
            this.animationsList.Add(sequence);
        }

        public void DisplayScore(int score, bool isTemporaryOwner)
        {
            this.IsActive = true;

            this.textParagraph2Ds[0].IsActive = true;
            if (isTemporaryOwner)
            {
                this.textParagraph2Ds[1].IsActive = false;
                this.textParagraph2Ds[2].IsActive = false;
                this.textParagraph2Ds[3].IsActive = true;
            }
            else
            {
                this.Score = score;

                this.textParagraph2Ds[1].IsActive = true;
                this.textParagraph2Ds[2].IsActive = true;
                this.textParagraph2Ds[3].IsActive = false;
            }
        }
    }
}
