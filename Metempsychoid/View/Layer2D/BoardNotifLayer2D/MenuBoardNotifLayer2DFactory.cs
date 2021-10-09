using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class MenuBoardNotifLayer2DFactory: BoardNotifLayer2DFactory
    {
        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is MenuBoardNotifLayer)
            {
                MenuBoardNotifLayer boardNotifLayer = obj as MenuBoardNotifLayer;

                return new MenuBoardNotifLayer2D(world2D, this, boardNotifLayer);
            }

            return null;
        }
    }
}
