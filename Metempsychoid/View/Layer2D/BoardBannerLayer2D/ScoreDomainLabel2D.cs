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

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class ScoreDomainLabel2D : TextCanevas2D
    {
        private Dictionary<string, ScoreDomainPlayerLabel2D> playerNameToScoreDomain2D;

        private ZoomVSState zoomState;

        private string ownerName;
        private bool isTemporaryOwner;

        public override bool IsActive
        {
            get
            {
                return base.IsActive && this.SpriteColor.A > 0;
            }
            set
            {
                base.IsActive = value;

                if (this.playerNameToScoreDomain2D != null)
                {
                    foreach (ScoreDomainPlayerLabel2D scoreDomainPlayerLabel2D in this.playerNameToScoreDomain2D.Values)
                    {
                        scoreDomainPlayerLabel2D.IsActive = value;
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
                this.textParagraph2Ds[1].CustomZoom = value;
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
                //Vector2f realPosition = new Vector2f(value.X - base.Size.X / 2, value.Y - this.bannerShape.Size.Y / 2);

                IntRect canevas = this.Canevas;
                Vector2f realPosition = new Vector2f(value.X - canevas.Width / 2, value.Y - canevas.Height / 2);

                base.Position = realPosition;

                foreach(ScoreDomainPlayerLabel2D scoreDomainPlayerLabel2D in this.playerNameToScoreDomain2D.Values)
                {
                    scoreDomainPlayerLabel2D.Position = value;
                }
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return base.SpriteColor;
            }
            set
            {
                base.SpriteColor = value;

                foreach (ScoreDomainPlayerLabel2D scoreDomainPlayerLabel2D in this.playerNameToScoreDomain2D.Values)
                {
                    scoreDomainPlayerLabel2D.SpriteColor = value;
                }
            }
        }

        public ScoreDomainLabel2D(ALayer2D parentLayer, Player player1, Player player2)
            : base(parentLayer)
        {
            this.ownerName = null;
            this.isTemporaryOwner = false;
            this.zoomState = ZoomVSState.STOP;

            this.Canevas = new IntRect(0, 0, 500, 200);

            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 50), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 60);
            this.UpdateTextOfParagraph(0, "score_domain_label");
            this.UpdateTextOfParagraph(1, "score_domain_vs");


            this.CreateTextParagraph2D(new Vector2f(0, 100), new Vector2f(0, 0), TextParagraph2D.Alignment.LEFT, 20);
            this.CreateTextParagraph2D(new Vector2f(0, 100), new Vector2f(0, 0), TextParagraph2D.Alignment.RIGHT, 20);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.ONETIME);

            IAnimation anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(2f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0.001f, anim);

            anim = new ZoomAnimation(2, 1, Time.FromSeconds(4f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);
            this.animationsList.Add(sequence);

            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(anim);

            this.InitializeScoreDomainLabel(parentLayer, player1, player2);
            this.Position = new Vector2f(0, 0);

            this.IsActive = false;
        }

        private void InitializeScoreDomainLabel(ALayer2D parentLayer, Player player1, Player player2)
        {
            this.playerNameToScoreDomain2D = new Dictionary<string, ScoreDomainPlayerLabel2D>();

            this.playerNameToScoreDomain2D.Add(player1.PlayerName, new ScoreDomainPlayerLabel2D(parentLayer, 0, player1));
            this.playerNameToScoreDomain2D.Add(player2.PlayerName, new ScoreDomainPlayerLabel2D(parentLayer, 1, player2));
        }

        public void UpdateScoreDomainLabel(string playerName, int score)
        {
            this.playerNameToScoreDomain2D[playerName].DisplayScore(score, this.isTemporaryOwner && playerName == this.ownerName);
        }

        public void DisplayScoreDomainLabel(string ownerName, bool isTemporaryOwner)
        {
            this.Zoom = 2;
            this.ownerName = ownerName;
            this.isTemporaryOwner = isTemporaryOwner;

            this.ActiveAllParagraphs();

            this.Position = new Vector2f(0, 0);
            this.SpriteColor = new Color(0, 0, 0, 0);

            foreach (KeyValuePair<string, ScoreDomainPlayerLabel2D> scoreDomainKeyValuePair in this.playerNameToScoreDomain2D)
            {
                scoreDomainKeyValuePair.Value.DisplayScore(0, this.isTemporaryOwner && scoreDomainKeyValuePair.Key == this.ownerName);
            }

            this.InitializeZoomVSRunning();

            this.IsActive = true;

            this.PlayAnimation(0);
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            switch (this.zoomState)
            {
                case ZoomVSState.RUNNING:
                    this.UpdateZoomVSStateRunning();
                    break;
                    case ZoomVSState.LABELS_RUNNING:

                    break;
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            if (this.IsActive)
            {
                foreach(ScoreDomainPlayerLabel2D scoreDomainPlayerLabel2D in this.playerNameToScoreDomain2D.Values)
                {
                    scoreDomainPlayerLabel2D.DrawIn(window, deltaTime);
                }
            }
        }

        public override void Dispose()
        {
            foreach (ScoreDomainPlayerLabel2D scoreDomainPlayerLabel2D in this.playerNameToScoreDomain2D.Values)
            {
                scoreDomainPlayerLabel2D.Dispose();
            }

            base.Dispose();
        }

        private void InitializeZoomVSRunning()
        {
            this.zoomState = ZoomVSState.RUNNING;
        }

        private void UpdateZoomVSStateRunning()
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializeZoomVSLabelRunning();
            }
        }

        private void InitializeZoomVSLabelRunning()
        {
            if (this.ownerName != null)
            {
                foreach (KeyValuePair<string, ScoreDomainPlayerLabel2D> keyValuePair in this.playerNameToScoreDomain2D)
                {
                    if (keyValuePair.Key == this.ownerName)
                    {
                        keyValuePair.Value.PlayAnimation(0);
                    }
                    else
                    {
                        keyValuePair.Value.PlayAnimation(1);
                    }
                }

                this.ActiveOnlyParagraph(0);

                this.zoomState = ZoomVSState.LABELS_RUNNING;
            }
            else
            {
                this.zoomState = ZoomVSState.STOP;
            }

            if (this.parentLayer.TryGetTarget(out ALayer2D layer2D))
            {
                (layer2D as BoardBannerLayer2D).OnDomainEvaluationFinished(this.ownerName);
            }
        }

        private void UpdateZoomVSLabelRunning()
        {
            bool labelsStillRunning = false;

            foreach (KeyValuePair<string, ScoreDomainPlayerLabel2D> keyValuePair in this.playerNameToScoreDomain2D)
            {
                if (keyValuePair.Value.IsAnimationRunning())
                {
                    labelsStillRunning = true;
                }
            }

            if(labelsStillRunning == false)
            {
                this.InitializeZoomVSStop();
            }
        }

        private void InitializeZoomVSStop()
        {
            this.zoomState = ZoomVSState.STOP;
        }

        private enum ZoomVSState
        {
            RUNNING,
            LABELS_RUNNING,
            STOP,
        }
    }
}
