﻿using Metempsychoid.Model.Layer.BoardGameLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class BoardGameLayer2D : ALayer2D
    {
        public BoardGameLayer2D(World2D world2D, IObject2DFactory factory, BoardGameLayer layer):
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);
        }


        public override Vector2f Position
        {
            set
            {
                base.Position = value * 0.75f;
            }
        }
    }
}
