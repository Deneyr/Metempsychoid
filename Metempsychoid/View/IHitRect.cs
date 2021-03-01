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

        void OnMousePressed(ControlEventType eventType);

        void OnMouseReleased(ControlEventType eventType);
    }
}
