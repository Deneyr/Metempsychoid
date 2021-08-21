using Metempsychoid.Animation;
using Metempsychoid.Model;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Layer2D.BoardBannerLayer2D;
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
    public class BoardPlayerLayer2D: ALayer2D, IBoardToLayerPositionConverter, ICardFocusedLayer, IScoreLayer
    {
        private static float COOLDOWN_FOCUS = 2;
        private static float COOLDOWN_DRAW = 0.2f;
        private float currentCooldownDraw;

        private List<AEntity2D> hittableEntities2D;

        private List<CardEntity2D> cardsDeck;
        private List<CardEntity2D> cardsCemetery;
        private List<CardEntity2D> cardsHand;

        private List<CardEntity2D> sourceCardEntities;

        private CardEntity2D cardDrawn;
        private CardEntity2D cardFocused;
        //private CardEntity2D cardPicked;

        //private CardToolTip cardToolTip;
        //private EndTurnButton2D endTurnButton;
        private ScoreLabel2D scoreLabel;

        private int maxPriority;

        private int nbCardsToDraw;

        private TurnPhase levelTurnPhase;

        public event Action<ICardFocusedLayer> CardFocusedChanged;

        public List<CardEntity2D> SourceCardEntities2D
        {
            get
            {
                return this.sourceCardEntities;
            }

            set
            {
                if (this.sourceCardEntities != null)
                {
                    foreach (CardEntity2D cardEntity2D in this.sourceCardEntities)
                    {
                        cardEntity2D.IsFocused = false;
                    }
                }

                this.sourceCardEntities = value;

                if (this.sourceCardEntities != null)
                {
                    foreach (CardEntity2D cardEntity2D in this.sourceCardEntities)
                    {
                        cardEntity2D.IsFocused = true;
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

                    if (this.cardFocused != null)
                    {
                        this.cardFocused.PlaySound("cardFocused");
                    }

                    this.CardFocusedChanged?.Invoke(this);
                }
            }
        }

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

                    //if ((this.parentLayer as BoardPlayerLayer).IsActiveTurn)
                    //{
                        switch (this.levelTurnPhase)
                        {
                            case TurnPhase.CREATE_HAND:
                                this.cardDrawn = null;
                                break;
                            case TurnPhase.MAIN:
                                //this.endTurnButton.ActiveButton();
                                break;
                            case TurnPhase.END_TURN:
                                this.FocusedGraphicEntity2D = null;

                                //this.endTurnButton.DeactiveButton();
                                break;
                        }
                    //}
                    //this.FocusedGraphicEntity2D = null;
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

                    //IntRect endTurnButtonCanvevas = this.endTurnButton.Canevas;
                    IntRect scoreLabelCanevas = this.scoreLabel.Canevas;

                    //this.endTurnButton.Position = new Vector2f(-endTurnButtonCanvevas.Width / 2, this.DefaultViewSize.Y / 2 - endTurnButtonCanvevas.Height);
                    this.scoreLabel.Position = new Vector2f(scoreLabelCanevas.Width / 2 - this.DefaultViewSize.X / 2 + 20, this.OffsetCard * 3/4);

                    foreach (KeyValuePair<AEntity, AEntity2D> pairEntity in this.objectToObject2Ds)
                    {
                        pairEntity.Value.Position = new Vector2f(pairEntity.Key.Position.X, pairEntity.Key.Position.Y + this.OffsetCard);
                    }
                }
            }
        }

        protected float OffsetCard
        {
            get
            {
                if ((this.parentLayer as BoardPlayerLayer).IndexPlayer == 0)
                {
                    return this.DefaultViewSize.Y / 2;
                }
                else
                {
                    return -this.DefaultViewSize.Y / 2;
                }
            }
        }

        public string PlayerName
        {
            get
            {
                return (this.parentLayer as BoardPlayerLayer).SupportedPlayer.PlayerName;
            }
        }

        public int PlayerScore
        {
            get
            {
                return this.scoreLabel.Score;
            }

            set
            {
                this.scoreLabel.Score = value;
            }
        }

        public BoardPlayerLayer2D(World2D world2D, IObject2DFactory factory, BoardPlayerLayer layer) :
            base(world2D, factory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.hittableEntities2D = new List<AEntity2D>();

            layer.CardDrawn += OnCardDrawn;
            layer.NbCardsToDrawChanged += OnNbCardToDrawsChanged;

            layer.CardFocused += OnCardFocused;

            layer.CardPicked += OnCardPicked;
            layer.CardUnpicked += OnCardUnpicked;

            layer.CardDestroyed += OnCardDestroyed;
            // layer.CardResurrected += OnCardResurrected;

            layer.SourceCardEntitiesSet += OnSourceCardEntitiesSet;

            //this.cardToolTip = new CardToolTip(this);
            //this.endTurnButton = new EndTurnButton2D(this);
            this.scoreLabel = new ScoreLabel2D(this);

            this.cardsDeck = new List<CardEntity2D>();
            this.cardsCemetery = new List<CardEntity2D>();
            this.cardsHand = new List<CardEntity2D>();

            layer.BoardToLayerPositionConverter = this;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            this.cardsDeck.Clear();
            this.cardsCemetery.Clear();
            this.cardsHand.Clear();

            this.maxPriority = 0;

            this.nbCardsToDraw = 0;

            this.currentCooldownDraw = 0;

            this.LevelTurnPhase = TurnPhase.VOID;

            this.cardDrawn = null;
            this.cardFocused = null;

            this.sourceCardEntities = null;
            //this.cardPicked = null;

            base.InitializeLayer(factory);

            BoardPlayerLayer parentBoardPlayerLayer = (this.parentLayer as BoardPlayerLayer);
            this.scoreLabel.DisplayScore(parentBoardPlayerLayer.IndexPlayer, parentBoardPlayerLayer.SupportedPlayer);
            this.scoreLabel.Score = 0;

            foreach (AEntity2D entity in this.objectToObject2Ds.Values)
            {
                if(entity is CardEntity2D)
                {
                    CardEntity2D cardEntity2D = entity as CardEntity2D;

                    cardEntity2D.Priority = this.maxPriority++;

                    this.cardsDeck.Add(cardEntity2D);
                }
            }
        }

        private void OnSourceCardEntitiesSet(List<CardEntity> obj)
        {
            if (obj != null && obj.Count > 0)
            {
                this.SourceCardEntities2D = obj.Select(pElem => this.objectToObject2Ds[pElem] as CardEntity2D).ToList();
            }
            else
            {
                this.SourceCardEntities2D = null;
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

                this.CardFocused = cardFocused;
                //this.cardToolTip.DisplayToolTip(obj.Card, cardFocused);
            }
            else
            {
                this.CardFocused = null;
                //this.cardToolTip.HideToolTip();
            }
        }

        private void OnCardDrawn(CardEntity obj)
        {
            this.cardDrawn = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardDrawn.PlaySound("cardDrawn");

            this.cardsDeck.Remove(this.cardDrawn);
            this.cardsHand.Add(this.cardDrawn);

            this.cardDrawn.SetCooldownFocus(COOLDOWN_FOCUS);

            this.UpdateCardHandPriority();
        }

        private void OnCardPicked(CardEntity obj)
        {
            CardEntity2D cardPicked = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            switch ((this.parentLayer as BoardPlayerLayer).CardPileFocused)
            {
                case BoardPlayerLayer.PileFocused.HAND:
                    this.cardsHand.Remove(cardPicked);
                    this.UpdateCardHandPriority();
                    break;
                case BoardPlayerLayer.PileFocused.CEMETERY:
                    this.cardsCemetery.Remove(cardPicked);
                    this.UpdateCardCimeteryPriority();
                    break;
            }
        }

        private void OnCardUnpicked(CardEntity obj)
        {
            CardEntity2D cardUnpicked = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            cardUnpicked.PlaySound("cardDrawn");

            switch ((this.parentLayer as BoardPlayerLayer).CardPileFocused)
            {
                case BoardPlayerLayer.PileFocused.HAND:
                    this.cardsHand.Add(cardUnpicked);
                    break;
                case BoardPlayerLayer.PileFocused.CEMETERY:
                    this.cardsCemetery.Add(cardUnpicked);
                    break;
            }

            cardUnpicked.SetCooldownFocus(COOLDOWN_FOCUS);

            switch ((this.parentLayer as BoardPlayerLayer).CardPileFocused)
            {
                case BoardPlayerLayer.PileFocused.HAND:
                    this.UpdateCardHandPriority();
                    break;
                case BoardPlayerLayer.PileFocused.CEMETERY:
                    this.UpdateCardCimeteryPriority();
                    break;
            }
        }

        //private void OnCardResurrected(CardEntity obj)
        //{
        //    this.cardsCemetery.Remove(this.GetEntity2DFromEntity(obj) as CardEntity2D);

        //    this.UpdateCardCimeteryPriority();
        //}

        private void OnCardDestroyed(CardEntity obj)
        {
            CardEntity2D cardDestroyed = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsCemetery.Add(cardDestroyed);

            this.UpdateCardCimeteryPriority();
        }

        private void UpdateCardHandPriority()
        {
            int i = 0;
            foreach (CardEntity2D cardEntity2D in this.cardsHand)
            {
                cardEntity2D.Priority = 2000 - i;
                i++;
            }
        }

        private void UpdateCardCimeteryPriority()
        {
            int i = 0;
            foreach (CardEntity2D cardEntity2D in this.cardsCemetery)
            {
                cardEntity2D.Priority = 2000 - i;
                i++;
            }
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            //this.cardToolTip.UpdateGraphics(deltaTime);
            //this.endTurnButton.UpdateGraphics(deltaTime);

            this.UpdateCardsToDraw(deltaTime);

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

            if ((this.parentLayer as BoardPlayerLayer).CardPileFocused == BoardPlayerLayer.PileFocused.HAND)
            {
                this.hittableEntities2D.AddRange(this.cardsHand);
            }
            else
            {
                if (this.SourceCardEntities2D != null)
                {
                    this.hittableEntities2D.AddRange(this.cardsCemetery.Where(pElem => this.SourceCardEntities2D.Contains(pElem)));
                }
            }

            //if (this.SourceCardEntities2D != null)
            //{
            //    this.hittableEntities2D.AddRange(this.SourceCardEntities2D);
            //}
            //else
            //{
            //    this.hittableEntities2D.AddRange(this.cardsHand);
            //}

            return this.hittableEntities2D;
        }

        private void UpdateCardsToDraw(Time deltaTime)
        {
            if (this.cardDrawn != null
                && this.currentCooldownDraw <= 0)
            {
                this.cardDrawn = null;
            }
            else
            {
                this.currentCooldownDraw -= deltaTime.AsSeconds();
            }

            if (this.nbCardsToDraw > 0
                && this.cardDrawn == null)
            {
                this.currentCooldownDraw = COOLDOWN_DRAW;
                this.SendEventToWorld(Model.Event.EventType.DRAW_CARD, null, string.Empty);
            }
        }

        private void UpdateCreateHandPhase(Time deltaTime)
        {
            if(this.nbCardsToDraw == 0
                && this.cardDrawn == null)
            {
                this.GoOnTurnPhase(TurnPhase.START_TURN);
            }
        }

        private void UpdateDrawPhase(Time deltaTime)
        {
            if (this.nbCardsToDraw == 0
                && this.cardDrawn == null)
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

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details, bool mustForwardEvent)
        {
            switch (this.levelTurnPhase)
            {
                case TurnPhase.MAIN:

                    if (eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed"
                        || eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "click")
                    {
                        this.SendUnpickEvent();
                    }
                    mustForwardEvent = base.OnControlActivated(eventType, details, mustForwardEvent);

                    break;
            }

            return true;
        }

        public void SendUnpickEvent()
        {
            Vector2i mousePosition = this.MousePosition;

            mousePosition.Y -= (int)this.OffsetCard;

            this.SendEventToWorld(Model.Event.EventType.PICK_CARD, null, mousePosition.X + ":" + mousePosition.Y);
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

            //this.cardToolTip.DrawIn(window, deltaTime);
            //this.endTurnButton.DrawIn(window, deltaTime);
            this.scoreLabel.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        protected override AEntity2D AddEntity(AEntity obj)
        {
            CardEntity2D entity2D = base.AddEntity(obj) as CardEntity2D;

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

                    entity2D.Position = new Vector2f(obj.Position.X, obj.Position.Y + this.OffsetCard);

                    //if(this.cardToolTip.CardFocused != null && this.cardToolTip.CardFocused == entity2D)
                    //{
                    //    this.cardToolTip.UpdatePosition();
                    //}
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

        public Vector2f BoardToLayerPosition(BoardGameLayer layer, Vector2f boardPosition)
        {
            Vector2f resultPosition = new Vector2f();
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                resultPosition = this.GetPositionInScene(world2D.LayersDictionary[layer].GetPositionInWindow(boardPosition));
            }

            return new Vector2f(resultPosition.X, resultPosition.Y - this.OffsetCard);
        }

        public Vector2f LayerToBoardPosition(BoardGameLayer layer, Vector2f layerPosition)
        {
            Vector2f resultPosition = new Vector2f();
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                layerPosition = new Vector2f(layerPosition.X, layerPosition.Y + this.OffsetCard);
                resultPosition = world2D.LayersDictionary[layer].GetPositionInScene(this.GetPositionInWindow(layerPosition));
            }

            return resultPosition;
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.cardsDeck.Clear();
            this.cardsCemetery.Clear();
            this.cardsHand.Clear();

            this.maxPriority = 0;

            this.nbCardsToDraw = 0;

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrawn = null;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardPlayerLayer).CardDrawn -= OnCardDrawn;
            (this.parentLayer as BoardPlayerLayer).NbCardsToDrawChanged -= OnNbCardToDrawsChanged;

            (this.parentLayer as BoardPlayerLayer).CardFocused -= OnCardFocused;

            (this.parentLayer as BoardPlayerLayer).CardPicked -= OnCardPicked;
            (this.parentLayer as BoardPlayerLayer).CardUnpicked -= OnCardUnpicked;

            (this.parentLayer as BoardPlayerLayer).CardDestroyed -= OnCardDestroyed;
            // (this.parentLayer as BoardPlayerLayer).CardResurrected -= OnCardResurrected;

            base.Dispose();
        }
    }
}
