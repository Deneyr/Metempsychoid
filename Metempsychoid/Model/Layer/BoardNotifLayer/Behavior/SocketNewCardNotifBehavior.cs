using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Card;
using Astrategia.Model.Event;
using Astrategia.Model.Layer.BoardGameLayer;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public class SocketNewCardNotifBehavior : ACardNotifBehavior
    {
        private SocketNewCardState state;

        protected bool mustNotifyBehaviorEnd;

        public List<string> NewCardIds
        {
            get;
            set;
        }

        public List<StarEntity> ToStarEntities
        {
            get;
            set;
        }

        public SocketNewCardState State
        {
            get
            {
                return this.state;
            }

            protected set
            {
                if (this.state != value)
                {
                    this.state = value;

                    switch (this.state)
                    {
                        case SocketNewCardState.PICK_CARD:
                            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(null);
                            //this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player).SetBehaviorSourceCardEntities(this.FromCardEntities);
                            break;
                        case SocketNewCardState.SOCKET_CARD:
                            //this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player).SetBehaviorSourceCardEntities(null);
                            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
                            break;
                    }

                    this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorPhaseChanged("MoveCardNotifBehavior." + this.State.ToString());
                }
            }
        }

        public SocketNewCardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity, List<string> newCardIds) 
            : base(cardBehaviorOwner, ownerCardEntity)
        {
            this.ToStarEntities = new List<StarEntity>();

            this.NewCardIds = newCardIds;

            this.state = SocketNewCardState.VOID;
        }

        public override void EndNotif(World world)
        {
            base.EndNotif(world);

            //this.FromStarEntities.Clear();
            //this.FromCardEntities.Clear();
            this.ToStarEntities.Clear();
            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);

            this.NodeLevel.BoardNotifLayer.RemoveCardsInHand();

            BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner);

            //currentPlayerLayer.SetBehaviorSourceCardEntities(this.FromCardEntities);

            //this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override void StartNotif(World world)
        {
            base.StartNotif(world);

            this.mustNotifyBehaviorEnd = false;

            //this.CardBehaviorOwner.OnBehaviorStart(this);

            //this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player).CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.CEMETERY;

            foreach (string cardId in this.NewCardIds)
            {
                Card.Card newCard = world.CardLibrary.CreateCard(cardId, this.OwnerCardEntity.Card.CurrentOwner);

                this.NodeLevel.BoardNotifLayer.AddCardToBoard(newCard, new SFML.System.Vector2f(0, 0));
            }

            this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner).CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.NONE;

            this.State = SocketNewCardState.PICK_CARD;
        }

        public override void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            base.HandleGameEvents(gameEvents);

            switch (this.state)
            {
                case SocketNewCardState.PICK_CARD:
                    this.HandlePickState(gameEvents);
                    break;
                case SocketNewCardState.SOCKET_CARD:
                    this.HandleSocketState(gameEvents);
                    break;
            }
        }

        public override bool UpdateNotif(World world)
        {
            if (this.mustNotifyBehaviorEnd)
            {
                //this.CardBehaviorOwner.OnBehaviorEnd(this);
                this.mustNotifyBehaviorEnd = false;

                if (this.IsActive)
                {
                    this.State = SocketNewCardState.PICK_CARD;
                }
            }

            return this.IsActive;
        }

        private void HandlePickState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            if (gameEvents.TryGetValue(EventType.NEXT_BEHAVIOR, out List<GameEvent> gameEventsNextBehavior))
            {
                if (gameEventsNextBehavior.Any())
                {
                    this.IsActive = false;
                    this.mustNotifyBehaviorEnd = true;
                    return;
                }
            }

            if (gameEvents.TryGetValue(EventType.FOCUS_CARD_HAND, out List<GameEvent> gameEventsFocused))
            {
                CardEntity cardEntity = null;
                bool encounterGameEvent = false;

                foreach (GameEvent gameEvent in gameEventsFocused)
                {
                    if (gameEvent.Layer == this.NodeLevel.BoardNotifLayer)
                    {
                        if (cardEntity == null
                            || gameEvent.Entity != null)
                        {
                            cardEntity = gameEvent.Entity as CardEntity;

                            encounterGameEvent = true;
                        }
                    }
                }

                if (encounterGameEvent)
                {
                    this.NodeLevel.BoardGameLayer.CardEntityFocused = null;
                    this.NodeLevel.BoardGameLayer.DomainEntityFocused = null;

                    this.NodeLevel.BoardNotifLayer.CardEntityFocused = cardEntity;
                }
            }

            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == this.NodeLevel.BoardNotifLayer && pElem.Entity != null);

                if (boardGameEvent != null)
                {
                    CardEntity cardToRemove = boardGameEvent.Entity as CardEntity;

                    //this.FromCardEntities.Remove(cardToRemove);

                    this.NodeLevel.PickCard(this.NodeLevel.BoardNotifLayer, cardToRemove);

                    this.CardBehaviorOwner.OnBehaviorCardPicked(this, this.NodeLevel.BoardGameLayer.CardEntityPicked);

                    this.State = SocketNewCardState.SOCKET_CARD;
                }
            }
        }

        private void HandleSocketState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            //BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player);

            if (gameEvents.TryGetValue(EventType.SOCKET_CARD, out List<GameEvent> gameEventsSocket))
            {
                GameEvent socketEvent = gameEventsSocket.FirstOrDefault();

                if (socketEvent != null)
                {
                    StarEntity starEntity = socketEvent.Entity as StarEntity;

                    if (this.ToStarEntities.Contains(starEntity))
                    {
                        this.ExecuteBehavior(starEntity);

                        this.NbBehaviorUse--;
                        this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorUseChanged(this.NbBehaviorUse);

                        this.mustNotifyBehaviorEnd = true;
                        return;
                    }
                }
            }

            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent nullPickEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == this.NodeLevel.BoardNotifLayer && pElem.Entity == null);
                GameEvent entityPickEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == this.NodeLevel.BoardNotifLayer && pElem.Entity != null);

                if (nullPickEvent != null)
                {
                    CardEntity cardToAdd = NodeLevel.UnpickCard(this.NodeLevel.BoardNotifLayer, nullPickEvent.Details);

                    //this.FromCardEntities.Add(cardToAdd);

                    this.State = SocketNewCardState.PICK_CARD;
                }

                if (entityPickEvent != null)
                {
                    CardEntity cardToRemove = entityPickEvent.Entity as CardEntity;

                    //this.FromCardEntities.Remove(cardToRemove);

                    this.NodeLevel.PickCard(this.NodeLevel.BoardNotifLayer, cardToRemove);

                    this.CardBehaviorOwner.OnBehaviorCardPicked(this, this.NodeLevel.BoardGameLayer.CardEntityPicked);

                    this.State = SocketNewCardState.SOCKET_CARD;
                }
            }

            if (gameEvents.TryGetValue(EventType.NEXT_BEHAVIOR, out List<GameEvent> gameEventsNextBehavior))
            {
                if (gameEventsNextBehavior.Any())
                {
                    this.IsActive = false;
                    this.mustNotifyBehaviorEnd = true;
                    return;
                }
            }
        }

        public override bool CanSocketCardOn(StarEntity starEntity, CardEntity cardToSocket)
        {
            return this.ToStarEntities.Contains(starEntity);
        }

        protected override void ExecuteBehavior(StarEntity starEntity)
        {
            if (this.NodeLevel.BoardGameLayer.CardEntityPicked != null)
            {
                this.ModifiedCardEntities.Add(this.NodeLevel.BoardGameLayer.CardEntityPicked);
            }

            this.NodeLevel.BoardGameLayer.SocketCard(starEntity, false);
        }

        public enum SocketNewCardState
        {
            VOID,
            PICK_CARD,
            SOCKET_CARD
        }
    }
}
