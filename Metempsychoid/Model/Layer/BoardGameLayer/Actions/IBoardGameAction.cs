﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer.Actions
{
    public interface IBoardGameAction
    {
        void ExecuteAction(BoardGameLayer layerToPerform);

        bool IsStillValid(BoardGameLayer layerToPerform);
    }
}
