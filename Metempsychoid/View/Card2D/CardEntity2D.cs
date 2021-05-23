using Metempsychoid.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Controls;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardEntity2D: AEntity2D, IHitRect
    {
        private static int WIDTH_BORDER = 15;

        private static float SPEED_LINK = 0.6f;

        private static float TIME_FLIP = 0.5f;

        private CardEntity2DFactory factory;
        private string cardName;

        private Sprite canevasSprite;
        private CardHalo2D cardHalo;
        private CardLabel2D cardLabel;

        private bool isSocketed;
        private Color playerColor;
        protected float ratioColor;

        protected bool isFliped;
        protected CardSideState sideState;
        protected int cardIndex;

        private float cooldownFocus;

        RenderStates render;

        Clock timer = new Clock();

        public bool IsAwakened
        {
            get
            {
                return this.cardHalo.IsActive;
            }
            set
            {
                this.cardHalo.IsActive = value;
            }
        }

        public string CardName
        {
            get
            {
                return this.cardName;
            }
            set
            {
                if(this.cardName != value)
                {
                    this.cardName = value;

                    if (this.isFliped)
                    {
                        this.InitializeFaceTransState(this.factory.GetIndexFromCardName(this.cardName));
                    }
                }  
            }
        }

        public bool IsFliped
        {
            get
            {
                return sideState == CardSideState.FACE 
                    && cardIndex > 1;
            }
            set
            {
                this.isFliped = value;

                switch (this.sideState)
                {
                    case CardSideState.FACE:
                        if(this.isFliped == false)
                        {
                            this.InitializeFaceTransState(1);
                        }
                        else
                        {
                            this.InitializeFaceTransState(this.factory.GetIndexFromCardName(this.cardName));
                        }
                        break;
                }
            }
        }

        public float RatioColor
        {
            get
            {
                return this.ratioColor;
            }
            protected set
            {
                if (this.ratioColor != value)
                {
                    this.ratioColor = value;

                    render.Shader.SetUniform("ratioColor", this.ratioColor);
                }
            }
        }

        public Color PlayerColor
        {
            get
            {
                return this.playerColor;
            }
            protected set
            {
                if (this.playerColor != value)
                {
                    this.playerColor = value;

                    render.Shader.SetUniform("playerColor", new SFML.Graphics.Glsl.Vec4(this.playerColor.R / 255f, this.playerColor.G / 255f, this.playerColor.B / 255f, 1));

                    if (this.isSocketed)
                    {
                        this.RatioColor = 0;
                    }
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

                this.canevasSprite.Position = value * MainWindow.MODEL_TO_VIEW;

                this.cardHalo.Position = this.Position;

                this.cardLabel.Position = new Vector2f(this.Position.X, this.Position.Y - this.Canevas.Height / 2 + WIDTH_BORDER);
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }

            set
            {
                base.Rotation = value;

                this.canevasSprite.Rotation = value;
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
                if (this.Zoom != value)
                {
                    this.ObjectSprite.Scale = new Vector2f(value, 1);

                    this.canevasSprite.Scale = new Vector2f(value, 1);

                    this.UpdateScaling();
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

        public override IntRect Canevas
        {
            get
            {
                return this.canevasSprite.TextureRect;
            }
            set
            {
                base.Canevas = value;

                this.canevasSprite.TextureRect = new IntRect(0, 0, this.ObjectSprite.TextureRect.Width + 2 * WIDTH_BORDER, this.ObjectSprite.TextureRect.Height + 2 * WIDTH_BORDER);
            }
        }

        public bool IsSocketed
        {
            get
            {
                return this.isSocketed;
            }

            set
            {
                if(this.isSocketed != value)
                {
                    this.isSocketed = value;

                    render.Shader.SetUniform("isSocketed", this.isSocketed);

                    this.RatioColor = 0;
                }
            }
        }

        public int CardValue
        {
            get
            {
                return this.cardLabel.Label;
            }

            set
            {
                this.cardLabel.Label = value;
            }
        }

        public int CardBonus
        {
            get
            {
                return this.cardLabel.Bonus;
            }

            set
            {
                this.cardLabel.Bonus = value;
            }
        }


        public CardEntity2D(IObject2DFactory factory, ALayer2D layer2D, CardEntity entity) :
            base(layer2D, true)
        {
            this.factory = factory as CardEntity2DFactory;

            this.cardHalo = new CardHalo2D(factory, layer2D, this);
            this.cardLabel = new CardLabel2D(layer2D, entity.Card.Value);

            this.ObjectSprite.Texture = factory.GetTextureByIndex(1);

            this.ObjectSprite.Origin = new Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            this.canevasSprite = new Sprite();

            this.canevasSprite.TextureRect = new IntRect(0, 0, this.ObjectSprite.TextureRect.Width + 2 * WIDTH_BORDER, this.ObjectSprite.TextureRect.Height + 2 * WIDTH_BORDER);

            this.canevasSprite.Origin = new Vector2f(this.canevasSprite.TextureRect.Width / 2, this.canevasSprite.TextureRect.Height / 2);

            this.Position = entity.Position;
            this.Rotation = entity.Rotation;

            this.canevasSprite.Color = Color.Black;
            this.playerColor = Color.Black;
            this.isSocketed = entity.ParentStar == null;
            this.ratioColor = -1;

            Shader shader = new Shader(null, null, @"Assets\Graphics\Shaders\cardCanevas.frag");

            Texture distortionMap = factory.GetTextureByIndex(0);
            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            this.canevasSprite.Texture = distortionMap;
            shader.SetUniform("distortionMapTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            //SequenceAnimation flipAnimation = new SequenceAnimation(Time.FromSeconds(1), AnimationType.ONETIME);
            IAnimation animation = new FlipAnimation(0, (float)(Math.PI / 2), Time.FromSeconds(TIME_FLIP), AnimationType.ONETIME, InterpolationMethod.LINEAR, 1);
            //flipAnimation.AddAnimation(0, animation);
            this.animationsList.Add(animation);

            animation = new FlipAnimation((float)(Math.PI / 2), 0, Time.FromSeconds(TIME_FLIP), AnimationType.ONETIME, InterpolationMethod.LINEAR, 1);
            //flipAnimation.AddAnimation(0.5f, animation);
            this.animationsList.Add(animation);

            this.Initialize(entity);

            this.UpdateScaling();
            render.Shader.SetUniform("margin", ((float)(WIDTH_BORDER - 5)) / this.canevasSprite.Texture.Size.X);
            render.Shader.SetUniform("outMargin", 5f / this.canevasSprite.Texture.Size.X);
        }

        private void Initialize(CardEntity entity)
        {
            this.PlayerColor = entity.Card.Player.PlayerColor;
            this.cardHalo.SpriteColor = this.PlayerColor;

            this.IsSocketed = entity.ParentStar != null;
            this.RatioColor = 1;
            this.cardName = entity.Card.Name;
            this.isFliped = entity.IsFliped;

            this.cooldownFocus = 0;

            if (this.isFliped)
            {
                this.cardIndex = this.factory.GetIndexFromCardName(this.cardName);
            }
            else
            {
                this.cardIndex = 1;
            }

            this.InitializeFaceState();
        }

        public void SetCooldownFocus(float cooldownFocus)
        {
            this.cooldownFocus = cooldownFocus;
        }

        protected virtual void UpdateScaling()
        {
            render.Shader.SetUniform("widthRatio", ((float) this.canevasSprite.TextureRect.Width) / this.canevasSprite.Texture.Size.X);
            render.Shader.SetUniform("heightRatio", ((float) this.canevasSprite.TextureRect.Height) / this.canevasSprite.Texture.Size.Y);
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            this.UpdateCooldowns(deltaTime);

            this.UpdateColorRatio(deltaTime);

            this.cardHalo.UpdateGraphics(deltaTime);

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        private void UpdateCooldowns(Time deltaTime)
        {
            if(this.cooldownFocus > 0)
            {
                this.cooldownFocus -= deltaTime.AsSeconds();

                if(this.cooldownFocus < 0)
                {
                    this.cooldownFocus = 0;
                }
            }
        }


        private void UpdateColorRatio(Time deltaTime)
        {
            switch (this.sideState)
            {
                case CardSideState.TRANSITIONING_START:
                    this.UpdateTransitioningStartState(deltaTime);
                    break;
                case CardSideState.TRANSITIONING_END:
                    this.UpdateTransitioningEndState(deltaTime);
                    break;
            }

            if (this.RatioColor < 1)
            {
                this.RatioColor += SPEED_LINK * deltaTime.AsSeconds();

                if (this.RatioColor > 1)
                {
                    this.RatioColor = 1f;
                }
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            this.cardHalo.DrawIn(window, deltaTime);

            window.Draw(this.ObjectSprite);

            this.cardLabel.DrawIn(window, deltaTime);

            window.Draw(this.canevasSprite, this.render);
        }

        private void InitializeFaceState()
        {
            this.isFliped = true;
            this.ObjectSprite.Texture = this.factory.GetTextureByIndex(this.cardIndex);

            this.sideState = CardSideState.FACE;

            if (this.IsFliped)
            {
                this.cardLabel.ShowLabel();
            }
        }

        private void InitializeFaceTransState(int cardIndex)
        {
            this.cardIndex = cardIndex;

            this.sideState = CardSideState.TRANSITIONING_START;

            this.PlayAnimation(0);

            this.cardLabel.HideLabel();
        }

        private void InitializeTransitioningEndState()
        {
            if (this.isFliped)
            {
                this.ObjectSprite.Texture = this.factory.GetTextureByIndex(this.cardIndex);
            }
            else
            {
                this.ObjectSprite.Texture = this.factory.GetTextureByIndex(1);
            }

            this.sideState = CardSideState.TRANSITIONING_END;

            this.PlayAnimation(1);
        }

        private void UpdateTransitioningStartState(Time deltaTime)
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializeTransitioningEndState();
            }
        }

        private void UpdateTransitioningEndState(Time deltaTime)
        {
            if (this.IsAnimationRunning() == false)
            {
                this.InitializeFaceState();
            }
        }

        // Part IHitRect

        public void OnMousePressed(ALayer2D parentLayer, ControlEventType eventType)
        {

        }

        public void OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (parentLayer.FocusedGraphicEntity2D == this)
            {
                Layer2D.BoardPlayerLayer2D.BoardPlayerLayer2D boardPlayerLayer2D = parentLayer as Layer2D.BoardPlayerLayer2D.BoardPlayerLayer2D;
                if (boardPlayerLayer2D != null && boardPlayerLayer2D.LevelTurnPhase == Model.Node.TestWorld.TurnPhase.MAIN)
                {
                    Vector2i mousePosition = parentLayer.MousePosition;

                    mousePosition.Y -= (int)(this.Bounds.Height / 2);
                    if (eventType == ControlEventType.MOUSE_LEFT_CLICK)
                    {
                        parentLayer.SendEventToWorld(Model.Event.EventType.PICK_CARD, parentLayer.GetEntityFromEntity2D(this), mousePosition.X + ":" + mousePosition.Y);
                    }
                }
            }
        }

        public void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            parentLayer.SendEventToWorld(Model.Event.EventType.FOCUS_CARD_HAND, parentLayer.GetEntityFromEntity2D(this), null);
        }

        public void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            parentLayer.SendEventToWorld(Model.Event.EventType.FOCUS_CARD_HAND, null, null);
        }

        public bool IsFocusable(ALayer2D parentLayer)
        {
            Layer2D.BoardPlayerLayer2D.BoardPlayerLayer2D boardPlayerLayer2D = parentLayer as Layer2D.BoardPlayerLayer2D.BoardPlayerLayer2D;
            if (boardPlayerLayer2D != null)
            {
                return this.cooldownFocus <= 0 && boardPlayerLayer2D.LevelTurnPhase == Model.Node.TestWorld.TurnPhase.MAIN;
            }
            return false;
        }

        public enum CardSideState
        {
            FACE,
            TRANSITIONING_START,
            TRANSITIONING_END
        }
    }
}
