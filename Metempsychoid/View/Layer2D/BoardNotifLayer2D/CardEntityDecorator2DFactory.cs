using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.View.Card2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class CardEntityDecorator2DFactory : CardEntity2DFactory
    {
        public CardEntityDecorator2DFactory():
            base()
        {
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is CardEntityDecorator)
            {
                CardEntityDecorator entity = obj as CardEntityDecorator;

                return new CardEntityDecorator2D(this, layer2D, entity);
            }

            return null;
        }
    }
}
