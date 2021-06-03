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

        public ALayer Layer
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public GameEvent(EventType type, ALayer Layer, AEntity entity, string details)
        {
            this.Type = type;

            this.Details = details;

            this.Layer = Layer;

            this.Entity = entity;
        }

    }

    public enum EventType
    {
        LEVEL_CHANGE,

        //CANEVAS_CHANGED,

        LEVEL_PHASE_CHANGE,
        DRAW_CARD,
        FOCUS_CARD_HAND,
        FOCUS_CARD_BOARD,
        PICK_CARD,
        SOCKET_CARD,
        MOVE_CARD_OVERBOARD
    }
}
