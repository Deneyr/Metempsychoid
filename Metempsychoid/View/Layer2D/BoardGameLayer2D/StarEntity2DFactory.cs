﻿using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class StarEntity2DFactory : AObject2DFactory
    {
        public StarEntity2DFactory()
        {
            this.texturesPath.Add(@"Assets\Graphics\Entities\Star.png");
            this.texturesPath.Add(@"Assets\Graphics\Shaders\distortion_map.png");

            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is StarEntity)
            {
                StarEntity entity = obj as StarEntity;

                return new StarEntity2D(this, layer2D, entity);
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
