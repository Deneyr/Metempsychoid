using Astrategia.Model;
using Astrategia.Model.Layer.BoardPlayerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardPlayerLayer2D
{
    public class MenuBoardPlayerLayer2DFactory: BoardPlayerLayer2DFactory
    {
        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is MenuBoardPlayerLayer)
            {
                MenuBoardPlayerLayer boardPlayerLayer = obj as MenuBoardPlayerLayer;

                return new MenuBoardPlayerLayer2D(world2D, this, boardPlayerLayer);
            }

            return null;
        }
    }
}
