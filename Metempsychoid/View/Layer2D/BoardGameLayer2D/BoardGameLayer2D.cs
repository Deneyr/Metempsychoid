using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.Model.Player;
using Metempsychoid.View.Card2D;
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
    public class BoardGameLayer2D : ALayer2D
    {
        private List<CardEntity2D> cardsOnBoard;

        private int maxAwakenedPriority;

        private CardEntity2D cardPicked;

        private CardEntity2D cardFocused;

        private List<StarLinkEntity2D> linksFocused;

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
                    this.cardFocused = value;
                }
            }
        }

        public TurnPhase LevelTurnPhase
        {
            get;
            private set;
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

            this.linksFocused = new List<StarLinkEntity2D>();

            layer.CardPicked += this.OnCardPicked;
            layer.CardUnpicked += this.OnCardUnPicked;

            layer.CardFocused += this.OnCardFocused;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            this.maxAwakenedPriority = 0;

            this.cardsOnBoard.Clear();

            this.linksFocused.Clear();

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
            this.CardPicked = this.objectToObject2Ds[obj] as CardEntity2D;
            this.CardPicked.Priority = 1001;
        }

        private void OnCardUnPicked(CardEntity obj)
        {
            this.CardPicked.Priority = 1000;
            this.CardPicked = null;
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

            this.UpdateCardFocusedFillLink();
        }

        private void UpdateCardFocusedFillLink()
        {
            if(this.CardFocused != null && this.CardFocused.IsAwakened)
            {
                CardEntity cardEntity = this.object2DToObjects[this.CardFocused] as CardEntity;

                this.linksFocused.Clear();
                foreach (Constellation constellation in cardEntity.Card.Constellations)
                {
                    foreach(List<StarLinkEntity> listStarLinks in constellation.LinkToStarLinkEntity.Values)
                    {
                        this.linksFocused.AddRange(listStarLinks.Select(pElem => this.objectToObject2Ds[pElem] as StarLinkEntity2D));
                    }
                }

                foreach (StarLinkEntity2D starLinkEntity2D in this.linksFocused)
                {
                    starLinkEntity2D.FillRatioState = StarLinkEntity2D.TargetedFillRatioState.DOWN;
                }
            }
            else
            {
                foreach(StarLinkEntity2D starLinkEntity2D in this.linksFocused)
                {
                    starLinkEntity2D.FillRatioState = StarLinkEntity2D.TargetedFillRatioState.UP;
                }
                this.linksFocused.Clear();
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

                    this.CardPicked.Priority = 1000;
                    this.CardPicked = null;
                    break;
                case "IsSocketed":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsSocketed = ((obj as CardEntity).ParentStar != null);
                    break;
                case "IsFliped":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsFliped = (obj as CardEntity).IsFliped;
                    break;
                case "IsAwakened":
                    CardEntity2D cardAwakened = this.objectToObject2Ds[obj] as CardEntity2D;
                    cardAwakened.IsAwakened = (obj as CardEntity).Card.IsAwakened;

                    cardAwakened.Priority = 10000 + this.maxAwakenedPriority++;
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
                //Vector2i mousePosition = this.MousePosition;

                //StarEntity2D starEntity2D = this.GetStarEntity2DOn(mousePosition);

                //Vector2f cardPosition = new Vector2f(mousePosition.X, mousePosition.Y);

                ////CardEntity cardEntity = this.object2DToObjects[this.cardPicked] as CardEntity;

                //CardEntity cardSocketed = null;
                //CardEntity2D card2DSocketed = null;
                //if (starEntity2D != null)
                //{
                //    StarEntity starEntity = this.object2DToObjects[starEntity2D] as StarEntity;

                //    if (this.cardPicked != null)
                //    {
                //        CardEntity cardEntity = this.object2DToObjects[this.cardPicked] as CardEntity;

                //        if (starEntity.CanSocketCard(cardEntity))
                //        {
                //            cardPosition = new Vector2f(starEntity2D.Position.X, starEntity2D.Position.Y);
                //        }
                //    }

                //    cardSocketed = starEntity.CardSocketed;
                //}

                //if(cardSocketed != null)
                //{
                //    card2DSocketed = this.objectToObject2Ds[cardSocketed] as CardEntity2D;
                //}

                //if(card2DSocketed != this.cardFocused)
                //{
                //    this.SendEventToWorld(Model.Event.EventType.FOCUS_CARD_BOARD, cardSocketed, null);
                //}

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
            return this.objectToObject2Ds.Values.Where(pElem => pElem is StarEntity2D);
        }

        //private StarEntity2D GetStarEntity2DOn(Vector2i mousePosition)
        //{
        //    StarEntity2D starEntityResult = null;

        //    foreach (AObject2D object2D in this.objectToObject2Ds.Values)
        //    {
        //        StarEntity2D starEntity2D = object2D as StarEntity2D;

        //        if (starEntity2D != null
        //            && starEntity2D.HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            return starEntityResult;
        //        }
        //    }

        //    return starEntityResult;
        //}

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
                    if (eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed")
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

            this.LevelTurnPhase = TurnPhase.VOID;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardGameLayer).CardPicked -= this.OnCardPicked;
            (this.parentLayer as BoardGameLayer).CardUnpicked -= this.OnCardUnPicked;

            (this.parentLayer as BoardGameLayer).CardFocused -= this.OnCardFocused;

            base.Dispose();
        }
    }
}
