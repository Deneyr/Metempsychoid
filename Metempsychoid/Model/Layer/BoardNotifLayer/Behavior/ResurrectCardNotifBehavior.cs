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
        private MoveState state;

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

        public MoveState State
        {
            get
            {
                return this.state;
            }

            private set
            {
                if (this.state != value)
                {
                    this.state = value;

                    switch (this.state)
                    {
                        case MoveState.PICK_CARD:
                            this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player).SetBehaviorSourceCardEntities(this.FromCardEntities);
                            break;
                        case MoveState.SOCKET_CARD:
                            //this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
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

            this.state = MoveState.VOID;
        }

        public override void EndNotif(World world)
        {
            base.EndNotif(world);

            //this.FromStarEntities.Clear();
            this.FromCardEntities.Clear();
            this.ToStarEntities.Clear();

            BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player);

            currentPlayerLayer.SetBehaviorSourceCardEntities(this.FromCardEntities);
            currentPlayerLayer.CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.HAND;

            //this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override void StartNotif(World world)
        {
            base.StartNotif(world);

            this.mustNotifyBehaviorEnd = false;

            this.CardBehaviorOwner.OnBehaviorStart(this);

            this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player).CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.CEMETERY;

            this.State = MoveState.PICK_CARD;
        }

        public override bool UpdateNotif(World world)
        {
            if (this.mustNotifyBehaviorEnd)
            {
                this.CardBehaviorOwner.OnBehaviorEnd(this);
                this.mustNotifyBehaviorEnd = false;

                if (this.IsActive)
                {
                    this.State = MoveState.PICK_CARD;
                }
            }

            return this.IsActive;
        }

        public override void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            base.HandleGameEvents(gameEvents);

            switch (this.state)
            {
                case MoveState.PICK_CARD:
                    this.HandlePickState(gameEvents);
                    break;
                case MoveState.SOCKET_CARD:
                    this.HandleSocketState(gameEvents);
                    break;
            }
        }

        private void HandlePickState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            if (gameEvents.TryGetValue(EventType.FOCUS_CARD_HAND, out List<GameEvent> gameEventsFocused))
            {
                BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.Player);
                GameEvent boardGameEvent = gameEventsFocused.FirstOrDefault(pElem => pElem.Layer == currentPlayerLayer);

                if (boardGameEvent != null)
                {
                    this.NodeLevel.BoardGameLayer.CardEntityFocused = null;

                    currentPlayerLayer.CardEntityFocused = boardGameEvent.Entity as CardEntity;
                }
            }

            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == NodeLevel.BoardGameLayer);

                if (boardGameEvent != null)
                {
                    NodeLevel.BoardGameLayer.PickCard(boardGameEvent.Entity as CardEntity);

                    if (NodeLevel.BoardGameLayer.CardEntityPicked != null)
                    {
                        this.CardBehaviorOwner.OnBehaviorCardPicked(this, NodeLevel.BoardGameLayer.CardEntityPicked);

                        this.State = MoveState.SOCKET_CARD;
                    }
                }
            }
        }

        private void HandleSocketState(Dictionary<EventType, List<GameEvent>> gameEvents)
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

            if (gameEvents.TryGetValue(EventType.MOVE_CARD_OVERBOARD, out List<GameEvent> gameEventsOverboard))
            {
                GameEvent boardGameEvent = gameEventsOverboard.FirstOrDefault(pElem => pElem.Layer == NodeLevel.BoardGameLayer);

                if (boardGameEvent != null)
                {
                    this.NodeLevel.MoveCardOverBoard(boardGameEvent.Details, (boardGameEvent.Entity as CardEntity));
                }
            }

            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == NodeLevel.BoardGameLayer);

                if (boardGameEvent == null)
                {
                    boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer is BoardPlayerLayer.BoardPlayerLayer);

                    if (boardGameEvent != null && boardGameEvent.Entity == null)
                    {
                        this.NodeLevel.UnpickCard(null, boardGameEvent.Details);

                        if (NodeLevel.BoardGameLayer.CardEntityPicked == null)
                        {
                            this.State = MoveState.PICK_CARD;
                        }
                    }
                }
            }
        }

        public override bool CanSocketCardOn(StarEntity starEntity, CardEntity cardToSocket)
        {
            return this.ToStarEntities.Contains(starEntity);
        }

        protected virtual void ExecuteBehavior(StarEntity starEntity)
        {
            //this.NodeLevel.BoardGameLayer.MoveCard(starEntity);
        }

        public enum MoveState
        {
            VOID,
            PICK_CARD,
            SOCKET_CARD
        }
    }
}
