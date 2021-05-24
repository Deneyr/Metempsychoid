using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.View.Controls;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Text2D
{
    public abstract class AButton2D : TextCanevas2D, IHitRect
    {
        public AButton2D(ALayer2D parentLayer) : base(parentLayer)
        {
        }

        public IntRect HitZone
        {
            get
            {
                Vector2f canevasPosition = this.Position;
                IntRect canevas = this.Canevas;
                return new IntRect((int)canevasPosition.X, (int)canevasPosition.Y, canevas.Width, canevas.Height);
            }
        }

        public abstract bool IsFocusable(ALayer2D parentLayer);

        public abstract void OnMousePressed(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType);
    }
}
