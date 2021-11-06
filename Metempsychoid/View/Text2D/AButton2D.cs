using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.View.Controls;
using SFML.Graphics;
using SFML.System;

namespace Astrategia.View.Text2D
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

        public bool IsPointHit(ALayer2D parentLayer, Vector2i position)
        {
            return this.HitZone.Contains(position.X, position.Y);
        }

        public abstract bool OnMousePressed(ALayer2D parentLayer, ControlEventType eventType);

        public virtual bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType)
        {
            return false;
        }

        public virtual bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            this.PlaySound("buttonClicked");

            return false;
        }

        public virtual void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            this.PlaySound("buttonFocused");
        }

        public abstract void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType);
    }
}
