using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
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

                    this.CreateTextOfParagraph(2, this.score.ToString(), "BannerTitle", Color.White);
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

        public ScoreDomainPlayerLabel2D(ALayer2D parentLayer, int indexPlayer, string playerName)
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

            if (indexPlayer == 0)
            {
                this.UpdateTextOfParagraph(0, "score_domain_player_label", playerName);
            }
            else
            {
                this.UpdateTextOfParagraph(0, "score_domain_opponent_label", playerName);
            }

            if (indexPlayer == 0)
            {
                this.UpdateTextOfParagraph(1, "score_domain_player_content");
            }
            else
            {
                this.UpdateTextOfParagraph(1, "score_domain_opponent_content");
            }

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

        public void DisplayScore(int score)
        {
            this.IsActive = true;

            this.Score = score;
        }
    }
}
