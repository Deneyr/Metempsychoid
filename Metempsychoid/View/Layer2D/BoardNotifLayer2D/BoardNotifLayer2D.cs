using Astrategia.Animation;
using Astrategia.Model;
using Astrategia.Model.Animation;
using Astrategia.Model.Layer.BoardNotifLayer;
using Astrategia.Model.Layer.BoardNotifLayer.Behavior;
using Astrategia.Model.Node.TestWorld;
using Astrategia.View.Card2D;
using Astrategia.View.Controls;
using Astrategia.View.Layer2D.BoardBannerLayer2D;
using Astrategia.View.Layer2D.BoardPlayerLayer2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardNotifLayer2D
{
    public class BoardNotifLayer2D : ALayer2D, ICardFocusedLayer
    {
        private static float COOLDOWN_FOCUS = 2;

        private AwakenedBannerLabel2D awakenedBannerLabel2D;
        private EffectBanner2D effectBanner2D;
        private EffectLabel2D effectLabel2D;

        private List<CardEntity2D> cardsHand;

        private CardEntity2D cardFocused;
        private CardEntityAwakenedDecorator2D cardAwakened;

        private List<AEntity2D> hittableEntities2D;

        private EndTurnButton2D endTurnButton;
        protected EffectBehaviorLabel2D effectBehaviorLabel2D;

        protected TurnPhase levelTurnPhase;

        private List<CardEntity2D> pendingRemovingCardEntities;

        public event Action<ICardFocusedLayer> CardFocusedChanged;

        public bool IsRunningBehavior
        {
            get;
            protected set;
        }

        public virtual TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            protected set
            {
                if (this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.CREATE_HAND:
                            break;
                        case TurnPhase.MAIN:
                            this.endTurnButton.ActiveButton(0);
                            break;
                        case TurnPhase.END_TURN:
                            this.FocusedGraphicEntity2D = null;

                            this.endTurnButton.DeactiveButton();
                            break;
                    }
                }
            }
        }

        public CardEntityAwakenedDecorator2D CardAwakened
        {
            get
            {
                return this.cardAwakened;
            }
            private set
            {
                if(value != this.cardAwakened)
                {
                    this.cardAwakened = value;

                    if (this.cardAwakened != null)
                    {
                        this.cardAwakened.DisplayAwakened();
                        this.awakenedBannerLabel2D.DisplayBanner();
                    }
                }
            }
        }

        public CardEntity2D CardFocused
        {
            get
            {
                return this.cardFocused;
            }
            set
            {
                if (this.cardFocused != value)
                {
                    this.cardFocused = value;

                    this.CardFocusedChanged?.Invoke(this);
                }
            }
        }

        //protected override Vector2f DefaultViewSize
        //{
        //    set
        //    {
        //        if (value != this.DefaultViewSize)
        //        {
        //            base.DefaultViewSize = value;

        //            IntRect endTurnButtonCanvevas = this.endTurnButton.Canevas;
        //            this.endTurnButton.Position = new Vector2f(-endTurnButtonCanvevas.Width / 2, this.DefaultViewSize.Y / 2 - endTurnButtonCanvevas.Height);

        //            IntRect effectBehaviorLabelCanvevas = this.effectBehaviorLabel2D.Canevas;
        //            this.effectBehaviorLabel2D.StartingPosition = new Vector2f(-this.DefaultViewSize.X / 2 - effectBehaviorLabelCanvevas.Width, 0);
        //        }
        //    }
        //}

        public BoardNotifLayer2D(World2D world2D, IObject2DFactory factory, BoardNotifLayer layer) :
            base(world2D, factory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.hittableEntities2D = new List<AEntity2D>();

            this.awakenedBannerLabel2D = new AwakenedBannerLabel2D(this);
            this.effectBanner2D = new EffectBanner2D(this);

            this.effectLabel2D = new EffectLabel2D(this);

            this.endTurnButton = new EndTurnButton2D(this);
            this.effectBehaviorLabel2D = new EffectBehaviorLabel2D(this);

            this.pendingRemovingCardEntities = new List<CardEntity2D>();

            this.cardsHand = new List<CardEntity2D>();

            layer.CardCreated += OnCardCreated;
            layer.CardRemoved += OnCardRemoved;

            layer.CardPicked += OnCardPicked;
            layer.CardUnpicked += OnCardUnpicked;

            layer.CardFocused += OnCardFocused;

            layer.CardAwakened += OnCardAwakened;

            layer.NotifBehaviorStarted += OnNotifBehaviorStarted;
            layer.NotifBehaviorPhaseChanged += OnNotifBehaviorPhaseChanged;
            layer.NotifBehaviorUseChanged += OnNotifBehaviorUseChanged;
            layer.NotifBehaviorEnded += OnNotifBehaviorEnded;
        }

        private void OnCardFocused(Model.Card.CardEntity obj)
        {
            if (obj != null)
            {
                CardEntity2D cardFocused = this.GetEntity2DFromEntity(obj) as CardEntity2D;

                this.CardFocused = cardFocused;
                //this.cardToolTip.DisplayToolTip(obj.Card, cardFocused);
            }
            else
            {
                this.CardFocused = null;
                //this.cardToolTip.HideToolTip();
            }
        }

        private void OnCardPicked(Model.Card.CardEntity obj)
        {
            this.cardsHand.Remove(this.GetEntity2DFromEntity(obj) as CardEntity2D);

            this.UpdateCardHandPriority();
        }

        private void OnCardUnpicked(Model.Card.CardEntity obj)
        {
            CardEntity2D cardPicked = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsHand.Add(cardPicked);

            cardPicked.SetCooldownFocus(COOLDOWN_FOCUS);

            this.UpdateCardHandPriority();
        }

        private void OnCardCreated(Model.Card.CardEntity obj)
        {
            CardEntity2D cardEntity = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsHand.Add(cardEntity);

            this.UpdateCardHandPriority();
        }

        private void OnCardRemoved(Model.Card.CardEntity obj)
        {
            CardEntity2D cardEntity2DToRemove = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsHand.Remove(cardEntity2DToRemove);

            this.UpdateCardHandPriority();

            cardEntity2DToRemove.PlayRemoveAnimation();
        }

        protected virtual void OnNotifBehaviorStarted(IBoardNotifBehavior obj)
        {
            this.IsRunningBehavior = true;

            if (obj.IsThereBehaviorLabel)
            {
                this.effectBehaviorLabel2D.ActiveLabel(obj);

                if(obj is ACardNotifBehavior)
                {
                    this.effectBehaviorLabel2D.Label = (obj as ACardNotifBehavior).NbBehaviorUse;
                }
            }

            if(obj.IsThereEndButton == false)
            {
                this.endTurnButton.DeactiveButton();
            }
            else
            {
                if (this.endTurnButton.IsActive)
                {
                    this.endTurnButton.SetParagraph(1);
                }
                else
                {
                    this.endTurnButton.ActiveButton(1);
                }
            }
        }

        private void OnNotifBehaviorUseChanged(int obj)
        {
            if (this.IsRunningBehavior && this.effectBehaviorLabel2D.IsActive)
            {
                this.effectBehaviorLabel2D.Label = obj;
            }
        }

        private void OnNotifBehaviorPhaseChanged(string obj)
        {
            if (this.endTurnButton.IsActive)
            {
                this.endTurnButton.SetParagraph(obj);
            }
        }

        protected virtual void OnNotifBehaviorEnded(IBoardNotifBehavior obj)
        {
            this.IsRunningBehavior = false;

            if (obj.IsThereBehaviorLabel)
            {
                this.effectBehaviorLabel2D.DeactiveLabel();
            }

            if (this.endTurnButton.IsActive == false)
            {
                this.endTurnButton.ActiveButton(0);
            }
            else
            {
                this.endTurnButton.SetParagraph(0);
            }
        }

        protected override void OnEntityRemoved(AEntity obj)
        {
            CardEntity2D cardEntity2DToRemove = this.objectToObject2Ds[obj] as CardEntity2D;

            if (cardEntity2DToRemove != null)
            {
                this.pendingRemovingCardEntities.Add(cardEntity2DToRemove);
            }
            else
            {
                base.OnEntityRemoved(obj);
            }
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            this.cardsHand.Clear();

            base.InitializeLayer(factory);

            this.cardFocused = null;
            this.cardAwakened = null;

            this.pendingRemovingCardEntities.Clear();

            this.LevelTurnPhase = TurnPhase.VOID;

            this.IsRunningBehavior = false;
        }

        public override void InitializeSpatialLayer()
        {
            float maxZoom = Math.Max(1920 / this.DefaultViewSize.X, 1080 / this.DefaultViewSize.Y);

            this.Zoom = maxZoom;
        }

        protected override void OnDefaultViewSizeChanged()
        {
            IntRect endTurnButtonCanvevas = this.endTurnButton.Canevas;
            this.endTurnButton.Position = new Vector2f(-endTurnButtonCanvevas.Width / 2, this.DefaultViewSize.Y * this.Zoom / 2 - endTurnButtonCanvevas.Height);

            IntRect effectBehaviorLabelCanvevas = this.effectBehaviorLabel2D.Canevas;
            this.effectBehaviorLabel2D.StartingPosition = new Vector2f(-this.DefaultViewSize.X * this.Zoom / 2 - effectBehaviorLabelCanvevas.Width, 0);
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj);
        }

        private void OnCardAwakened(CardEntityAwakenedDecorator obj)
        {
            if (obj != null)
            {
                this.CardAwakened = this.objectToObject2Ds[obj] as CardEntityAwakenedDecorator2D;
            }
            else
            {
                this.CardAwakened = null;
            }
        }

        public Vector2f GetPositionFrom(ALayer layerFrom, Vector2f position)
        {
            Vector2f originPosition = new Vector2f();
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                originPosition = this.GetPositionInScene(world2D.LayersDictionary[layerFrom].GetPositionInWindow(position));
            }

            return originPosition;
        }

        private void UpdateCardHandPriority()
        {
            int i = 0;
            foreach (CardEntity2D cardEntity2D in this.cardsHand)
            {
                cardEntity2D.Priority = 1000 + i;
                i++;
            }
        }

        //protected override AEntity2D AddEntity(AEntity obj)
        //{
        //    AEntity2D entity2D = base.AddEntity(obj);

        //    return entity2D;
        //}

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "Value":
                    CardEntityAwakenedDecorator2D cardConcerned2D = this.objectToObject2Ds[obj] as CardEntityAwakenedDecorator2D;
                    CardEntityAwakenedDecorator cardConcerned = (obj as CardEntityAwakenedDecorator);

                    cardConcerned2D.CardValue = cardConcerned.Card.Value;
                    cardConcerned2D.CardValueModifier = cardConcerned.Card.ValueModifier;
                    break;
               
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            this.endTurnButton.UpdateGraphics(deltaTime);

            if(this.pendingRemovingCardEntities.Count > 0)
            {
                List<CardEntity2D> currentRemovingCards = this.pendingRemovingCardEntities.Where(pElem => pElem.IsAnimationRunning() == false).ToList();  
                
                foreach(CardEntity2D removingCard in currentRemovingCards)
                {
                    base.OnEntityRemoved(this.object2DToObjects[removingCard]);

                    this.pendingRemovingCardEntities.Remove(removingCard);
                }
            }

            if (this.CardAwakened != null)
            {
                switch (this.CardAwakened.DecoratorState)
                {
                    case CardEntityAwakenedDecorator2D.CardDecoratorState.PENDING:

                        if (this.effectBanner2D.IsActive == false)
                        {
                            this.effectBanner2D.DisplayEffectBanner();

                            this.effectLabel2D.DisplayEffectLabel((this.object2DToObjects[this.CardAwakened] as CardEntityAwakenedDecorator).Card.EffectIdLoc);
                        }
                        else if (this.effectBanner2D.IsAnimationRunning() == false)
                        {
                            this.CardAwakened.HideAwakened();

                            this.effectBanner2D.IsActive = false;
                            this.effectLabel2D.IsActive = false;
                        }

                        break;
                    case CardEntityAwakenedDecorator2D.CardDecoratorState.FINISHED:
                        AEntity cardEntityDecorator = this.object2DToObjects[this.CardAwakened];

                        this.CardAwakened = null;

                        this.SendEventToWorld(Model.Event.EventType.NEXT_BEHAVIOR, cardEntityDecorator, string.Empty);
                        break;
                }
            }
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details, bool mustForwardEvent)
        {
            mustForwardEvent = base.OnControlActivated(eventType, details, mustForwardEvent);

            if(eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed"
                || eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "click")
            //if (eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "click")
            {
                Vector2i mousePosition = this.MousePosition;

                this.SendEventToWorld(Model.Event.EventType.PICK_CARD, null, mousePosition.X + ":" + mousePosition.Y);
            }

            return mustForwardEvent;
        }

        public void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            this.SendEventToWorld(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase));
        }

        public void SendUnpickEvent()
        {
            Vector2i mousePosition = this.MousePosition;

            //mousePosition.Y -= (int)this.OffsetCard;

            this.SendEventToWorld(Model.Event.EventType.PICK_CARD, null, mousePosition.X + ":" + mousePosition.Y);
        }

        protected override IEnumerable<AEntity2D> GetEntities2DFocusable()
        {
            this.hittableEntities2D.Clear();

            if(this.cardsHand != null && this.cardsHand.Count > 0)
            {
                this.hittableEntities2D.AddRange(this.cardsHand);
            }

            this.hittableEntities2D.Add(this.endTurnButton);

            return this.hittableEntities2D;
        }

        //// TO REMOVE
        //protected override void UpdateFocusedEntity2Ds()
        //{
        //    Vector2i mousePosition = this.MousePosition;

        //    AEntity2D newFocusedEntity2D = null;
        //    IEnumerable<AEntity2D> focusableEntities2D = this.GetEntities2DFocusable();
        //    foreach (AEntity2D entity2D in focusableEntities2D)
        //    {
        //        IHitRect hitRect = entity2D as IHitRect;

        //        if (hitRect != null
        //            && hitRect.IsFocusable(this)
        //            && hitRect.HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            if (newFocusedEntity2D == null
        //                || (Math.Abs(mousePosition.X - entity2D.Position.X) + Math.Abs(mousePosition.Y - entity2D.Position.Y)
        //                    < Math.Abs(mousePosition.X - newFocusedEntity2D.Position.X) + Math.Abs(mousePosition.Y - newFocusedEntity2D.Position.Y)))
        //            {
        //                newFocusedEntity2D = entity2D;
        //            }
        //        }
        //    }

        //    this.FocusedGraphicEntity2D = newFocusedEntity2D as IHitRect;
        //}
        ////

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            //FloatRect bounds = this.Bounds;
            //foreach (CardEntity2D removingCardEntity2D in this.pendingRemovingCardEntities)
            //{
            //    if (removingCardEntity2D.IsActive
            //        && removingCardEntity2D.Bounds.Intersects(bounds))
            //    {
            //        removingCardEntity2D.DrawIn(window, deltaTime);
            //    }
            //}

            this.awakenedBannerLabel2D.DrawIn(window, deltaTime);

            this.effectBanner2D.DrawIn(window, deltaTime);
            this.effectLabel2D.DrawIn(window, deltaTime);

            this.effectBehaviorLabel2D.DrawIn(window, deltaTime);
            this.endTurnButton.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        public override void Dispose()
        {
            if(this.effectBanner2D != null)
            {
                this.effectBanner2D.Dispose();
            }

            if (this.effectLabel2D != null)
            {
                this.effectLabel2D.Dispose();
            }

            (this.parentLayer as BoardNotifLayer).CardCreated -= OnCardCreated;
            (this.parentLayer as BoardNotifLayer).CardRemoved -= OnCardRemoved;

            (this.parentLayer as BoardNotifLayer).CardPicked -= OnCardPicked;
            (this.parentLayer as BoardNotifLayer).CardUnpicked -= OnCardUnpicked;

            (this.parentLayer as BoardNotifLayer).CardFocused -= OnCardFocused;

            (this.parentLayer as BoardNotifLayer).CardAwakened -= OnCardAwakened;

            (this.parentLayer as BoardNotifLayer).NotifBehaviorStarted -= OnNotifBehaviorStarted;
            (this.parentLayer as BoardNotifLayer).NotifBehaviorPhaseChanged -= OnNotifBehaviorPhaseChanged;
            (this.parentLayer as BoardNotifLayer).NotifBehaviorEnded -= OnNotifBehaviorEnded;

            base.Dispose();
        }
    }
}
