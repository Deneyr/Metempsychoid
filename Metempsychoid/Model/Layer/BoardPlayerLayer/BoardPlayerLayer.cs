using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using Metempsychoid.Model.Node;
using Metempsychoid.Model.Node.TestWorld;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardPlayerLayer
{
    public class BoardPlayerLayer : EntityLayer.EntityLayer
    {
        private static int TOP_DECK = 3;

        private static Vector2f DECK_POSITION = new Vector2f(-600, -150);
        private static Vector2f CEMETERY_POSITION = new Vector2f(-600, -150);
        private static Vector2f HAND_POSITION = new Vector2f(700, -150);

        private static int HAND_CARD_SPACE = 50;
        private static int CEMETERY_CARD_SPACE = 10;

        private bool isHandShowed;
        private bool isCemeteryShowed;

        private int nbCardsToDraw;

        private CardEntity cardFocused;

        private PileFocused pileFocused;
        private PileFocused pilePicked;

        public event Action<CardEntity, PileFocused> CardPicked;
        public event Action<CardEntity, PileFocused> CardUnpicked;

        public event Action<CardEntity> CardDestroyed;
        // public event Action<CardEntity> CardResurrected;

        public event Action<CardEntity> CardDrawn;

        public event Action<CardEntity> CardFocused;

        public event Action<PileFocused> PileFocusedChanged;

        public event Action<int> NbCardsToDrawChanged;

        public event Action<List<CardEntity>> SourceCardEntitiesSet;

        public PileFocused CardPileFocused
        {
            get
            {
                return this.pileFocused;
            }

            set
            {
                if(this.pileFocused != value)
                {
                    this.pileFocused = value;

                    switch (this.pileFocused)
                    {
                        case PileFocused.CEMETERY:
                            this.IsHandShowed = false;
                            this.IsCemeteryShowed = true;
                            break;
                        case PileFocused.HAND:
                            this.IsHandShowed = true;
                            this.IsCemeteryShowed = false;
                            break;
                        //case PileFocused.FULL:
                        //    this.IsHandShowed = true;
                        //    this.IsCemeteryShowed = true;
                        //    break;
                        case PileFocused.NONE:
                            this.IsHandShowed = false;
                            this.IsCemeteryShowed = false;
                            break;
                    }

                    this.PileFocusedChanged?.Invoke(this.pileFocused);
                }
            }
        }

        public List<CardEntity> BehaviorSourceCardEntities
        {
            get;
            private set;
        }

        public IBoardToLayerPositionConverter BoardToLayerPositionConverter
        {
            get;
            set;
        }

        public Player.Player SupportedPlayer
        {
            get;
            private set;
        }

        public int IndexPlayer
        {
            get;
            private set;
        }

        public bool IsActiveTurn
        {
            get;
            private set;
        }

        protected Vector2f DeckPosition
        {
            get
            {
                Vector2f result = DECK_POSITION;

                if (this.IndexPlayer != 0)
                {
                    result.Y *= -1;
                }

                return result;
            }
        }

        protected Vector2f CemeteryPosition
        {
            get
            {
                Vector2f result = CEMETERY_POSITION;

                if (this.IndexPlayer != 0)
                {
                    result.Y *= -1;
                }

                //if (this.isCemeteryShowed == false)
                //{
                //    if (this.IsActiveTurn)
                //    {
                //        result.Y *= -0.6f;
                //    }
                //    else
                //    {
                //        result.Y *= -2f;
                //    }
                //}

                if (this.isCemeteryShowed)
                {
                    result.X = 0;
                }

                return result;
            }
        }

        protected int CemeterySpace
        {
            get
            {
                if (this.pileFocused == PileFocused.CEMETERY)
                {
                    return HAND_CARD_SPACE;
                }
                return CEMETERY_CARD_SPACE;
            }
        } 

        protected Vector2f HandPosition
        {
            get
            {
                Vector2f result = HAND_POSITION;

                if (this.IndexPlayer != 0)
                {
                    result.Y *= -1;
                }

                //if (this.isHandShowed == false)
                //{
                //    if (this.IsActiveTurn)
                //    {
                //        result.Y *= -0.6f;
                //    }
                //    else
                //    {
                //        result.Y *= -1f;
                //    }
                //}

                if (this.isHandShowed)
                {
                    result.X = 0;
                }

                return result;
            }
        }

        protected int HandSpace
        {
            get
            {
                if (this.pileFocused == PileFocused.HAND)
                {
                    return HAND_CARD_SPACE;
                }
                return CEMETERY_CARD_SPACE;
            }
        }

        protected bool IsHandShowed
        {
            get
            {
                return this.isHandShowed;
            }
            set
            {
                if(this.isHandShowed != value)
                {
                    this.isHandShowed = value;

                    this.UpdateCardsHandPosition();
                }
            }
        }

        protected bool IsCemeteryShowed
        {
            get
            {
                return this.isCemeteryShowed;
            }
            set
            {
                if (this.isCemeteryShowed != value)
                {
                    this.isCemeteryShowed = value;

                    this.UpdateCardsCimeteryPosition();
                }
            }
        }

        public List<CardEntity> CardsDeck
        {
            get;
            private set;
        }

        public List<CardEntity> CardsCemetery
        {
            get;
            private set;
        }

        public List<CardEntity> CardsHand
        {
            get;
            private set;
        }

        public int NbCardsToDraw
        {
            get
            {
                return this.nbCardsToDraw;
            }
            set
            {
                if(this.nbCardsToDraw != value)
                {
                    this.nbCardsToDraw = value;

                    this.NotifyNbCardsToDraw();
                }
            }
        }

        public CardEntity CardEntityFocused
        {
            get
            {
                return this.cardFocused;
            }

            set
            {
                if (value != this.cardFocused)
                {
                    if (value == null || this.Entities.Contains(value))
                    {
                        this.cardFocused = value;

                        if (this.cardFocused == null || this.CardsHand.Contains(this.cardFocused))
                        {
                            this.UpdateCardsHandPosition();
                        }

                        if (this.cardFocused == null || this.CardsCemetery.Contains(this.cardFocused))
                        {
                            this.UpdateCardsCimeteryPosition();
                        }

                        this.NotifyCardFocused(this.cardFocused);
                    }
                }
            }
        }

        public BoardPlayerLayer()
        {
            this.CardsDeck = new List<CardEntity>();
            this.CardsCemetery = new List<CardEntity>();
            this.CardsHand = new List<CardEntity>();

            this.BehaviorSourceCardEntities = null;

            this.TypesInChunk.Add(typeof(CardEntity));

            this.BoardToLayerPositionConverter = null;
        }

        public void SetBehaviorSourceCardEntities(List<CardEntity> sourceCardEntities)
        {
            // unpick ?

            this.CardEntityFocused = null;

            if (sourceCardEntities != null && sourceCardEntities.Count > 0)
            {
                this.BehaviorSourceCardEntities = new List<CardEntity>(sourceCardEntities);
            }
            else
            {
                this.BehaviorSourceCardEntities = null;
            }

            this.SourceCardEntitiesSet?.Invoke(this.BehaviorSourceCardEntities);
        }

        public override void FlushLayer()
        {
            this.CardsDeck.Clear();
            this.CardsHand.Clear();
            this.CardsCemetery.Clear();

            base.FlushLayer();
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            int i = 0;

            this.CardsDeck.Clear();
            this.CardsHand.Clear();
            this.CardsCemetery.Clear();

            this.isCemeteryShowed = false;
            this.isHandShowed = true;
            this.pileFocused = PileFocused.HAND;
            this.pilePicked = PileFocused.NONE;

            this.IsActiveTurn = false;

            this.SupportedPlayer = (levelNode as CardBoardLevel).GetPlayerFromIndex(world, out int currentPlayerIndex);
            this.IndexPlayer = currentPlayerIndex;

            List<Card.Card> deckCards = this.SupportedPlayer.ConstructDeck(world.CardLibrary);

            foreach (Card.Card card in deckCards)
            {
                CardEntity cardEntity = new CardEntity(this, card, false);

                cardEntity.Position = this.DeckPosition;

                cardEntity.IsActive = i < TOP_DECK;

                this.CardsDeck.Add(cardEntity);
                this.AddEntityToLayer(cardEntity);

                i++;
            }

            this.nbCardsToDraw = 0;

            this.cardFocused = null;
        }

        public bool DrawCard(bool isFliped = true)
        {
            //Random random = new Random();
            //if(random.NextDouble() < 0.4)
            //{
            //    if (this.CardsDeck.Any()
            //    && this.NbCardsToDraw > 0)
            //    {
            //        CardEntity cardEntity = this.CardsDeck.FirstOrDefault();
            //        this.CardsDeck.RemoveAt(0);

            //        cardEntity.IsFliped = isFliped;

            //        this.AddCardToCemetery(cardEntity.Card, new Vector2f(0, 0));
            //        this.RemoveEntityFromLayer(cardEntity);

            //        this.NbCardsToDraw -= 1;
            //    }


            //    return false;
            //}


            if (this.CardsDeck.Any()
                && this.NbCardsToDraw > 0)
            {
                CardEntity cardEntity = this.CardsDeck.FirstOrDefault();
                this.CardsDeck.RemoveAt(0);

                cardEntity.IsFliped = isFliped;

                this.CardsHand.Add(cardEntity);

                if (this.CardsDeck.Count >= TOP_DECK)
                {
                    this.CardsDeck[TOP_DECK - 1].IsActive = true;
                }

                this.NotifyCardDrawn(cardEntity);

                this.UpdateCardsHandPosition();

                this.NbCardsToDraw -= 1;

                return true;
            }

            return false;
        }

        public void OnStartTurn()
        {
            this.IsActiveTurn = true;

            foreach(CardEntity cardInHand in this.CardsHand)
            {
                cardInHand.IsFliped = true;
            }
        }

        public void OnEndTurn()
        {
            this.IsActiveTurn = false;

            foreach (CardEntity cardInHand in this.CardsHand)
            {
                cardInHand.IsFliped = false;
            }
        }

        public bool PickCard(CardEntity cardToPick)
        {
            bool isCardHand = this.CardsHand.Contains(cardToPick);
            bool isCardCemetery = this.CardsCemetery.Contains(cardToPick);

            if (isCardHand || isCardCemetery)
            {
                this.pilePicked = isCardHand ? PileFocused.HAND : PileFocused.CEMETERY;

                this.NotifyCardPicked(cardToPick, this.pilePicked);

                this.RemoveEntityFromLayer(cardToPick);

                this.CardEntityFocused = null;

                return true;
            }
            return false;
        }

        public CardEntity UnpickCard(Card.Card cardToUnpick, Vector2f startPosition)
        {
            CardEntity cardEntity = new CardEntity(this, cardToUnpick, true);

            this.AddEntityToLayer(cardEntity);

            cardEntity.Position = startPosition;

            switch (this.pilePicked)
            {
                case PileFocused.HAND:
                    this.CardsHand.Add(cardEntity);
                    break;
                case PileFocused.CEMETERY:
                    this.CardsCemetery.Add(cardEntity);
                    break;
            }

            this.NotifyCardUnpicked(cardEntity, this.pilePicked);

            switch (this.pilePicked)
            {
                case PileFocused.HAND:
                    this.UpdateCardsHandPosition();
                    break;
                case PileFocused.CEMETERY:
                    this.UpdateCardsCimeteryPosition();
                    break;
            }

            this.pilePicked = PileFocused.NONE;

            return cardEntity;
        }

        public void AddCardToCemetery(Card.Card cardToAdd, Vector2f startPosition)
        {
            CardEntity cardEntity = new CardEntity(this, cardToAdd, true);

            this.AddEntityToLayer(cardEntity);

            if(this.BoardToLayerPositionConverter != null)
            {
                CardBoardLevel testLevelNode = this.ownerLevelNode as CardBoardLevel;

                startPosition = this.BoardToLayerPositionConverter.BoardToLayerPosition(testLevelNode.BoardGameLayer, startPosition);
            }

            cardEntity.Position = startPosition;

            this.CardsCemetery.Add(cardEntity);

            this.CardDestroyed?.Invoke(cardEntity);

            this.UpdateCardsCimeteryPosition();
        }

        public override void RemoveEntityFromLayer(AEntity entity)
        {
            if (entity is CardEntity)
            {
                CardEntity cardEntity = entity as CardEntity;

                this.CardsDeck.Remove(cardEntity);

                this.CardsHand.Remove(cardEntity);

                this.CardsCemetery.Remove(cardEntity);
            }
            base.RemoveEntityFromLayer(entity);
        }

        private void UpdateCardsCimeteryPosition()
        {
            float startWidth = this.CemeteryPosition.X + this.CemeterySpace * this.CardsCemetery.Count / 2f;

            int i = 0;
            bool cardFocusedEncountered = false;

            foreach (CardEntity cardEntity in this.CardsCemetery)
            {
                Vector2f newPosition;
                cardFocusedEncountered |= this.cardFocused == cardEntity;

                if (this.cardFocused != null)
                {
                    if (this.cardFocused == cardEntity)
                    {
                        newPosition = new Vector2f(startWidth - i * this.CemeterySpace, this.CemeteryPosition.Y);
                    }
                    else if (cardFocusedEncountered)
                    {
                        newPosition = new Vector2f(startWidth - (i + 4) * this.CemeterySpace, this.CemeteryPosition.Y);
                    }
                    else
                    {
                        newPosition = new Vector2f(startWidth - (i - 4) * this.CemeterySpace, this.CemeteryPosition.Y);
                    }
                }
                else
                {
                    newPosition = new Vector2f(startWidth - i * this.CemeterySpace, this.CemeteryPosition.Y);
                }

                IAnimation positionAnimation;
                if (this.cardFocused != null)
                {
                    positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
                }
                else
                {
                    positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(2f), AnimationType.ONETIME, InterpolationMethod.SIGMOID);
                }

                cardEntity.PlayAnimation(positionAnimation);
                i++;

            }
        }

        private void UpdateCardsHandPosition()
        {
            float startWidth = this.HandPosition.X + this.HandSpace * this.CardsHand.Count / 2f;

            int i = 0;
            bool cardFocusedEncountered = false;

            foreach (CardEntity cardEntity in this.CardsHand)
            {
                Vector2f newPosition;
                cardFocusedEncountered |= this.cardFocused == cardEntity;

                if (this.cardFocused != null)
                {
                    if (this.cardFocused == cardEntity)
                    {
                        newPosition = new Vector2f(startWidth - i * this.HandSpace, this.HandPosition.Y);
                    }
                    else if (cardFocusedEncountered)
                    {
                        newPosition = new Vector2f(startWidth - (i + 4) * this.HandSpace, this.HandPosition.Y);
                    }
                    else
                    {
                        newPosition = new Vector2f(startWidth - (i - 4) * this.HandSpace, this.HandPosition.Y);
                    }
                }
                else
                {
                    newPosition = new Vector2f(startWidth - i * this.HandSpace, this.HandPosition.Y);
                }

                IAnimation positionAnimation;
                if (this.cardFocused != null)
                {
                    positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
                }
                else
                {
                    positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(2f), AnimationType.ONETIME, InterpolationMethod.SIGMOID);
                }

                cardEntity.PlayAnimation(positionAnimation);
                i++;

            }
        }

        protected void NotifyCardDrawn(CardEntity obj)
        {
            this.CardDrawn?.Invoke(obj);
        }

        protected void NotifyNbCardsToDraw()
        {
            this.NbCardsToDrawChanged?.Invoke(this.NbCardsToDraw);
        }

        protected void NotifyCardPicked(CardEntity cardPicked, PileFocused pilePicked)
        {
            this.CardPicked?.Invoke(cardPicked, pilePicked);
        }

        protected void NotifyCardFocused(CardEntity cardFocused)
        {
            this.CardFocused?.Invoke(cardFocused);
        }

        protected void NotifyCardUnpicked(CardEntity cardUnpicked, PileFocused pilePicked)
        {
            this.CardUnpicked?.Invoke(cardUnpicked, pilePicked);
        }

        public enum PileFocused
        {
            HAND = 0,
            CEMETERY = 1,
            NONE = 2
        }
    }
}
