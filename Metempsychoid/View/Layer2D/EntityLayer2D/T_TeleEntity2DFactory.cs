using Metempsychoid.Model;
using Metempsychoid.Model.Layer.EntityLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.EntityLayer2D
{
    class T_TeleEntity2DFactory : AObject2DFactory
    {
        public T_TeleEntity2DFactory()
        {
            this.texturesPath.Add(@"D:\Projects\Metempsychoid\Assets\Graphics\Entities\TV[86x76].png");

            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is T_TeleEntity)
            {
                T_TeleEntity entity = obj as T_TeleEntity;

                return new T_TeleEntity2D(this, layer2D, entity);
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
