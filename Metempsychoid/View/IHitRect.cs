using Astrategia.View.Controls;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View
{
    public interface IHitRect
    {
        IntRect HitZone
        {
            get;
        }

        bool IsFocusable(ALayer2D parentLayer);

        bool IsPointHit(ALayer2D parentLayer, Vector2i position);

        bool OnMousePressed(ALayer2D parentLayer, ControlEventType eventType);

        bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType);

        bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType);

        void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType);

        void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType);
    }
}
