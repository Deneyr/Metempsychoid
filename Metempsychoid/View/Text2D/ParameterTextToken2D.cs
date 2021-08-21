using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Text2D
{
    public class ParameterTextToken2D : TextToken2D
    {
        public string TokenType
        {
            get;
            private set;
        }

        public int NbHandlingToken
        {
            get;
            set;
        }

        public ParameterTextToken2D(string tokenType, Color fillColor, int nbHandlingToken = 0) : base(string.Empty, fillColor)
        {
            this.TokenType = tokenType;
            this.NbHandlingToken = nbHandlingToken;
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            
        }

        public override TextToken2D CloneToken()
        {
            return new ParameterTextToken2D(this.TokenType, this.SpriteColor, this.NbHandlingToken);
        }

        public List<TextToken2D> CreateTextToken2DFrom(string tokenText)
        {
            List<TextToken2D> textToken2Ds = TextParagraphFactory.CreateTextTokens(this.TokenType, tokenText, this.SpriteColor);

            foreach (TextToken2D textToken in textToken2Ds)
            {
                textToken.CharacterSize = this.CharacterSize;
                textToken.CustomZoom = this.CustomZoom;
            }

            this.NbHandlingToken = textToken2Ds.Count;

            return textToken2Ds;
        }
    }
}
