using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class BoardPlayerLayer2DFactory : AObject2DFactory
    {
        public BoardPlayerLayer2DFactory()
        {
            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is BoardPlayerLayer)
            {
                BoardPlayerLayer boardPlayerLayer = obj as BoardPlayerLayer;

                return new BoardPlayerLayer2D(world2D, this, boardPlayerLayer);
            }

            return null;
        }
    }
}
