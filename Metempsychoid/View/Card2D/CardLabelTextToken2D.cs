using Metempsychoid.View.Text2D;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardLabelTextToken2D : TextToken2D
    {
        public CardLabelTextToken2D(string text, Color fillColor) 
            : base(text, fillColor)
        {
        }

        public override Color SpriteColor
        {
            set
            {
                this.text2D.FillColor = new Color(value.R, value.G, value.B, value.A);
                this.text2D.OutlineColor = new Color(this.text2D.OutlineColor.R, this.text2D.OutlineColor.G, this.text2D.OutlineColor.B, value.A);
            }
        }

        public override TextToken2D CloneToken()
        {
            return new CardLabelTextToken2D(this.FullText, this.SpriteColor);
        }
    }
}
