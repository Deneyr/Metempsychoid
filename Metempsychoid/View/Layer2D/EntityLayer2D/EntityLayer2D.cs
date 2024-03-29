﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using SFML.System;

namespace Astrategia.View.Layer2D.EntityLayer2D
{
    public class EntityLayer2D : ALayer2D
    {
        public EntityLayer2D(World2D world2D, IObject2DFactory factory, ALayer layer) : 
            base(world2D, factory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);
        }

        public override Vector2f Position
        {
            set
            {
                base.Position = value * 0.5f;
            }
        }
    }
}
