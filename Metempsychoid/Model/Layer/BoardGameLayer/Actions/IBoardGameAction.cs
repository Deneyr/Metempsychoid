﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public interface IBoardGameAction
    {
        void ExecuteAction(BoardGameLayer layerToPerform);
    }
}
