using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardGameLayer;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class CurvedStarLinkEntity2DFactory : AObject2DFactory
    {
        public CurvedStarLinkEntity2DFactory()
        {
            this.texturesPath.Add(@"Assets\Graphics\Shaders\linkDistorsionMap3.png");
            this.texturesPath.Add(@"Assets\Graphics\Shaders\linkDistorsionMap4.png");

            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is CurvedStarLinkEntity)
            {
                CurvedStarLinkEntity entity = obj as CurvedStarLinkEntity;

                return new CurvedStarLinkEntity2D(layer2D, this, entity);
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
