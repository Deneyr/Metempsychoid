using Astrategia.Animation;
using Astrategia.View.Animation;
using Astrategia.View.Controls;
using Astrategia.View.Layer2D.MenuLayer2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    public class ReturnMenuButton2D : ACJButton2D
    {
        public ReturnMenuButton2D(ALayer2D parentLayer) : base(parentLayer, 300, "end_game_menu_button")
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

                (parentLayer as BoardBannerLayer2D).ChangeLevel("StartPageLevel");
            }

            this.Zoom = 1f;

            return false;
        }
    }
}