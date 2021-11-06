using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardPlayerLayer
{
    public class OppBoardPlayerLayer: BoardPlayerLayer
    {
        public override void OnStartTurn()
        {
            this.IsActiveTurn = true;
        }

        public override void OnEndTurn()
        {
            this.IsActiveTurn = false;
        }
    }
}
