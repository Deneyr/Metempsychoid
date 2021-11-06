using Astrategia.Model.Card;
using Astrategia.Model.Event;
using Astrategia.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public abstract class ABoardNotifBehavior : IBoardNotifBehavior
    {
        public CardEntity OwnerCardEntity
        {
            get;
            private set;
        }

        public CardBoardLevel NodeLevel
        {
            get;
            set;
        }

        public virtual bool IsActive
        {
            get;
            protected set;
        }

        public virtual bool IsThereEndButton
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsThereBehaviorLabel
        {
            get
            {
                return false;
            }
        }

        public ABoardNotifBehavior(CardEntity ownerCardEntity)
        {
            this.OwnerCardEntity = ownerCardEntity;

            this.IsActive = true;
        }

        public abstract void EndNotif(World world);

        public abstract void StartNotif(World world);

        public abstract bool UpdateNotif(World world);

        public virtual void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            if(gameEvents.TryGetValue(EventType.NEXT_BEHAVIOR, out List<GameEvent> gameEventsNextBehavior) && gameEventsNextBehavior.Any())
            {
                this.IsActive = false;
            }
        }
    }
}
