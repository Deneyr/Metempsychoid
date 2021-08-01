using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.View.Controls;

namespace Metempsychoid.View.Layer2D.MenuLayer2D
{
    public class CJStartButton2D : ACJButton2D
    {
        public CJStartButton2D(ALayer2D parentLayer) : base(parentLayer, 300, "start_game")
        {
        }

        //public override bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType)
        //{
        //    return base.OnMouseClicked(;
        //}

        public override bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (parentLayer.FocusedGraphicEntity2D == this)
            {
                base.OnMouseReleased(parentLayer, eventType);

                parentLayer.SendEventToWorld(Model.Event.EventType.LEVEL_CHANGE, null, "TestLevel");
            }

            this.Zoom = 1f;

            return false;
        }
    }
}
