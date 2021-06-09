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
    public class CardMovedNotifBehavior : ABoardNotifBehavior
    {
        private MoveState state;

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
                            this.level.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
                            break;
                        case MoveState.SOCKET_CARD:
                            this.level.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
                            break;
                    }
                }
            }
        }

        public List<StarEntity> FromStarEntities
        {
            get;
            private set;
        }

        public List<StarEntity> ToStarEntities
        {
            get;
            private set;
        }

        public CardMovedNotifBehavior(TestLevel level, List<StarEntity> fromStarEntities, List<StarEntity> toStarEntities) :
            base(level)
        {
            this.FromStarEntities = fromStarEntities;

            this.ToStarEntities = toStarEntities;

            this.state = MoveState.VOID;
        }

        public override void EndNotif(World world)
        {

        }

        public override void StartNotif(World world)
        {
            this.State = MoveState.PICK_CARD;
        }

        public override bool UpdateNotif(World world)
        {
            if (this.IsActive)
            {

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
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == level.BoardGameLayer);

                if (boardGameEvent != null)
                {
                    level.BoardGameLayer.PickCard(boardGameEvent.Entity as CardEntity);

                    if (level.BoardGameLayer.CardEntityPicked != null)
                    {
                        this.State = MoveState.SOCKET_CARD;
                    }
                }
            }
        }

        private void HandleSocketState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            if (gameEvents.TryGetValue(EventType.MOVE_CARD_OVERBOARD, out List<GameEvent> gameEventsOverboard))
            {
                GameEvent boardGameEvent = gameEventsOverboard.FirstOrDefault(pElem => pElem.Layer == level.BoardGameLayer);

                if (boardGameEvent != null)
                {
                    this.level.MoveCardOverBoard(boardGameEvent.Details, (boardGameEvent.Entity as CardEntity));
                }
            }

            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == level.BoardGameLayer);

                if (boardGameEvent == null)
                {
                    boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer is BoardPlayerLayer.BoardPlayerLayer);

                    if (boardGameEvent != null && boardGameEvent.Entity == null)
                    {
                        this.level.UnpickCard(null, boardGameEvent.Details);

                        if (level.BoardGameLayer.CardEntityPicked == null)
                        {
                            this.State = MoveState.PICK_CARD;
                        }
                    }
                }
            }
        }

        public enum MoveState
        {
            VOID,
            PICK_CARD,
            SOCKET_CARD
        }
    }
}
