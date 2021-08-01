using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Controls;
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
    public class EndTurnButton2D : AButton2D
    {
        private RectangleShape bannerShape;

        private bool isActive;

        private Dictionary<string, int> idLabelToIndex;

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

        public EndTurnButton2D(ALayer2D parentLayer) 
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(200, 75));
            this.SpriteColor = new Color(0, 0, 0, 255);

            this.Position = new Vector2f(0, 0);

            this.idLabelToIndex = new Dictionary<string, int>();
            this.CreateTextParagraph2D(new Vector2f(0, 25), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(0, "end_turn");

            this.CreateTextParagraph2D(new Vector2f(0, 25), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(1, "pass_action");

            this.CreateTextParagraph2D(new Vector2f(0, 25), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(2, "destroy_action");

            this.CreateTextParagraph2D(new Vector2f(0, 25), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(3, "convert_action");

            this.idLabelToIndex.Add("DeleteCardNotifBehavior." + Enum.GetName(typeof(Model.Layer.BoardNotifLayer.Behavior.DeleteCardNotifBehavior.DeleteState), Model.Layer.BoardNotifLayer.Behavior.DeleteCardNotifBehavior.DeleteState.PICK_CARD), 1);
            this.idLabelToIndex.Add("DeleteCardNotifBehavior." + Enum.GetName(typeof(Model.Layer.BoardNotifLayer.Behavior.DeleteCardNotifBehavior.DeleteState), Model.Layer.BoardNotifLayer.Behavior.DeleteCardNotifBehavior.DeleteState.CAN_DELETE_CARD), 2);
            this.idLabelToIndex.Add("ConvertCardNotifBehavior." + Enum.GetName(typeof(Model.Layer.BoardNotifLayer.Behavior.ConvertCardNotifBehavior.DeleteState), Model.Layer.BoardNotifLayer.Behavior.ConvertCardNotifBehavior.DeleteState.PICK_CARD), 1);
            this.idLabelToIndex.Add("ConvertCardNotifBehavior." + Enum.GetName(typeof(Model.Layer.BoardNotifLayer.Behavior.ConvertCardNotifBehavior.DeleteState), Model.Layer.BoardNotifLayer.Behavior.ConvertCardNotifBehavior.DeleteState.CAN_DELETE_CARD), 3);

            IAnimation showAnimation = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(showAnimation);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(2), AnimationType.LOOP);
            IAnimation focusedAnimation = new ColorAnimation(new Color(0, 0, 0, 255), new Color(125, 125, 125, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0,  focusedAnimation);

            focusedAnimation = new ColorAnimation(new Color(125, 125, 125, 255), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(1, focusedAnimation);
            this.animationsList.Add(sequence);

            IAnimation hideAnimation = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(hideAnimation);

            IAnimation labelChangedAnimation = new ZoomAnimation(2, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(labelChangedAnimation);

            this.IsActive = false;
        }

        public override bool IsFocusable(ALayer2D parentLayer)
        {
            return this.isActive;
        }

        public void ActiveButton(int paragraphIndex)
        {
            this.SpriteColor = new Color(0, 0, 0, 0);

            this.IsActive = true;

            this.ActiveOnlyParagraph(paragraphIndex);

            this.PlayAnimation(0);
        }

        public void SetParagraph(string paragraphId)
        {
            if (this.idLabelToIndex.ContainsKey(paragraphId))
            {
                this.ActiveOnlyParagraph(this.idLabelToIndex[paragraphId]);

                if (this.IsAnimationRunning() == false)
                {
                    this.PlayAnimation(3);
                }
            }
        }

        public void SetParagraph(int paragraphIndex)
        {
            this.ActiveOnlyParagraph(paragraphIndex);

            if (this.IsAnimationRunning() == false)
            {
                this.PlayAnimation(3);
            }
        }

        public void DeactiveButton()
        {
            this.IsActive = false;

            this.PlayAnimation(2);
        }

        public override void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            base.OnMouseFocused(parentLayer, eventType);

            this.Zoom = 1;

            this.PlayAnimation(1);
        }

        public override bool OnMousePressed(ALayer2D parentLayer, ControlEventType eventType)
        {
            this.Zoom = 1.1f;
            return false;
        }

        //public override bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType)
        //{
        //    return false;
        //}

        public override bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (parentLayer.FocusedGraphicEntity2D == this)
            {
                base.OnMouseReleased(parentLayer, eventType);

                BoardNotifLayer2D boardPlayerLayer2D = (parentLayer as BoardNotifLayer2D);

                if (boardPlayerLayer2D.IsRunningBehavior)
                {
                    boardPlayerLayer2D.SendEventToWorld(Model.Event.EventType.NEXT_BEHAVIOR, null, null);
                }
                else
                {
                    boardPlayerLayer2D.GoOnTurnPhase(Model.Node.TestWorld.TurnPhase.END_TURN);
                }
            }

            this.Zoom = 1;

            return false;
        }

        public override void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (this.isActive)
            {
                AObject2D.animationManager.StopAnimation(this);

                this.SpriteColor = new Color(0, 0, 0, 255);
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
    }
}
