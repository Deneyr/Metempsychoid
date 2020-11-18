using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using SFML.System;

namespace Metempsychoid.View.Layer2D.EntityLayer2D
{
    public class EntityLayer2DFactory : AObject2DFactory
    {
        public Vector2i Area
        {
            get;
            private set;
        }

        public EntityLayer2DFactory()
        {
            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is EntityLayer)
            {
                EntityLayer entityLayer2D = obj as EntityLayer;

                return new EntityLayer2D(world2D, this, entityLayer2D);
            }

            return null;
        }
    }
}