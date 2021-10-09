using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Layer.MenuTextLayer;

namespace Metempsychoid.View.Layer2D.MenuTextLayer2D
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
