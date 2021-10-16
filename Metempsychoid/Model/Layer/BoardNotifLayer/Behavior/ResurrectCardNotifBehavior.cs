using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class ResurrectCardNotifBehavior : ACardNotifBehavior
    {
        private ResurrectState state;

        protected bool mustNotifyBehaviorEnd;

        public List<CardEntity> FromCardEntities
        {
            get;
            set;
        }

        public List<StarEntity> ToStarEntities
        {
            get;
            set;
        }

        public ResurrectState State
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
                        case ResurrectState.PICK_CARD:
                            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(null);
                            this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner).SetBehaviorSourceCardEntities(this.FromCardEntities);
                            break;
                        case ResurrectState.SOCKET_CARD:
                            //this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player).SetBehaviorSourceCardEntities(null);
                            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
                            break;
                    }

                    this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorPhaseChanged("MoveCardNotifBehavior." + this.State.ToString());
                }
            }
        }

        public ResurrectCardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity)
            : base(cardBehaviorOwner, ownerCardEntity)
        {
            //this.FromStarEntities = new List<StarEntity>();

            this.FromCardEntities = new List<CardEntity>();

            this.ToStarEntities = new List<StarEntity>();

            this.state = ResurrectState.VOID;
        }

        public override void EndNotif(World world)
        {
            base.EndNotif(world);

            //this.FromStarEntities.Clear();
            this.FromCardEntities.Clear();
            this.ToStarEntities.Clear();

            BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner);

            //this.NodeLevel.UnpickCard(currentPlayerLayer, )

            currentPlayerLayer.SetBehaviorSourceCardEntities(this.FromCardEntities);
            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
            currentPlayerLayer.CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.NONE;

            //this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override void StartNotif(World world)
        {
            base.StartNotif(world);

            this.mustNotifyBehaviorEnd = false;

            //this.CardBehaviorOwner.OnBehaviorStart(this);

            this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner).CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.CEMETERY;

            this.State = ResurrectState.PICK_CARD;
        }

        public override bool UpdateNotif(World world)
        {
            if (this.mustNotifyBehaviorEnd)
            {
                //this.CardBehaviorOwner.OnBehaviorEnd(this);
                this.mustNotifyBehaviorEnd = false;

                if (this.IsActive)
                {
                    this.State = ResurrectState.PICK_CARD;
                }
            }

            return this.IsActive;
        }

        public override void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            base.HandleGameEvents(gameEvents);

            switch (this.state)
            {
                case ResurrectState.PICK_CARD:
                    this.HandlePickState(gameEvents);
                    break;
                case ResurrectState.SOCKET_CARD:
                    this.HandleSocketState(gameEvents);
                    break;
            }
        }

        private void HandlePickState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner);

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
                    if (gameEvent.Layer == currentPlayerLayer)
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

                    currentPlayerLayer.CardEntityFocused = cardEntity;
                }
            }

            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == currentPlayerLayer && pElem.Entity != null);

                if (boardGameEvent != null)
                {
                    CardEntity cardToRemove = boardGameEvent.Entity as CardEntity;

                    this.FromCardEntities.Remove(cardToRemove);

                    this.NodeLevel.PickCard(currentPlayerLayer, cardToRemove);

                    this.CardBehaviorOwner.OnBehaviorCardPicked(this, this.NodeLevel.BoardGameLayer.CardEntityPicked);

                    this.State = ResurrectState.SOCKET_CARD;
                }
            }
        }

        private void HandleSocketState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner);

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
                GameEvent nullPickEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == currentPlayerLayer && pElem.Entity == null);
                GameEvent entityPickEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == currentPlayerLayer && pElem.Entity != null);

                if(nullPickEvent != null)
                {
                    CardEntity cardToAdd = NodeLevel.UnpickCard(currentPlayerLayer, nullPickEvent.Details);

                    this.FromCardEntities.Add(cardToAdd);

                    this.State = ResurrectState.PICK_CARD;
                }

                if (entityPickEvent != null)
                {
                    CardEntity cardToRemove = entityPickEvent.Entity as CardEntity;

                    this.FromCardEntities.Remove(cardToRemove);

                    this.NodeLevel.PickCard(currentPlayerLayer, cardToRemove);

                    this.CardBehaviorOwner.OnBehaviorCardPicked(this, this.NodeLevel.BoardGameLayer.CardEntityPicked);

                    this.State = ResurrectState.SOCKET_CARD;
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

            this.NodeLevel.BoardGameLayer.SocketCard(starEntity);
        }

        public enum ResurrectState
        {
            VOID,
            PICK_CARD,
            SOCKET_CARD
        }
    }
}
