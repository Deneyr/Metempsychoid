using Astrategia.View.Text2D;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    public class TitleBannerTextToken2D : TextToken2D
    {
        public TitleBannerTextToken2D(string text, Color fillColor) 
            : base(text, fillColor, false)
        {
            this.text2D.Font = AObject2DFactory.GetFontByName("Protector");

            this.text2D.FillColor = fillColor;
            this.text2D.OutlineThickness = 2;
            this.text2D.OutlineColor = Color.Black;

            this.UpdateCanevas();
        }

        public override TextToken2D CloneToken()
        {
            return new TitleBannerTextToken2D(this.FullText, this.SpriteColor);
        }
    }
}
