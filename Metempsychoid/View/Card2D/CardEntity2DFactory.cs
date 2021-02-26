using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardEntity2DFactory : AObject2DFactory
    {
        public CardEntity2DFactory()
        {
            this.texturesPath.Add(@"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\distortion_map.png");

            this.texturesPath.Add(@"D:\Projects\Metempsychoid\Assets\Graphics\Cards\backCard.jpg");

            this.texturesPath.Add(@"D:\Projects\Metempsychoid\Assets\Graphics\Cards\wheelCard.jpg");

            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is CardEntity)
            {
                CardEntity entity = obj as CardEntity;

                return new CardEntity2D(this, entity);
            }

            return null;
        }

        public override void OnTextureLoaded(string path, Texture texture)
        {
            if (this.Resources.ContainsKey(path))
            {
                texture.Smooth = true;

                this.Resources[path] = texture;
            }
        }
    }
}
