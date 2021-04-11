using Metempsychoid.View.Text2D;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class TitleBannerTextToken2D : TextToken2D
    {
        public TitleBannerTextToken2D(string text) : base(text)
        {
            this.text2D.Font = AObject2DFactory.GetFontByName("Protector");

            this.text2D.FillColor = Color.White;
            this.text2D.OutlineThickness = 2;
            this.text2D.OutlineColor = Color.Black;
        }
    }
}
