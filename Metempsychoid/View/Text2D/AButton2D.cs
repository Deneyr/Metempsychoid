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
    public abstract class AButton2D : TextParagraph2D, IHitRect
    {
        public AButton2D(TextCanevas2D textCanevas2D, Vector2f positionOffsetTopLeft, Vector2f positionOffsetBotRight, Alignment alignment, uint characterSize) 
            : base(textCanevas2D, positionOffsetTopLeft, positionOffsetBotRight, alignment, characterSize)
        {
        }

        public abstract IntRect HitZone
        {
            get;
        }

        public abstract void OnMousePressed(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType);

        public abstract void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType);
    }
}
