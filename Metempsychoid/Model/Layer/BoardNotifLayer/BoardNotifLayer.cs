using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Animation;
using Astrategia.Model.Animation;
using Astrategia.Model.Card;
using Astrategia.Model.Event;
using Astrategia.Model.Layer.BoardNotifLayer.Behavior;
using Astrategia.Model.Node;
using SFML.System;

namespace Astrategia.Model.Layer.BoardNotifLayer
{
    public class BoardNotifLayer : EntityLayer.EntityLayer
    {
        private static Vector2f HAND_POSITION = new Vector2f(350, 0);
        private static int HAND_CARD_SPACE = 100;

        private CardEntity cardFocused;

        private CardEntityAwakenedDecorator cardAwakened;

        public event Action<CardEntity> CardFocused;

        public event Action<CardEntity> CardCreated;
        public event Action<CardEntity> CardRemoved;

        public event Action<CardEntity> CardPicked;
        public event Action<CardEntity> CardUnpicked;

        public event Action<CardEntityAwakenedDecorator> CardAwakened;

        public event Action<IBoardNotifBehavior> NotifBehaviorStarted;
        public event Action<string> NotifBehaviorPhaseChanged;
        public event Action<int> NotifBehaviorUseChanged;
        public event Action<IBoardNotifBehavior> NotifBehaviorEnded;

        public List<IBoardNotifBehavior> NotifBehaviorsList
        {
            get;
            private set;
        }

        public IBoardNotifBehavior CurrentNotifBehavior
        {
            get;
            private set;
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

                        this.UpdateCardsHandPosition();

                        this.CardFocused?.Invoke(this.cardFocused);
                    }
                }
            }
        }

        public List<CardEntity> CardsHand
        {
            get;
            private set;
        }

        protected Vector2f HandPosition
        {
            get
            {
                Vector2f result = HAND_POSITION;

                return result;
            }
        }

        public BoardNotifLayer()
        {
            this.CardsHand = new List<CardEntity>();

            this.TypesInChunk.Add(typeof(CardEntityDecorator));
            this.TypesInChunk.Add(typeof(CardEntityAwakenedDecorator));
        }

        public void ForwardGameEventsToBehavior(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            this.CurrentNotifBehavior.HandleGameEvents(gameEvents);
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            this.NotifBehaviorsList = new List<IBoardNotifBehavior>();

            this.CurrentNotifBehavior = null;

            this.cardAwakened = null;
        }

        public void AddCardToBoard(Card.Card cardToAdd, Vector2f position)
        {
            CardEntity cardEntity = new CardEntity(this, cardToAdd, true);

            this.AddEntityToLayer(cardEntity);

            cardEntity.Position = position;

            this.CardsHand.Add(cardEntity);

            this.CardCreated?.Invoke(cardEntity);

            this.UpdateCardsHandPosition();
        }

        public bool PickCard(CardEntity cardToPick)
        {
            if (this.CardsHand.Contains(cardToPick))
            {
                this.CardPicked.Invoke(cardToPick);

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

            this.CardsHand.Add(cardEntity);

            this.CardUnpicked.Invoke(cardEntity);

            this.UpdateCardsHandPosition();

            return cardEntity;
        }

        public void RemoveCardsInHand()
        {
            List<CardEntity> cardsHandList = this.CardsHand.ToList();

            foreach (CardEntity cardEntity in cardsHandList)
            {
                this.CardRemoved?.Invoke(cardEntity);

                this.RemoveEntityFromLayer(cardEntity);
            }

            this.CardEntityFocused = null;
        }

        public override void RemoveEntityFromLayer(AEntity entity)
        {
            if (entity is CardEntity)
            {
                CardEntity cardEntity = entity as CardEntity;

                this.CardsHand.Remove(cardEntity);
            }
            base.RemoveEntityFromLayer(entity);
        }

        public void NotifyCardAwakened(CardEntity cardToNotify, int valueBeforeAwakened)
        {
            CardEntityAwakenedDecorator cardEntity = new CardEntityAwakenedDecorator(this, cardToNotify, valueBeforeAwakened);

            this.cardAwakened = cardEntity;

            this.AddEntityToLayer(cardEntity);

            this.CardAwakened?.Invoke(cardEntity);
        }

        public void RemoveCardAwakened()
        {
            if(this.cardAwakened != null)
            {
                this.CardAwakened?.Invoke(null);

                this.RemoveEntityFromLayer(this.cardAwakened);

                this.cardAwakened = null;
            }
        }

        public override void UpdateLogic(World world, Time deltaTime)
        {
            if(this.CurrentNotifBehavior != null)
            {
                if (this.CurrentNotifBehavior.UpdateNotif(world) == false)
                {
                    this.CurrentNotifBehavior.EndNotif(world);
                    this.NotifBehaviorEnded?.Invoke(this.CurrentNotifBehavior);
                    this.CurrentNotifBehavior = null;
                }
            }

            if (this.NotifBehaviorsList.Count > 0 && this.CurrentNotifBehavior == null)
            {
                this.CurrentNotifBehavior = this.NotifBehaviorsList.First();
                this.NotifBehaviorsList.RemoveAt(0);

                this.CurrentNotifBehavior.StartNotif(world);
                this.NotifBehaviorStarted?.Invoke(this.CurrentNotifBehavior);
            }
        }

        private void UpdateCardsHandPosition()
        {
            float startWidth = this.HandPosition.X + HAND_CARD_SPACE * this.CardsHand.Count / 2f;

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
                        newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, this.HandPosition.Y);
                    }
                    else if (cardFocusedEncountered)
                    {
                        newPosition = new Vector2f(startWidth - (i + 1) * HAND_CARD_SPACE, this.HandPosition.Y);
                    }
                    else
                    {
                        newPosition = new Vector2f(startWidth - (i - 1) * HAND_CARD_SPACE, this.HandPosition.Y);
                    }
                }
                else
                {
                    newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, this.HandPosition.Y);
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

        public void NotifyNotifBehaviorPhaseChanged(string newPhase)
        {
            this.NotifBehaviorPhaseChanged?.Invoke(newPhase);
        }

        public void NotifyNotifBehaviorUseChanged(int newUseCount)
        {
            this.NotifBehaviorUseChanged?.Invoke(newUseCount);
        }
    }
}
