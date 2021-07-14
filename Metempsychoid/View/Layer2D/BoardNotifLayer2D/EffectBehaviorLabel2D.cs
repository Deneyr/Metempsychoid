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

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class EffectBehaviorLabel2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        private bool isActive;

        private int label;

        private Vector2f offset;
        private Vector2f startingPosition;

        private Dictionary<Type, string> idLabelToIndex;

        public override bool IsActive
        {
            get
            {
                return this.isActive || this.IsAnimationRunning();
            }
            set
            {
                this.isActive = value;
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
                this.offset = value;

                this.UpdatePosition();
            }
        }

        public Vector2f StartingPosition
        {
            get
            {
                return this.startingPosition;
            }
            set
            {
                this.startingPosition = value;

                this.UpdatePosition();
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

        public int Label
        {
            get
            {
                return this.label;
            }
            set
            {
                if (this.label != value)
                {
                    this.label = value;

                    this.CreateTextOfParagraph(2, this.label.ToString(), "BannerTitle", Color.Green);

                    if (this.IsAnimationRunning() == false)
                    {
                        this.PlayAnimation(2);
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

        public EffectBehaviorLabel2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.startingPosition = new Vector2f(0, 0);
            this.offset = new Vector2f(0, 0);

            this.bannerShape = new RectangleShape(new Vector2f(500, 150));
            this.SpriteColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            this.idLabelToIndex = new Dictionary<Type, string>();
            this.CreateTextParagraph2D(new Vector2f(0, 35), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);

            this.CreateTextParagraph2D(new Vector2f(25, 85), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(1, "behavior_using_label");

            this.CreateTextParagraph2D(new Vector2f(-370, 75), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 30);

            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.MoveCardNotifBehavior), "move_behavior_label");
            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.SwapCardNotifBehavior), "swap_behavior_label");
            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.DeleteCardNotifBehavior), "delete_behavior_label");
            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.ResurrectCardNotifBehavior), "resurrect_behavior_label");
            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.SocketCardNotifBehavior), "play_behavior_label");
            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.SocketNewCardNotifBehavior), "play_new_behavior_label");
            this.idLabelToIndex.Add(typeof(Model.Layer.BoardNotifLayer.Behavior.ConvertCardNotifBehavior), "convert_behavior_label");

            IAnimation showAnimation = new PositionAnimation(new Vector2f(0, 0), new Vector2f(this.Canevas.Width, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(showAnimation);
            //IAnimation showAnimation = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            //this.animationsList.Add(showAnimation);

            //SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(2), AnimationType.LOOP);
            //IAnimation focusedAnimation = new ColorAnimation(new Color(0, 0, 0, 255), new Color(125, 125, 125, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(0, focusedAnimation);

            //focusedAnimation = new ColorAnimation(new Color(125, 125, 125, 255), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(1, focusedAnimation);
            //this.animationsList.Add(sequence);

            //IAnimation hideAnimation = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            //this.animationsList.Add(hideAnimation);

            IAnimation hideAnimation = new PositionAnimation(new Vector2f(this.Canevas.Width, 0), new Vector2f(0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(hideAnimation);

            IAnimation labelChangedAnimation = new ZoomAnimation(2, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(labelChangedAnimation);

            this.IsActive = false;
        }

        private void UpdatePosition()
        {
            Vector2f newPosition = this.startingPosition + this.offset;

            base.Position = newPosition;

            this.bannerShape.Position = newPosition * MainWindow.MODEL_TO_VIEW;
        }

        public void ActiveLabel(Type behaviorType)
        {
            if (this.idLabelToIndex.ContainsKey(behaviorType))
            {
                this.UpdateTextOfParagraph(0, this.idLabelToIndex[behaviorType]);

                //this.SpriteColor = new Color(0, 0, 0, 0);

                this.IsActive = true;

                this.PlayAnimation(0);
            }
        }

        public void DeactiveLabel()
        {
            this.IsActive = false;

            this.PlayAnimation(1);
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
