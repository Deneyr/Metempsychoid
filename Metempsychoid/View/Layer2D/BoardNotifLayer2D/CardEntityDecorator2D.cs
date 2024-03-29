﻿using Astrategia.Model.Card;
using Astrategia.View.Card2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardNotifLayer2D
{
    public class CardEntityDecorator2D : CardEntity2D
    {
        public CardEntityDecorator2D(IObject2DFactory factory, ALayer2D layer2D, CardEntity entity) 
            : base(factory, layer2D, entity)
        {
        }
    }
}
