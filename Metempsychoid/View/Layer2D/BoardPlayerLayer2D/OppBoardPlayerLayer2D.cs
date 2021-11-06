using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardPlayerLayer;

namespace Astrategia.View.Layer2D.BoardPlayerLayer2D
{
    public class OppBoardPlayerLayer2D : BoardPlayerLayer2D
    {
        public OppBoardPlayerLayer2D(World2D world2D, IObject2DFactory factory, BoardPlayerLayer layer) 
            : base(world2D, factory, layer)
        {
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details, bool mustForwardEvent)
        {
            //switch (this.levelTurnPhase)
            //{
            //    case TurnPhase.MAIN:

            //        if (eventType == ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed"
            //            || eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "click")
            //        {
            //            this.SendUnpickEvent();
            //        }

            //        mustForwardEvent = base.OnControlActivated(eventType, details, mustForwardEvent);

            //        break;
            //}

            //return true;
            return mustForwardEvent;
        }

        internal override void SendEventToWorld(Model.Event.EventType eventType, AEntity entityConcerned, string details)
        {
            if (eventType == Model.Event.EventType.DRAW_CARD
                || eventType == Model.Event.EventType.FOCUS_CARD_PILE
                || eventType == Model.Event.EventType.LEVEL_PHASE_CHANGE)
            {
                base.SendEventToWorld(eventType, entityConcerned, details);
            }
        }
    }
}
