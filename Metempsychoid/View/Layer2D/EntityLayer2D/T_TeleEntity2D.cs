using Metempsychoid.Model.Layer.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.EntityLayer2D
{
    public class T_TeleEntity2D : AEntity2D
    {
        public T_TeleEntity2D(IObject2DFactory factory, T_TeleEntity entity)
        {
            this.Position = entity.Position;
            this.Rotation = entity.Rotation;

            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);

            this.ObjectSprite.TextureRect = new SFML.Graphics.IntRect(0, 0, 86, 76);
        }
    }
}
