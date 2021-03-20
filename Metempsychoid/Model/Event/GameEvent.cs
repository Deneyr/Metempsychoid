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

        public AEntity Entity
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public GameEvent(EventType type, AEntity entity, string details)
        {
            this.Type = type;

            this.Details = details;

            this.Entity = entity;
        }

    }

    public enum EventType
    {
        LEVEL_PHASE_CHANGE,
        DRAW_CARD,
        FOCUS_CARD,
        PICK_CARD
    }
}
