using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Event
{
    public class GameEvent
    {
        public EventType Type{
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public GameEvent(EventType type, string details)
        {
            this.Type = type;

            this.Details = details;
        }

    }

    public enum EventType
    {
        START,
        ENDING,
        PICK_WORD,
        DROP_WORD,
        INSERT_WORD,
        OPEN_BUBBLE,
        CLOSE_BUBBLE,
        START_TALK,
        END_TALK,
        DOOR_KNOCK,
        DOOR_OPEN,
        VALIDATION,
        END_TIMER,
        SPEED_UP_DIALOGUE
    }
}
