﻿using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardGameLayer2D
{
    public class BoardGameLayer2DFactory : AObject2DFactory
    {
        public BoardGameLayer2DFactory()
        {
            //this.InitializeFactory();

            this.idToMusicsPath.Add("cardLevelMusic", @"Assets\Musics\Magistar.ogg");
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is BoardGameLayer)
            {
                BoardGameLayer boardGameLayer = obj as BoardGameLayer;

                return new BoardGameLayer2D(world2D, this, boardGameLayer);
            }

            return null;
        }
    }
}
