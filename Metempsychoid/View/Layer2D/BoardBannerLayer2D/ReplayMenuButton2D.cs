using Metempsychoid.View.Controls;
using Metempsychoid.View.Layer2D.MenuLayer2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class ReplayMenuButton2D : ACJButton2D
    {
        public ReplayMenuButton2D(ALayer2D parentLayer) : base(parentLayer, 300, "end_game_restart_button")
        {
            this.IsActive = false;
        }

        public override bool IsFocusable(ALayer2D parentLayer)
        {
            return this.IsActive;
        }

        public override void DeactiveButton()
        {
            this.IsActive = false;
        }

        public override bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            if (parentLayer.FocusedGraphicEntity2D == this)
            {
                base.OnMouseReleased(parentLayer, eventType);

                (parentLayer as BoardBannerLayer2D).ChangeLevel("CardBoardLevel");
            }

            this.Zoom = 1f;

            return false;
        }
    }
}