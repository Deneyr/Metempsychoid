using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public abstract class ABoardNotifBehavior : IBoardNotifBehavior
    {
        public CardEntity OwnerCardEntity
        {
            get;
            private set;
        }

        public TestLevel NodeLevel
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
