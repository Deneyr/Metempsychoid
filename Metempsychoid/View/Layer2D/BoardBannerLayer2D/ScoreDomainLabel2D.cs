﻿using Metempsychoid.Animation;
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

        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }

            set
            {
                this.textParagraph2Ds[1].Zoom = value;
            }
        }
        //private int score;

        //public int Score
        //{
        //    get
        //    {
        //        return this.score;
        //    }
        //    set
        //    {
        //        if (this.score != value)
        //        {
        //            this.score = value;

        //            this.CreateTextOfParagraph(2, this.score.ToString(), "BannerTitle");

        //            this.PlayAnimation(0);
        //        }
        //    }
        //}

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

        //public override IntRect Canevas
        //{
        //    get
        //    {
        //        return new IntRect(0, 0, (int)this.bannerShape.Size.X, (int)this.bannerShape.Size.Y);
        //    }

        //    set
        //    {
        //        Vector2f newSize = new Vector2f(value.Width, value.Height);

        //        if (this.bannerShape.Size != newSize)
        //        {
        //            this.bannerShape.Size = newSize;

        //            IntRect newCanevas = this.Canevas;
        //            foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
        //            {
        //                textParagraph2D.Canevas = newCanevas;
        //            }
        //        }
        //    }
        //}

        //public override float Zoom
        //{
        //    get
        //    {
        //        return base.Zoom;
        //    }

        //    set
        //    {
        //        this.textParagraph2Ds[2].Zoom = value;
        //    }
        //}

        //public override FloatRect Bounds
        //{
        //    get
        //    {
        //        return this.bannerShape.GetGlobalBounds();
        //    }
        //}

        public ScoreDomainLabel2D(ALayer2D parentLayer, string playerName1, string playerName2)
            : base(parentLayer)
        {
            this.ownerName = null;
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

            this.InitializeScoreDomainLabel(parentLayer, playerName1, playerName2);
            this.Position = new Vector2f(0, 0);

            this.IsActive = false;
        }

        private void InitializeScoreDomainLabel(ALayer2D parentLayer, string playerName1, string playerName2)
        {
            this.playerNameToScoreDomain2D = new Dictionary<string, ScoreDomainPlayerLabel2D>();

            this.playerNameToScoreDomain2D.Add(playerName1, new ScoreDomainPlayerLabel2D(parentLayer, 0, playerName1));
            this.playerNameToScoreDomain2D.Add(playerName2, new ScoreDomainPlayerLabel2D(parentLayer, 1, playerName2));
        }

        public void UpdateScoreDomainLabel(string playerName, int score)
        {
            this.playerNameToScoreDomain2D[playerName].DisplayScore(score);
        }

        public void DisplayScoreDomainLabel(string ownerName)
        {
            this.Zoom = 2;
            this.ownerName = ownerName;

            this.ActiveAllParagraphs();

            this.Position = new Vector2f(0, 0);
            this.SpriteColor = new Color(0, 0, 0, 0);

            foreach (ScoreDomainPlayerLabel2D scoreDomainPlayerLabel2D in this.playerNameToScoreDomain2D.Values)
            {
                scoreDomainPlayerLabel2D.DisplayScore(0);
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
