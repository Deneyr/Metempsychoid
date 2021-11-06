using Astrategia.Model.Card;
using Astrategia.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public class SendInternalEventNotifBehavior : ABoardNotifBehavior
    {
        public InternalEventType EventTypeToSend
        {
            get;
            private set;
        }

        public string Details
        {
            get;
            private set;
        }

        public SendInternalEventNotifBehavior(CardEntity ownerCardEntity, InternalEventType eventTypeToSend, string details) : base(ownerCardEntity)
        {
            this.EventTypeToSend = eventTypeToSend;
            this.Details = details;
        }

        public override void EndNotif(World world)
        {

        }

        public override void StartNotif(World world)
        {
            world.NotifyInternalGameEvent(new InternalGameEvent(this.EventTypeToSend, this.Details));
        }

        public override bool UpdateNotif(World world)
        {
            return false;
        }
    }
}