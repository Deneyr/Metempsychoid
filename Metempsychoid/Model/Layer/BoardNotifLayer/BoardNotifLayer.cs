using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using Metempsychoid.Model.Node;
using SFML.System;

namespace Metempsychoid.Model.Layer.BoardNotifLayer
{
    public class BoardNotifLayer : EntityLayer.EntityLayer
    {
        private static Vector2f HAND_POSITION = new Vector2f(350, 0);
        private static int HAND_CARD_SPACE = 100;

        private CardEntityAwakenedDecorator cardFocused;

        public event Action<CardEntityAwakenedDecorator> CardFocused;

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

        public CardEntityAwakenedDecorator CardEntityFocused
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

        public List<CardEntityDecorator> CardsHand
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
            this.CardsHand = new List<CardEntityDecorator>();

            this.TypesInChunk.Add(typeof(CardEntityDecorator));
        }

        public void ForwardGameEventsToBehavior(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            this.CurrentNotifBehavior.HandleGameEvents(gameEvents);
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            this.NotifBehaviorsList = new List<IBoardNotifBehavior>();

            this.CurrentNotifBehavior = null;

            // this.CardAwakened = null;
        }

        public void AddCardToBoard(CardEntity cardToAdd, Vector2f position)
        {
            CardEntityDecorator cardEntity = new CardEntityDecorator(this, cardToAdd);
            this.AddEntityToLayer(cardEntity);

            cardEntity.Position = position;

            this.CardsHand.Add(cardEntity);

            this.UpdateCardsHandPosition();
        }

        public void NotifyCardAwakened(CardEntity cardToNotify, int valueBeforeAwakened)
        {
            CardEntityAwakenedDecorator cardEntity = new CardEntityAwakenedDecorator(this, cardToNotify, valueBeforeAwakened);

            this.AddEntityToLayer(cardEntity);

            this.CardAwakened?.Invoke(cardEntity);
        }

        public void RemoveCardFromBoard(CardEntityAwakenedDecorator cardToRemove)
        {
            this.RemoveEntityFromLayer(cardToRemove);
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
