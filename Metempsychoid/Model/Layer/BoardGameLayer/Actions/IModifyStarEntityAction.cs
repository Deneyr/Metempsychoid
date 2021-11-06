﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer.Actions
{
    public interface IModifyStarEntityAction : IBoardGameAction
    {
        StarEntity OwnerStar
        {
            get;
        }
    }
}
