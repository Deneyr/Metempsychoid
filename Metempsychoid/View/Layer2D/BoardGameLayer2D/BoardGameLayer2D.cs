using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.Model.Player;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Layer2D.BoardBannerLayer2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class BoardGameLayer2D : ALayer2D, ICardFocusedLayer, IDomainsLayer
    {
        private List<CardEntity2D> cardsOnBoard;

        private List<CJStarDomain2D> domainsOwnedByPlayers;
        private int currenDomainEvaluatedIndex;

        private int maxAwakenedPriority;

        private CardEntity2D cardPicked;
        private CardEntity2D cardFocused;

        private List<CardEntity2D> sourceCardEntities;
        private List<StarEntity2D> sourceStarEntities;
        private List<CardEntity2D> targetCardEntities;
        private List<StarEntity2D> targetStarEntities;

        private List<StarLinkEntity2D> linksFocused;

        private TurnPhase levelTurnPhase;

        public event Action<ICardFocusedLayer> CardFocusedChanged;

        public event Action StartDomainEvaluated;
        public event Action<CJStarDomain> DomainEvaluated;
        public event Action EndDomainEvaluated;

        public List<CardEntity2D> SourceCardEntities2D
        {
            get
            {
                return this.sourceCardEntities;
            }

            set
            {
                if(this.sourceCardEntities != null)
                {
                    foreach(CardEntity2D cardEntity2D in this.sourceCardEntities)
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

        public List<StarEntity2D> TargetStarEntities2D
        {
            get
            {
                return this.targetStarEntities;
            }
            set
            {
                if (this.targetStarEntities != null)
                {
                    foreach (StarEntity2D starEntity2D in this.targetStarEntities)
                    {
                        starEntity2D.IsFocused = false;
                    }
                }

                this.targetStarEntities = value;

                if (this.targetStarEntities != null)
                {
                    foreach (StarEntity2D starEntity2D in this.targetStarEntities)
                    {
                        starEntity2D.IsFocused = true;
                    }
                }
            }
        }

        public List<CardEntity2D> TargetCardEntities2D
        {
            get
            {
                return this.targetCardEntities;
            }

            set
            {
                if (this.targetCardEntities != null)
                {
                    foreach (CardEntity2D cardEntity2D in this.targetCardEntities)
                    {
                        cardEntity2D.IsFocused = false;
                    }
                }

                this.targetCardEntities = value;

                if (this.targetCardEntities != null)
                {
                    foreach (CardEntity2D cardEntity2D in this.targetCardEntities)
                    {
                        cardEntity2D.IsFocused = true;
                    }
                }
            }
        }

        public CardEntity2D CardPicked
        {
            get
            {
                return this.cardPicked;
            }
            set
            {
                if (this.cardPicked != value)
                {
                    this.cardPicked = value;
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
                if(this.cardFocused != value)
                {
                    if(this.cardFocused != null)
                    {
                        this.cardFocused.IsFocused = false;

                        this.ClearCardFocusedFillLink();
                    }

                    this.cardFocused = value;

                    if (this.cardFocused != null)
                    {
                        this.cardFocused.IsFocused = true;

                        this.AddCardFocusedFillLink();
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
                if (this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.COUNT_POINTS:
                            this.InitializeCountPointsPhase();
                            break;
                    }
                }
            }
        }

        public override Vector2f Position
        {
            set
            {
                base.Position = value * 0.75f;
            }
        }

        public BoardGameLayer2D(World2D world2D, IObject2DFactory factory, BoardGameLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.cardsOnBoard = new List<CardEntity2D>();
            this.domainsOwnedByPlayers = new List<CJStarDomain2D>();

            this.linksFocused = new List<StarLinkEntity2D>();

            layer.CardPicked += this.OnCardPicked;
            //layer.CardUnpicked += this.OnCardUnPicked;

            layer.CardFocused += this.OnCardFocused;

            layer.SourceStarEntitiesSet += OnSourceStarEntitiesSet;
            layer.TargetStarEntitiesSet += OnTargetStarEntitiesSet;
        }

        private void OnSourceStarEntitiesSet(List<StarEntity> obj)
        {
            this.TargetStarEntities2D = null;
            this.TargetCardEntities2D = null;

            if (obj != null && obj.Count > 0)
            {
                this.sourceStarEntities = obj.Select(pElem => this.objectToObject2Ds[pElem] as StarEntity2D).ToList();
                this.SourceCardEntities2D = obj.Select(pElem => this.objectToObject2Ds[pElem.CardSocketed] as CardEntity2D).ToList();
            }
            else
            {
                this.sourceStarEntities = null;
                this.SourceCardEntities2D = null;
            }
        }

        private void OnTargetStarEntitiesSet(List<StarEntity> obj)
        {
            this.SourceCardEntities2D = null;

            if (obj != null && obj.Count > 0)
            {
                this.sourceStarEntities = obj.Select(pElem => this.objectToObject2Ds[pElem] as StarEntity2D).ToList();

                if (obj.First().CardSocketed != null)
                {
                    this.TargetStarEntities2D = null;
                    this.TargetCardEntities2D = obj.Select(pElem => this.objectToObject2Ds[pElem.CardSocketed] as CardEntity2D).ToList();
                }
                else
                {
                    this.TargetCardEntities2D = null;
                    this.TargetStarEntities2D = this.sourceStarEntities;
                }

            }
            else
            {
                this.sourceStarEntities = null;
                this.TargetStarEntities2D = null;
            }
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            this.maxAwakenedPriority = 0;

            this.cardsOnBoard.Clear();
            this.domainsOwnedByPlayers.Clear();

            this.linksFocused.Clear();

            this.sourceCardEntities = null;
            this.sourceStarEntities = null;
            this.targetCardEntities = null;
            this.targetStarEntities = null;

            base.InitializeLayer(factory);

            this.LevelTurnPhase = TurnPhase.VOID;

            this.cardPicked = null;
            this.cardFocused = null;
        }

        protected override AEntity2D AddEntity(AEntity obj)
        {
            AEntity2D entityAdded =  base.AddEntity(obj);

            if(entityAdded is CardEntity2D)
            {
                this.cardsOnBoard.Add(entityAdded as CardEntity2D);
            }

            return entityAdded;
        }

        protected override void OnEntityRemoved(AEntity obj)
        {
            if (obj is CardEntity)
            {
                this.cardsOnBoard.Remove(this.objectToObject2Ds[obj] as CardEntity2D);
            }

            base.OnEntityRemoved(obj);
        }

        private void OnCardPicked(CardEntity obj)
        {
            if (obj != null)
            {
                this.CardPicked = this.objectToObject2Ds[obj] as CardEntity2D;
                this.CardPicked.Priority = 3001;
            }
            else
            {
                this.CardPicked.Priority = 1000;
                this.CardPicked = null;
            }
        }

        private void OnCardFocused(CardEntity obj)
        {
            if (obj != null)
            {
                this.CardFocused = this.objectToObject2Ds[obj] as CardEntity2D;
            }
            else
            {
                this.CardFocused = null;
            }
        }

        private void ClearCardFocusedFillLink()
        {
            if (this.CardFocused.IsAwakened)
            {
                foreach (StarLinkEntity2D starLinkEntity2D in this.linksFocused)
                {
                    starLinkEntity2D.IsFocused = false;
                }
                this.linksFocused.Clear();
            }
        }

        private void AddCardFocusedFillLink()
        {
            if (this.CardFocused.IsAwakened)
            {
                CardEntity cardEntity = this.object2DToObjects[this.CardFocused] as CardEntity;

                this.linksFocused.Clear();
                foreach (Constellation constellation in cardEntity.Card.Constellations)
                {
                    foreach (List<StarLinkEntity> listStarLinks in constellation.LinkToStarLinkEntity.Values)
                    {
                        this.linksFocused.AddRange(listStarLinks.Select(pElem => this.objectToObject2Ds[pElem] as StarLinkEntity2D));
                    }
                }

                foreach (StarLinkEntity2D starLinkEntity2D in this.linksFocused)
                {
                    starLinkEntity2D.IsFocused = true;
                }
            }
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "CardSocketed":
                    StarEntity starEntity = obj as StarEntity;
                    StarEntity2D starEntity2D = this.objectToObject2Ds[obj] as StarEntity2D;

                    starEntity2D.SetCardSocketed(starEntity.CardSocketed);
                    break;
                case "IsSocketed":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsSocketed = ((obj as CardEntity).ParentStar != null);
                    break;
                case "IsFliped":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsFliped = (obj as CardEntity).IsFliped;
                    break;
                case "IsSelected":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsSelected = (obj as CardEntity).IsSelected;
                    break;
                case "IsAwakened":
                    CardEntity2D cardAwakened = this.objectToObject2Ds[obj] as CardEntity2D;
                    cardAwakened.IsAwakened = (obj as CardEntity).Card.IsAwakened;

                    cardAwakened.Priority = 2000 + this.maxAwakenedPriority++;
                    break;
                case "IsActive":
                    this.objectToObject2Ds[obj].IsActive = (obj as AEntity).IsActive;
                    break;
                case "Player":
                    CardEntity cardConcerned = obj as CardEntity;

                    (this.objectToObject2Ds[cardConcerned] as CardEntity2D).PlayerColor = cardConcerned.Card.Player.PlayerColor;
                    (this.objectToObject2Ds[cardConcerned.ParentStar] as StarEntity2D).SetCardSocketed(cardConcerned);
                    break;
                case "Value":
                    CardEntity2D card2DChanged = this.objectToObject2Ds[obj] as CardEntity2D;
                    CardEntity cardChanged = obj as CardEntity;
                    card2DChanged.CardValue = cardChanged.Card.Value;
                    card2DChanged.CardValueModifier = cardChanged.Card.ValueModifier;
                    break;
                case "DomainOwner":
                    CJStarDomain2D domain2DChanged = this.objectToObject2Ds[obj] as CJStarDomain2D;
                    CJStarDomain domainChanged = (obj as CJStarDomain);

                    if (domainChanged.DomainOwner != null)
                    {
                        domain2DChanged.TargetedColor = domainChanged.DomainOwner.PlayerColor;
                    }
                    else
                    {
                        domain2DChanged.TargetedColor = Color.Black;
                    }
                    break;

            }
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase) Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.START_LEVEL:
                    this.UpdateStartLevelPhase(deltaTime);
                    break;
                case TurnPhase.MAIN:
                    this.UpdateMainPhase(deltaTime);
                    break;
                case TurnPhase.COUNT_POINTS:
                    this.UpdateCountPointsPhase(deltaTime);
                    break;
            }
        }

        private void UpdateStartLevelPhase(Time deltaTime)
        {
            bool areAllLinksActive = true;
            foreach (KeyValuePair<AEntity, AEntity2D> pair in this.objectToObject2Ds)
            {
                if (pair.Value is StarLinkEntity2D)
                {
                    if (pair.Key.IsActive)
                    {
                        areAllLinksActive &= pair.Value.IsActive;
                    }
                }
            }

            if (areAllLinksActive)
            {
                this.GoOnTurnPhase(TurnPhase.CREATE_HAND);
            }
        }

        private void UpdateMainPhase(Time deltaTime)
        {

        }

        protected override void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        {
            base.UpdateViewSize(viewSize, deltaTime);

            this.UpdateCardInteraction();
        }

        private void UpdateCardInteraction()
        {
            if (this.CardPicked != null)
            {
                Vector2i mousePosition = this.MousePosition;

                Vector2f cardPosition = new Vector2f(mousePosition.X, mousePosition.Y);

                CardEntity cardEntity = this.object2DToObjects[this.CardPicked] as CardEntity;

                if (this.focusedGraphicEntity2D is StarEntity2D)
                {
                    StarEntity2D starEntity2D = this.focusedGraphicEntity2D as StarEntity2D;

                    StarEntity starEntity = this.object2DToObjects[starEntity2D] as StarEntity;

                    if (starEntity.CanSocketCard(cardEntity))
                    {
                        cardPosition = new Vector2f(starEntity2D.Position.X, starEntity2D.Position.Y);
                    }
                }

                this.CardPicked.Position = cardPosition;
            }
        }

        protected override IEnumerable<AEntity2D> GetEntities2DFocusable()
        {
            if(this.SourceCardEntities2D != null
                || this.TargetCardEntities2D != null)
            {
                return this.sourceStarEntities;
            }
            return this.objectToObject2Ds.Values.Where(pElem => pElem is StarEntity2D);
        }

        private void InitializeCountPointsPhase()
        {
            this.currenDomainEvaluatedIndex = -1;
            this.domainsOwnedByPlayers = this.objectToObject2Ds.Where(pElem => pElem.Value is CJStarDomain2D && (pElem.Key as CJStarDomain).IsThereAtLeastOneCard).Select(pElem => pElem.Value as CJStarDomain2D).ToList();

            if (this.domainsOwnedByPlayers.Count > 0)
            {
                this.StartDomainEvaluated?.Invoke();
                this.EvaluateDomain(0);
            }
        }

        private void UpdateCountPointsPhase(Time deltaTime)
        {
            if(this.currenDomainEvaluatedIndex >= 0)
            {
                CJStarDomain2D currentDomainEvaluated = this.domainsOwnedByPlayers[this.currenDomainEvaluatedIndex];

                if(currentDomainEvaluated.IsAnimationRunning() == false)
                {
                    currentDomainEvaluated.Priority = (this.object2DToObjects[currentDomainEvaluated] as CJStarDomain).Priority;

                    this.currenDomainEvaluatedIndex++;
                    if (this.currenDomainEvaluatedIndex < this.domainsOwnedByPlayers.Count)
                    {
                        this.EvaluateDomain(this.currenDomainEvaluatedIndex);
                    }
                    else
                    {
                        this.currenDomainEvaluatedIndex = -1;
                    }
                }
            }
            else
            {
                this.EndDomainEvaluated?.Invoke();
            }
        }

        private void EvaluateDomain(int index)
        {
            this.currenDomainEvaluatedIndex = index;
            CJStarDomain2D domainToEvaluate = this.domainsOwnedByPlayers[index];

            domainToEvaluate.Priority = 4000;
            this.domainsOwnedByPlayers[index].PlayAnimation(0);

            this.DomainEvaluated?.Invoke(this.object2DToObjects[domainToEvaluate] as CJStarDomain);
        }

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            this.SendEventToWorld(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase));
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details)
        {

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.MAIN:
                    base.OnControlActivated(eventType, details);

                    //if (eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "pressed")
                    //{
                    //    if (this.CardPicked != null)
                    //    {
                    //        StarEntity2D starEntity2D = this.focusedGraphicEntity2D as StarEntity2D;

                    //        if (starEntity2D != null)
                    //        {
                    //            StarEntity starEntity = this.object2DToObjects[starEntity2D] as StarEntity;
                    //            CardEntity cardEntity = this.object2DToObjects[this.CardPicked] as CardEntity;

                    //            if (starEntity.CanSocketCard(cardEntity))
                    //            {
                    //                this.SendEventToWorld(Model.Event.EventType.SOCKET_CARD, this.object2DToObjects[starEntity2D], null);
                    //            }
                    //        }
                    //    }
                    //    Pick card on board
                    //    else
                    //    {
                    //        CardEntity2D cardFocused = this.GetCardFocused();

                    //        if (cardFocused != null)
                    //        {
                    //            if (this.world2D.TryGetTarget(out World2D world))
                    //            {
                    //                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.PICK_CARD, this.object2DToObjects[cardFocused], null));
                    //            }
                    //        }
                    //    }
                    //}
                    if (eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed"
                        || eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "click")
                    {
                        if (this.CardPicked != null
                            && this.CardPicked.IsSocketed)
                        {
                            Vector2i mousePosition = this.MousePosition;
                            CardEntity cardEntity = this.object2DToObjects[this.CardPicked] as CardEntity;

                            this.SendEventToWorld(Model.Event.EventType.MOVE_CARD_OVERBOARD, cardEntity, mousePosition.X + ":" + mousePosition.Y);
                        }
                    }
                    break;
            }

            return true;
        }

        //private CardEntity2D GetCardFocused()
        //{
        //    CardEntity2D cardFocused = null;

        //    Vector2i mousePosition = this.MousePosition;

        //    foreach (CardEntity2D cardDeck in this.cardsOnBoard)
        //    {
        //        if (cardDeck.IsFocusable
        //            && cardDeck is IHitRect
        //            && (cardDeck as IHitRect).HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            if (cardFocused == null
        //                || Math.Abs(mousePosition.X - cardDeck.Position.X) + Math.Abs(mousePosition.Y - cardDeck.Position.Y)
        //                < Math.Abs(mousePosition.X - cardFocused.Position.X) + Math.Abs(mousePosition.Y - cardFocused.Position.Y))
        //            {
        //                cardFocused = cardDeck;
        //            }
        //        }
        //    }

        //    return cardFocused;
        //}

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.cardsOnBoard.Clear();
            this.domainsOwnedByPlayers.Clear();

            this.LevelTurnPhase = TurnPhase.VOID;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardGameLayer).CardPicked -= this.OnCardPicked;
            //(this.parentLayer as BoardGameLayer).CardUnpicked -= this.OnCardUnPicked;

            (this.parentLayer as BoardGameLayer).CardFocused -= this.OnCardFocused;

            (this.parentLayer as BoardGameLayer).SourceStarEntitiesSet -= OnSourceStarEntitiesSet;
            (this.parentLayer as BoardGameLayer).TargetStarEntitiesSet -= OnTargetStarEntitiesSet;

            base.Dispose();
        }
    }
}
