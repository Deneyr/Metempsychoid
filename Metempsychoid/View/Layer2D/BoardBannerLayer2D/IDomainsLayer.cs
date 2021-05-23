using Metempsychoid.Model.Layer.BoardGameLayer;
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
    }
}
