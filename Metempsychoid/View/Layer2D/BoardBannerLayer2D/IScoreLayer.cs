using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    public interface IScoreLayer
    {
        string PlayerName
        {
            get;
        }

        int PlayerScore
        {
            get;
            set;
        }
    }
}
