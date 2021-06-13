using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Node.TestWorld;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class CardMoveNotifBehavior : ACardNotifBehavior
    {
        private MoveState state;

        protected bool mustNotifyBehaviorEnd;

        public MoveState State
        {
            get
            {
                return this.state;
            }

            private set
            {
                if(this.state != value)
                {
                    this.state = value;

                    switch (this.state)
                    {
                        case MoveState.PICK_CARD:
                            this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
                            break;
                        case MoveState.SOCKET_CARD:
                            this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
                            break;
                    }
                }
            }
        }

        public CardMoveNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity)//List<StarEntity> fromStarEntities, List<StarEntity> toStarEntities) :
            : base(cardBehaviorOwner, ownerCardEntity)
        {
            this.FromStarEntities = new List<StarEntity>();

            this.ToStarEntities = new List<StarEntity>();

            this.state = MoveState.VOID;
        }

        public override void EndNotif(World world)
        {
            base.EndNotif(world);

            this.FromStarEntities.Clear();
            this.ToStarEntities.Clear();

            this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override void StartNotif(World world)
        {
            base.StartNotif(world);

            this.mustNotifyBehaviorEnd = false;

            this.CardBehaviorOwner.OnBehaviorStart(this);

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
            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == NodeLevel.BoardGameLayer);

                if (boardGameEvent != null)
                {
                    NodeLevel.BoardGameLayer.PickCard(boardGameEvent.Entity as CardEntity);

                    if (NodeLevel.BoardGameLayer.CardEntityPicked != null)
                    {
                        this.CardBehaviorOwner.OnBehaviorCardPicked(this);

                        this.State = MoveState.SOCKET_CARD;
                    }
                }
            }
        }

        private void HandleSocketState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
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

        protected virtual void ExecuteBehavior(StarEntity starEntity)
        {
            this.NodeLevel.BoardGameLayer.MoveCard(starEntity);
        }

        public enum MoveState
        {
            VOID,
            PICK_CARD,
            SOCKET_CARD
        }
    }
}
