using Metempsychoid.Animation;
using Metempsychoid.Model;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class BoardPlayerLayer2D: ALayer2D
    {
        private static float COOLDOWN_FOCUS = 2;

        private List<AEntity2D> hittableEntities2D;

        private List<CardEntity2D> cardsDeck;
        private List<CardEntity2D> cardsCemetery;
        private List<CardEntity2D> cardsHand;

        private CardEntity2D cardDrew;

        private CardToolTip cardToolTip;
        private EndTurnButton2D endTurnButton;

        private int maxPriority;

        private int nbCardsToDraw;

        private TurnPhase levelTurnPhase;

        public TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            private set
            {
                if(this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.CREATE_HAND:
                            this.cardDrew = null;
                            break;
                        case TurnPhase.MAIN:
                            this.endTurnButton.ActiveButton();
                            break;
                        case TurnPhase.END_TURN:
                            this.endTurnButton.DeactiveButton();
                            break;
                    }
                }
            }
        }


        protected override Vector2f DefaultViewSize
        {
            set
            {
                if (value != this.DefaultViewSize)
                {
                    base.DefaultViewSize = value;

                    foreach (KeyValuePair<AEntity, AEntity2D> pairEntity in this.objectToObject2Ds)
                    {
                        pairEntity.Value.Position = new Vector2f(pairEntity.Key.Position.X, this.DefaultViewSize.Y / 2 + pairEntity.Key.Position.Y);
                    }
                }
            }
        }

        public BoardPlayerLayer2D(World2D world2D, IObject2DFactory factory, BoardPlayerLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.hittableEntities2D = new List<AEntity2D>();

            layer.CardDrew += OnCardDrew;
            layer.NbCardsToDrawChanged += OnNbCardToDrawsChanged;

            layer.CardFocused += OnCardFocused;

            layer.CardPicked += OnCardPicked;
            layer.CardUnpicked += OnCardUnpicked;

            this.cardToolTip = new CardToolTip(this);
            this.endTurnButton = new EndTurnButton2D(this);
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            this.cardsDeck = new List<CardEntity2D>();
            this.cardsCemetery = new List<CardEntity2D>();
            this.cardsHand = new List<CardEntity2D>();

            this.maxPriority = 0;

            this.nbCardsToDraw = 0;

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrew = null;
            // this.cardFocused = null;

            IntRect endTurnButtonCanvevas = this.endTurnButton.Canevas;
            this.endTurnButton.Position = new Vector2f(-endTurnButtonCanvevas.Width / 2, this.view.Size.Y / 2 - endTurnButtonCanvevas.Height / 2);

            base.InitializeLayer(factory);

            foreach(AEntity2D entity in this.objectToObject2Ds.Values)
            {
                if(entity is CardEntity2D)
                {
                    CardEntity2D cardEntity2D = entity as CardEntity2D;

                    cardEntity2D.Priority = this.maxPriority++;

                    this.cardsDeck.Add(cardEntity2D);
                }
            }
        }

        private void OnNbCardToDrawsChanged(int obj)
        {
            this.nbCardsToDraw = obj;
        }

        private void OnCardFocused(CardEntity obj)
        {
            if (obj != null)
            {
                CardEntity2D cardFocused = this.GetEntity2DFromEntity(obj) as CardEntity2D;

                this.cardToolTip.DisplayToolTip(obj.Card, cardFocused);
            }
            else
            {
                this.cardToolTip.HideToolTip();
            }
        }

        private void OnCardDrew(CardEntity obj)
        {
            this.cardDrew = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsDeck.Remove(this.cardDrew);
            this.cardsHand.Add(this.cardDrew);

            this.cardDrew.SetCooldownFocus(COOLDOWN_FOCUS);

            this.UpdateCardEntitiesPriority();
        }

        private void OnCardPicked(CardEntity obj)
        {
            this.cardsHand.Remove(this.GetEntity2DFromEntity(obj) as CardEntity2D);

            this.UpdateCardEntitiesPriority();
        }

        private void OnCardUnpicked(CardEntity obj)
        {
            CardEntity2D cardPicked = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsHand.Add(cardPicked);

            cardPicked.SetCooldownFocus(COOLDOWN_FOCUS);

            this.UpdateCardEntitiesPriority();
        }

        private void UpdateCardEntitiesPriority()
        {
            int i = 0;
            foreach (CardEntity2D cardEntity2D in this.cardsHand)
            {
                cardEntity2D.Priority = 1000 + i;
                i++;
            }
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            this.cardToolTip.UpdateGraphics(deltaTime);
            this.endTurnButton.UpdateGraphics(deltaTime);

            this.UpdateCardsToDraw();

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.CREATE_HAND:
                    this.UpdateCreateHandPhase(deltaTime);
                    break;
                case TurnPhase.DRAW:
                    this.UpdateDrawPhase(deltaTime);
                    break;
                case TurnPhase.MAIN:
                    this.UpdateMainPhase(deltaTime);
                    break;
            }
        }

        protected override IEnumerable<AEntity2D> GetEntities2DFocusable()
        {
            this.hittableEntities2D.Clear();

            this.hittableEntities2D.AddRange(this.cardsHand);
            this.hittableEntities2D.Add(this.endTurnButton);

            return this.hittableEntities2D;
        }

        private void UpdateCardsToDraw()
        {
            if (this.cardDrew != null
                && this.cardDrew.IsFliped)
            {
                this.cardDrew = null;
            }

            if (this.nbCardsToDraw > 0
                && this.cardDrew == null)
            {
                this.SendEventToWorld(Model.Event.EventType.DRAW_CARD, null, string.Empty);
            }
        }

        private void UpdateCreateHandPhase(Time deltaTime)
        {
            if(this.nbCardsToDraw == 0
                && this.cardDrew == null)
            {
                this.GoOnTurnPhase(TurnPhase.START_TURN);
            }
        }

        private void UpdateDrawPhase(Time deltaTime)
        {
            if (this.nbCardsToDraw == 0
                && this.cardDrew == null)
            {
                this.GoOnTurnPhase(TurnPhase.MAIN);
            }
        }

        private void UpdateMainPhase(Time deltaTime)
        {
            this.UpdateFocusedEntity2Ds();

            //CardEntity2D cardFocused = this.GetCardFocused();

            //if (cardFocused != this.cardFocused)
            //{
            //    this.cardFocused = cardFocused;

            //    AEntity associatedCardFocused = null;
            //    if (this.cardFocused != null)
            //    {
            //        associatedCardFocused = this.object2DToObjects[cardFocused];
            //    }

            //    if (this.world2D.TryGetTarget(out World2D world))
            //    {
            //        world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.FOCUS_CARD_HAND, associatedCardFocused, null));
            //    }
            //}     
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details)
        {
            switch (this.levelTurnPhase)
            {
                case TurnPhase.MAIN:
                    //if(eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "pressed"
                    //    && this.cardFocused != null)
                    //{
                    //    Vector2i mousePosition = this.MousePosition;

                    //    mousePosition.Y -= (int)(this.view.Size.Y / 2);

                    //    AEntity associatedCardFocused = this.object2DToObjects[this.cardFocused];

                    //    if (this.world2D.TryGetTarget(out World2D world))
                    //    {
                    //        world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.PICK_CARD, associatedCardFocused, mousePosition.X + ":" + mousePosition.Y));
                    //    }
                    //}
                    //else if(eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed")
                    //{
                    //    Vector2i mousePosition = this.MousePosition;

                    //    mousePosition.Y -= (int)(this.view.Size.Y / 2);

                    //    if (this.world2D.TryGetTarget(out World2D world))
                    //    {
                    //        world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.PICK_CARD, null, mousePosition.X + ":" + mousePosition.Y));
                    //    }
                    //}

                    if (eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed")
                    {
                        Vector2i mousePosition = this.MousePosition;

                        mousePosition.Y -= (int)(this.view.Size.Y / 2);

                        this.SendEventToWorld(Model.Event.EventType.PICK_CARD, null, mousePosition.X + ":" + mousePosition.Y);
                    }
                    base.OnControlActivated(eventType, details);

                    break;
            }

            return true;
        }

        //private CardEntity2D GetCardFocused()
        //{
        //    CardEntity2D cardFocused = null;

        //    Vector2i mousePosition = this.MousePosition;

        //    foreach (CardEntity2D cardDeck in this.cardsHand)
        //    {
        //        if(cardDeck.IsFocusable
        //            && cardDeck is IHitRect
        //            && (cardDeck as IHitRect).HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            if(cardFocused == null
        //                || Math.Abs(mousePosition.X - cardDeck.Position.X) + Math.Abs(mousePosition.Y - cardDeck.Position.Y) 
        //                < Math.Abs(mousePosition.X - cardFocused.Position.X) + Math.Abs(mousePosition.Y - cardFocused.Position.Y))
        //            {
        //                cardFocused = cardDeck;
        //            }
        //        }
        //    }

        //    return cardFocused;
        //}

        public void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            this.SendEventToWorld(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase));
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.cardToolTip.DrawIn(window, deltaTime);
            this.endTurnButton.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        protected override AEntity2D AddEntity(AEntity obj)
        {
            CardEntity2D entity2D = base.AddEntity(obj) as CardEntity2D;

            if (entity2D != null)
            {
                entity2D.Position = new Vector2f(obj.Position.X, obj.Position.Y + this.view.Size.Y / 2);
            }

            return entity2D;
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            switch (propertyName)
            {
                case "IsFliped":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsFliped = (obj as CardEntity).IsFliped;
                    break;
                case "Position":
                    AEntity2D entity2D = this.objectToObject2Ds[obj];
                    entity2D.Position = new Vector2f(obj.Position.X, obj.Position.Y + this.view.Size.Y / 2);

                    if(this.cardToolTip.CardFocused != null && this.cardToolTip.CardFocused == entity2D)
                    {
                        this.cardToolTip.UpdatePosition();
                    }
                    break;
                case "IsActive":
                    this.objectToObject2Ds[obj].IsActive = obj.IsActive;
                    break;
            }
        }

        //protected override void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        //{
        //    this.DefaultViewSize = viewSize;
        //    this.view.Size = viewSize;

        //    foreach(KeyValuePair<AEntity, AEntity2D> pairEntity in this.objectToObject2Ds)
        //    {
        //        pairEntity.Value.Position = new Vector2f(pairEntity.Key.Position.X, this.view.Size.Y / 2 + pairEntity.Key.Position.Y);
        //    }
        //}

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.cardsDeck.Clear();

            this.cardsCemetery.Clear();

            this.cardsHand.Clear();

            this.maxPriority = 0;

            this.nbCardsToDraw = 0;

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrew = null;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardPlayerLayer).CardDrew -= OnCardDrew;
            (this.parentLayer as BoardPlayerLayer).NbCardsToDrawChanged -= OnNbCardToDrawsChanged;

            (this.parentLayer as BoardPlayerLayer).CardFocused -= OnCardFocused;

            (this.parentLayer as BoardPlayerLayer).CardPicked -= OnCardPicked;
            (this.parentLayer as BoardPlayerLayer).CardUnpicked -= OnCardUnpicked;

            base.Dispose();
        }
    }
}
