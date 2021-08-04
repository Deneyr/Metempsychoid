using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class CardToolTip2DFactory : AObject2DFactory
    {
        public CardToolTip2DFactory()
        {
            //this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is ToolTipEntity)
            {
                ToolTipEntity entity = obj as ToolTipEntity;

                return new CardToolTip2D(layer2D);
            }

            return null;
        }
    }
}
