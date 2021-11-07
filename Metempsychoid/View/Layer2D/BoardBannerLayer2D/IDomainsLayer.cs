using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.View.Layer2D.BoardGameLayer2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    public interface IDomainsLayer
    {
        event Action StartDomainEvaluated;
        event Action<CJStarDomain> DomainEvaluated;
        event Action EndDomainEvaluated;

        event Action<IDomainsLayer> DomainFocusedChanged;

        event Action<int> NbCardsToPlaceChanged;

        CJStarDomain2D DomainFocused
        {
            get;
        }
    }
}
