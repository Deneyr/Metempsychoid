using Astrategia.Model;
using Astrategia.Model.Layer.BoardPlayerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardPlayerLayer2D
{
    public class OppBoardPlayerLayer2DFactory: BoardPlayerLayer2DFactory
    {
        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is OppBoardPlayerLayer)
            {
                OppBoardPlayerLayer oppBoardPlayerLayer = obj as OppBoardPlayerLayer;

                return new OppBoardPlayerLayer2D(world2D, this, oppBoardPlayerLayer);
            }

            return null;
        }
    }
}
