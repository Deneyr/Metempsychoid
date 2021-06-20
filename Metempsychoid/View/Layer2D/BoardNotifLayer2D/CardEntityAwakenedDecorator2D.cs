using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Card2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class CardEntityAwakenedDecorator2D : CardEntityDecorator2D
    {
        private int cardValue;

        private int cardValueModifier;

        private Vector2f initialPosition;

        private StarEffect2D starEffect2D;
        private BeamsEffect2D beamsEffect2D;

        public CardDecoratorState DecoratorState
        {
            get;
            private set;
        }

        public override Color SpriteColor
        {
            set
            {
                if (this.SpriteColor != value)
                {
                    this.PlayerColor = new Color(this.PlayerColor.R, this.PlayerColor.G, this.PlayerColor.B, value.A);

                    base.SpriteColor = value;
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
                base.Position = value;

                if (this.starEffect2D != null)
                {
                    this.starEffect2D.Position = this.Position;
                }

                if (this.beamsEffect2D != null)
                {
                    this.beamsEffect2D.Position = this.Position;
                }
            }
        }

        public override Color PlayerColor
        {
            protected set
            {
                if (this.playerColor != value)
                {
                    this.playerColor = value;

                    render.Shader.SetUniform("playerColor", new SFML.Graphics.Glsl.Vec4(this.playerColor.R / 255f, this.playerColor.G / 255f, this.playerColor.B / 255f, this.playerColor.A / 255f));

                    this.cardHalo.SpriteColor = this.playerColor;
                }
            }
        }

        public override int CardValue
        {
            get
            {
                return this.cardValue;
            }

            set
            {
                this.cardValue = value;
            }
        }

        public override int CardValueModifier
        {
            get
            {
                return this.cardValueModifier;
            }

            set
            {
                this.cardValueModifier = value;
            }
        }

        //protected override void ShowCardLabel()
        //{
        //    this.cardLabel.ShowInstantLabel();
        //}

        public CardEntityAwakenedDecorator2D(IObject2DFactory factory, ALayer2D layer2D, CardEntityAwakenedDecorator entity) 
            : base(factory, layer2D, entity)
        {
            //SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.ONETIME);

            this.starEffect2D = new StarEffect2D(factory, layer2D, this);
            this.beamsEffect2D = new BeamsEffect2D(factory, layer2D, this);

            this.initialPosition = entity.CardDecoratedPosition;

            this.cardValue = entity.Card.Value;
            this.cardValueModifier = entity.Card.ValueModifier;

            //IAnimation animation = new ColorAnimation(new Color(255, 255, 255, 0), new Color(255, 255, 255, 255), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            ////sequence.AddAnimation(0, animation);
            //this.animationsList.Add(animation);

            //animation = new ColorAnimation(new Color(255, 255, 255, 255), new Color(this.PlayerColor.R, this.PlayerColor.G, this.PlayerColor.B, 0), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            ////sequence.AddAnimation(3, animation);
            ////flipAnimation.AddAnimation(0, animation);
            //this.animationsList.Add(animation);

            this.DecoratorState = CardDecoratorState.FINISHED;
        }

        public void DisplayAwakened()
        {
            this.InitializeStartState();
        }

        public void HideAwakened()
        {
            this.InitializeEndState();
        }

        public override bool IsAnimationRunning()
        {
            return base.IsAnimationRunning() || this.starEffect2D.IsAnimationRunning() || this.beamsEffect2D.IsAnimationRunning();
        }


        private void InitializeStartState()
        {
            this.DecoratorState = CardDecoratorState.START;

            if (this.parentLayer.TryGetTarget(out ALayer2D layer2D))
            {
                Vector2f fromPosition = (layer2D as BoardNotifLayer2D).GetPositionFrom((layer2D.GetEntityFromEntity2D(this) as CardEntityAwakenedDecorator).CardEntityDecorated.ParentLayer, this.initialPosition);

                SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(1), AnimationType.ONETIME);

                IAnimation animation = new ColorAnimation(new Color(255, 255, 255, 0), new Color(255, 255, 255, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
                sequence.AddAnimation(0.01f, animation);

                animation = new PositionAnimation(fromPosition, new Vector2f(0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
                sequence.AddAnimation(0, animation);

                this.PlayAnimation(sequence);
            }
        }

        private void UpdateStartState()
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializeMiddleState();
            }
        }

        private void InitializeMiddleState()
        {
            this.DecoratorState = CardDecoratorState.MIDDLE;

            this.IsAwakened = true;

            base.CardValue = this.cardValue;
            base.CardValueModifier = this.cardValueModifier;

            this.starEffect2D.DisplayStarEffect();
            this.beamsEffect2D.DisplayBeamsEffect();
        }

        private void InitializeEndState()
        {
            this.DecoratorState = CardDecoratorState.END;

            this.cardLabel.HideLabel();

            this.starEffect2D.HideStarEffect();
            this.beamsEffect2D.HideBeamsEffect();

            if (this.parentLayer.TryGetTarget(out ALayer2D layer2D))
            {
                Vector2f fromPosition = (layer2D as BoardNotifLayer2D).GetPositionFrom((layer2D.GetEntityFromEntity2D(this) as CardEntityAwakenedDecorator).CardEntityDecorated.ParentLayer, this.initialPosition);

                SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(1), AnimationType.ONETIME);

                IAnimation animation = new ColorAnimation(new Color(255, 255, 255, 255), new Color(255, 255, 255, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
                sequence.AddAnimation(0.01f, animation);

                animation = new PositionAnimation(new Vector2f(0, 0), fromPosition, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
                sequence.AddAnimation(0, animation);

                this.PlayAnimation(sequence);
            }
        }

        private void UpdateMiddleState()
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializePendingState();
            }
        }

        private void UpdateEndState()
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializeFinishedState();
            }
        }

        private void InitializeFinishedState()
        {
            this.IsActive = false;

            this.DecoratorState = CardDecoratorState.FINISHED;
        }

        private void InitializePendingState()
        {
            this.starEffect2D.ContinueStarEffect();
            this.beamsEffect2D.ContinueBeamsEffect();

            this.DecoratorState = CardDecoratorState.PENDING;
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            switch (this.DecoratorState)
            {
                case CardDecoratorState.START:
                    this.UpdateStartState();
                    break;
                case CardDecoratorState.MIDDLE:
                    this.UpdateMiddleState();
                    break;
                case CardDecoratorState.PENDING:

                    break;
                case CardDecoratorState.END:
                    this.UpdateEndState();
                    break;
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                this.beamsEffect2D.DrawIn(window, deltaTime);
                this.starEffect2D.DrawIn(window, deltaTime);
            }

            base.DrawIn(window, deltaTime);
        }

        public override void Dispose()
        {
            this.starEffect2D.Dispose();
            this.beamsEffect2D.Dispose();

            base.Dispose();
        }

        public enum CardDecoratorState
        {
            START,
            MIDDLE,
            PENDING,
            END,
            FINISHED
        }
    }
}
