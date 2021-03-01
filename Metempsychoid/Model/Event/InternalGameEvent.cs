using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Event
{
    public class InternalGameEvent
    {
        public InternalEventType Type
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public InternalGameEvent(InternalEventType type, string details)
        {
            this.Type = type;

            this.Details = details;
        }

    }

    public enum InternalEventType
    {
        GO_TO_WORLD,
        GO_TO_LEVEL,
        GO_TO_LEVEL_PART
    }
}
