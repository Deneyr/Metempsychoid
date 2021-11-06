using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardBannerLayer;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    public class BoardBannerLayer2DFactory : AObject2DFactory
    {
        public BoardBannerLayer2DFactory()
        {
            //this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is BoardBannerLayer)
            {
                BoardBannerLayer backgroundLayer = obj as BoardBannerLayer;

                return new BoardBannerLayer2D(world2D, this, backgroundLayer);
            }

            return null;
        }
    }
}
