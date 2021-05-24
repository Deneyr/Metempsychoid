using Metempsychoid.View.Controls;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public interface IHitRect
    {
        IntRect HitZone
        {
            get;
        }

        bool IsFocusable(ALayer2D parentLayer);

        void OnMousePressed(ALayer2D parentLayer, ControlEventType eventType);

        void OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType);

        void OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType);

        void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType);

        void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType);
    }
}
