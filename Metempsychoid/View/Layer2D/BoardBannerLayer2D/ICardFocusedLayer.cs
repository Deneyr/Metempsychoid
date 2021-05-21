﻿using Metempsychoid.View.Card2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public interface ICardFocusedLayer
    {
        event Action<ICardFocusedLayer> CardFocusedChanged;

        CardEntity2D CardFocused
        {
            get;
        }

    }
}
