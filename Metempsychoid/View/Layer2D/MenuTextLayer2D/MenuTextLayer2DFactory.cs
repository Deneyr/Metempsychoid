using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.MenuTextLayer;

namespace Astrategia.View.Layer2D.MenuTextLayer2D
{
    public class MenuTextLayer2DFactory : AObject2DFactory
    {
        public MenuTextLayer2DFactory()
        {
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is MenuTextLayer)
            {
                MenuTextLayer menuTextLayer = obj as MenuTextLayer;

                return new MenuTextLayer2D(world2D, this, menuTextLayer);
            }

            return null;
        }
    }
}
