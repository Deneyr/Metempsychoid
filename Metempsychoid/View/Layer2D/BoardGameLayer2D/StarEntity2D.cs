using Metempsychoid.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Controls;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class StarEntity2D : AEntity2D, IHitRect
    {
        internal static Color DEFAULT_COLOR = Color.Blue;

        RenderStates render;

        protected bool isFocused;

        protected bool isActive;

        public StarState StarEntityState
        {
            get;
            private set;
        }

        public bool IsFocused
        {
            get
            {
                return this.isFocused;
            }
            set
            {
                if (this.isFocused != value)
                {
                    this.isFocused = value;

                    render.Shader.SetUniform("isFocused", this.isFocused);
                }
            }
        }

        public override bool IsActive
        {
            get
            {
                return this.StarEntityState != StarState.NOT_ACTIVE;
            }
            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;

                    if(this.StarEntityState == StarState.TRANSITIONING)
                    {
                        this.PlayAnimation(this.CreateTransitioningAnimation());
                    }
                }
            }
        }

        public IntRect HitZone
        {
            get
            {
                return new IntRect((int)(this.Position.X - this.Canevas.Width / 2),
                    (int)(this.Position.Y - this.Canevas.Height / 2),
                    this.Canevas.Width,
                    this.Canevas.Height);
            }
        }

        public StarEntity2D(IObject2DFactory factory, ALayer2D parentLayer, StarEntity entity):
            base(parentLayer, factory, entity)
        {

            Shader shader = new Shader(null, null, @"Assets\Graphics\Shaders\StarFrag.frag");

            Texture distortionMap = factory.GetTextureById("distortionTexture");
            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            shader.SetUniform("distortionMapTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.isFocused = true;
            this.IsFocused = false;

            this.SetCardSocketed(entity.CardSocketed);

            this.ObjectSprite.Texture = factory.GetTextureById("starTexture");

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            // Active animation
            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.LOOP);

            IAnimation anim = new ZoomAnimation(1, 1.5f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(1.5f, 1, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(2, anim);

            this.animationsList.Add(sequence);

            // Start : Transitioning active animation
            Random rand = new Random();
            float startTime = (float) (rand.NextDouble() * 2);
            sequence = new SequenceAnimation(Time.FromSeconds(startTime + 2), AnimationType.ONETIME);

            anim = new ZoomAnimation(0, 0, Time.FromSeconds(startTime), AnimationType.ONETIME, InterpolationMethod.STEP);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(0f, 1f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(startTime, anim);

            this.animationsList.Add(sequence);

            this.InitializeState(entity);
        }

        public void SetCardSocketed(CardEntity cardEntity)
        {
            if (cardEntity != null)
            {
                this.PlaySound("cardSocketed");
                this.ObjectSprite.Color = cardEntity.Card.CurrentOwner.PlayerColor;
            }
            else
            {
                this.ObjectSprite.Color = DEFAULT_COLOR;
            }
        }

        private IAnimation CreateTransitioningAnimation()
        {
            IAnimation result;
            if (this.isActive)
            {
                float time = Math.Abs(1 - this.Zoom) * 2;
                result = new ZoomAnimation(this.Zoom, 1f, Time.FromSeconds(time), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            }
            else
            {
                float time = Math.Abs(this.Zoom) * 2;
                result = new ZoomAnimation(this.Zoom, 0f, Time.FromSeconds(time), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            }

            return result;
        }

        private void InitializeState(StarEntity entity)
        {
            this.isActive = entity.IsActive;
            if (this.isActive)
            {
                this.PlayAnimation(1);

                this.StarEntityState = StarState.TRANSITIONING;
            }
            else
            {
                this.StartNotActiveState();
            }
        }

        private void StartTransitioningState()
        {
            this.StarEntityState = StarState.TRANSITIONING;

            this.PlayAnimation(this.CreateTransitioningAnimation());
        }

        private void StartNotActiveState()
        {
            this.StarEntityState = StarState.NOT_ACTIVE;
        }

        private void StartActiveState()
        {
            this.StarEntityState = StarState.ACTIVE;

            this.PlayAnimation(0);
        }

        private void UpdateTransitioning(Time deltaTime)
        {
            if (this.IsAnimationRunning() == false)
            {
                if(this.isActive)
                {
                    this.StartActiveState();
                }
                else
                {
                    this.StartNotActiveState();
                }
            }
        }

        private void UpdateActive(Time deltaTime)
        {
            if (this.isActive == false)
            {
                this.StartTransitioningState();
            }
        }

        private void UpdateNotActive(Time deltaTime)
        {
            if (this.isActive)
            {
                this.StartTransitioningState();
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.StarEntityState)
            {
                case StarState.ACTIVE:
                    this.UpdateActive(deltaTime);
                    break;
                case StarState.TRANSITIONING:
                    this.UpdateTransitioning(deltaTime);
                    break;
                case StarState.NOT_ACTIVE:
                    this.UpdateNotActive(deltaTime);
                    break;
            }

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite, this.render);
        }

        public bool OnMousePressed(ALayer2D parentLayer, ControlEventType eventType)
        {
            return true;
        }

        public bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (parentLayer is BoardGameLayer2D)
            {
                if (parentLayer.FocusedGraphicEntity2D == this)
                {
                    if (eventType == ControlEventType.MOUSE_LEFT_CLICK)
                    {
                        BoardGameLayer2D boardGameLayer2D = (parentLayer as BoardGameLayer2D);

                        if (boardGameLayer2D.CardPicked != null)
                        {
                            StarEntity starEntity = parentLayer.GetEntityFromEntity2D(this) as StarEntity;
                            CardEntity cardEntity = parentLayer.GetEntityFromEntity2D(boardGameLayer2D.CardPicked) as CardEntity;

                            if (starEntity.CanSocketCard(cardEntity))
                            {
                                boardGameLayer2D.SendEventToWorld(Model.Event.EventType.SOCKET_CARD, starEntity, null);
                            }
                        }
                        else if (boardGameLayer2D.SourceCardEntities2D != null && boardGameLayer2D.SourceCardEntities2D.Count > 0)
                        {
                            StarEntity starEntity = parentLayer.GetEntityFromEntity2D(this) as StarEntity;

                            boardGameLayer2D.SendEventToWorld(Model.Event.EventType.PICK_CARD, starEntity.CardSocketed, null);
                        }
                    }
                }
                
            }
            return true;
        }

        public bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            return true;
        }

        public void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            StarEntity starEntity = parentLayer.GetEntityFromEntity2D(this) as StarEntity;

            if (starEntity.CardSocketed != null)
            {
                parentLayer.SendEventToWorld(Model.Event.EventType.FOCUS_CARD_BOARD, starEntity.CardSocketed, null);
            }
        }

        public void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            parentLayer.SendEventToWorld(Model.Event.EventType.FOCUS_CARD_BOARD, null, null);
        }

        public bool IsFocusable(ALayer2D parentLayer)
        {
            return this.IsActive;
        }

        public enum StarState
        {
            NOT_ACTIVE,
            TRANSITIONING,
            ACTIVE
        }
    }
}
