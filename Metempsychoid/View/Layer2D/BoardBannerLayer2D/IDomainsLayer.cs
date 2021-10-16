﻿using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.View.Layer2D.BoardGameLayer2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public interface IDomainsLayer
    {
        event Action StartDomainEvaluated;
        event Action<CJStarDomain> DomainEvaluated;
        event Action EndDomainEvaluated;

        event Action<IDomainsLayer> DomainFocusedChanged;

        CJStarDomain2D DomainFocused
        {
            get;
        }
    }
}
