using Astrategia.Model;
using Astrategia.Model.Layer.BoardNotifLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardNotifLayer2D
{
    class CardEntityAwakenedDecorator2DFactory : CardEntityDecorator2DFactory
    {
        public CardEntityAwakenedDecorator2DFactory() :
            base()
        {
            this.AddTexturePath("beamEffectTexture", @"Assets\Graphics\Cards\Effects\beamEffect.png");
            this.AddTexturePath("starEffectTexture", @"Assets\Graphics\Cards\Effects\starEffect.png");
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is CardEntityAwakenedDecorator)
            {
                CardEntityAwakenedDecorator entity = obj as CardEntityAwakenedDecorator;

                return new CardEntityAwakenedDecorator2D(this, layer2D, entity);
            }

            return null;
        }
    }
}