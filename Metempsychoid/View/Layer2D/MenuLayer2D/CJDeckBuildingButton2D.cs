using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.View.Controls;

namespace Astrategia.View.Layer2D.MenuLayer2D
{
    public class CJDeckBuildingButton2D : ACJButton2D
    {
        public CJDeckBuildingButton2D(ALayer2D parentLayer, string idLabel) : base(parentLayer, 300, idLabel)
        {
        }

        public override bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType)
        {
            return false;
        }

        public override bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (parentLayer.FocusedGraphicEntity2D == this)
            {

            }
            this.Zoom = 1f;

            return false;
        }
    }
}
