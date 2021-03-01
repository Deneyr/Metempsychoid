using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BackgroundLayer2D
{
    public class TileBackgoundObject2D: AEntity2D
    {
        public TileBackgoundObject2D(ALayer2D layer2D, Texture texture, string textureName):
            base(layer2D)
        {
            string[] token = textureName.Split(',');
            int coordX = int.Parse(token[0]);
            int coordY = int.Parse(token[1]);

            this.ObjectSprite.Texture = texture;

            this.Position = new SFML.System.Vector2f(coordX * texture.Size.X, coordY * texture.Size.Y);
        }

    }
}
