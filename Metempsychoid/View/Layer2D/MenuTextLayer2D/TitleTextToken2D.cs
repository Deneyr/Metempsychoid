using Metempsychoid.View.Text2D;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.MenuTextLayer2D
{
    public class TitleTextToken2D : TextToken2D
    {
        public TitleTextToken2D(string text, Color fillColor)
            : base(text, fillColor, false)
        {
            this.text2D.Font = AObject2DFactory.GetFontByName("dumbTitle");

            this.text2D.FillColor = fillColor;
            this.text2D.OutlineThickness = 20;
            this.text2D.OutlineColor = Color.Black;

            this.UpdateCanevas();
        }

        public override TextToken2D CloneToken()
        {
            return new TitleTextToken2D(this.FullText, this.SpriteColor);
        }
    }
}