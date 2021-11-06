using Astrategia.Animation;
using Astrategia.Model.Player;
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
    public class EndLevelBanner2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        private List<List<TextParagraph2D>> paragraphRows;
        private int currentRowIndex;

        private bool isDraw;

        public BannerState State
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
                switch (this.State)
                {
                    case BannerState.MAIN:
                        List<TextParagraph2D> paragraph2DRow = this.paragraphRows[this.currentRowIndex];

                        foreach(TextParagraph2D row in paragraph2DRow)
                        {
                            row.SpriteColor = value;
                        }
                        break;
                    default:
                        base.SpriteColor = value;
                        break;
                }
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
                switch (this.State)
                {
                    case BannerState.WINNER:
                        this.paragraphRows.Last()[0].CustomZoom = value;

                        break;
                    default:
                        this.bannerShape.Scale = new Vector2f(1, value.Y);
                        break;
                }
            }
        }

        //public override float Zoom
        //{
        //    get
        //    {
        //        return base.Zoom;
        //    }

        //    set
        //    {
        //        base.Zoom = value;
        //    }
        //}

        public override FloatRect Bounds
        {
            get
            {
                return this.bannerShape.GetGlobalBounds();
            }
        }

        public EndLevelBanner2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.paragraphRows = new List<List<TextParagraph2D>>();

            this.bannerShape = new RectangleShape(new Vector2f(1000, 1000));
            this.bannerShape.FillColor = new Color(0, 0, 0, 200);

            this.Position = new Vector2f(0, 0);

            this.isDraw = false;

            IAnimation anim = new ZoomAnimation(0, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(anim);

            anim = new ZoomAnimation(1, 2, Time.FromSeconds(3), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(anim);

            this.State = BannerState.START;

            this.IsActive = false;
        }

        public void DisplayEndLevelBanner(Dictionary<string, List<int>> scores, Player player, Player opponent)
        {
            this.State = BannerState.START;

            this.SpriteColor = new Color(0, 0, 0, 255);

            this.paragraphRows.Clear();
            this.textParagraph2Ds.Clear();

            TextParagraph2D textParagraph2D;

            textParagraph2D = this.CreateTextParagraph2D(new Vector2f(0, 50), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 30);
            this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });
            this.UpdateTextOfParagraph(0, "end_game_header");

            textParagraph2D = this.CreateTextParagraph2D(new Vector2f(0, 100), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });
            this.UpdateTextOfParagraph(1, "end_game_header2");

            textParagraph2D = this.CreateTextParagraph2D(new Vector2f(20, 150), new Vector2f(300, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });
            this.UpdateTextOfParagraph(2, "end_game_player", player.PlayerName);
            this.textParagraph2Ds[2].UpdateParameterColor(0, player.PlayerColor);
            textParagraph2D = this.CreateTextParagraph2D(new Vector2f(300, 150), new Vector2f(20, 0), Text2D.TextParagraph2D.Alignment.CENTER, 20);
            this.paragraphRows.Last().Add(textParagraph2D);
            this.UpdateTextOfParagraph(3, "end_game_player", opponent.PlayerName);
            this.textParagraph2Ds[3].UpdateParameterColor(0, opponent.PlayerColor);

            List<int> scorePlayer = scores[player.PlayerName];
            List<int> scoreOpponent = scores[opponent.PlayerName];

            int offsetY = 150;
            for (int i = 0; i < scorePlayer.Count; i++)
            {
                offsetY += 50;

                int scorePlayerRow = scorePlayer[i];
                int scoreOpponentRow = scoreOpponent[i];

                uint policeSize = 20;
                if(i == scorePlayer.Count - 1)
                {
                    policeSize = 30;
                }

                textParagraph2D = this.CreateTextParagraph2D(new Vector2f(20, offsetY), new Vector2f(800, 0), Text2D.TextParagraph2D.Alignment.CENTER, policeSize);
                this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "end_game_turns", (i + 1).ToString());

                textParagraph2D = this.CreateTextParagraph2D(new Vector2f(20, offsetY), new Vector2f(300, 0), Text2D.TextParagraph2D.Alignment.CENTER, policeSize);
                this.paragraphRows.Last().Add(textParagraph2D);
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "end_game_points", scorePlayerRow.ToString());
                textParagraph2D = this.CreateTextParagraph2D(new Vector2f(300, offsetY), new Vector2f(20, 0), Text2D.TextParagraph2D.Alignment.CENTER, policeSize);
                this.paragraphRows.Last().Add(textParagraph2D);
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "end_game_points", scoreOpponentRow.ToString());

                if(scorePlayerRow > scoreOpponentRow)
                {
                    this.paragraphRows.Last()[1].UpdateParameterColor(0, Color.Green);
                }
                else if(scorePlayerRow < scoreOpponentRow)
                {
                    this.paragraphRows.Last()[2].UpdateParameterColor(0, Color.Green);
                }
            }

            offsetY += 80;

            textParagraph2D = this.CreateTextParagraph2D(new Vector2f(0, offsetY), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 50);
            this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });

            this.isDraw = false;
            if (scorePlayer.Last() > scoreOpponent.Last())
            {
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "end_game_winner");

                textParagraph2D = this.CreateTextParagraph2D(new Vector2f(0, offsetY + 80), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 50);
                this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "field_title", player.PlayerName);

                this.paragraphRows.Last()[0].UpdateParameterColor(0, player.PlayerColor);
            }
            else if(scorePlayer.Last() < scoreOpponent.Last())
            {
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "end_game_winner");

                textParagraph2D = this.CreateTextParagraph2D(new Vector2f(0, offsetY + 80), new Vector2f(0, 0), Text2D.TextParagraph2D.Alignment.CENTER, 50);
                this.paragraphRows.Add(new List<TextParagraph2D>() { textParagraph2D });
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "field_title", opponent.PlayerName);

                this.paragraphRows.Last()[0].UpdateParameterColor(0, opponent.PlayerColor);
            }
            else
            {
                this.UpdateTextOfParagraph(this.textParagraph2Ds.Count - 1, "end_game_draw");
                this.isDraw = true;
            }

            this.InitializeOpeningState();
        }

        public void HideEndLevelBanner()
        {
            this.textParagraph2Ds.Clear();

            this.IsActive = false;

            this.State = BannerState.START;
        }

        private void InitializeOpeningState()
        {
            this.Zoom = 0;
            this.SpriteColor = new Color(0, 0, 0, 0);

            this.PlayAnimation(0);

            this.IsActive = true;

            this.State = BannerState.OPENING;
        }

        private void InitializeMainState()
        {
            this.currentRowIndex = 0;

            this.PlayAnimation(1);

            this.State = BannerState.MAIN;
        }

        private void InitializeWinnerState()
        {
            if (this.isDraw == false)
            {
                this.PlayAnimation(2);
            }

            this.State = BannerState.WINNER;
        }

        private void InitializeEndState()
        {
            this.State = BannerState.END;
        }

        private void UpdateOpeningState()
        {
            if(this.IsAnimationRunning() == false)
            {
                this.InitializeMainState();
            }
        }

        private void UpdateMainState()
        {
            if (this.IsAnimationRunning() == false)
            {
                this.currentRowIndex++;

                if(this.currentRowIndex < this.paragraphRows.Count)
                {
                    this.PlayAnimation(1);
                }
                else
                {
                    this.InitializeWinnerState();
                }
            }
        }

        private void UpdateWinnerState()
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializeEndState();
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.State)
            {
                case BannerState.OPENING:
                    this.UpdateOpeningState();
                    break;
                case BannerState.MAIN:
                    this.UpdateMainState();
                    break;
                case BannerState.WINNER:
                    this.UpdateWinnerState();
                    break;
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.bannerShape);
            }

            base.DrawIn(window, deltaTime);
        }

        public enum BannerState
        {
            START,
            OPENING,
            MAIN,
            WINNER,
            END
        }
    }
}
