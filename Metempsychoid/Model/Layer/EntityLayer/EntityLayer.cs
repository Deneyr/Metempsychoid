using Astrategia.Animation;
using Astrategia.Model.Animation;
using Astrategia.Model.Node;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.EntityLayer
{
    public class EntityLayer: ALayer
    {
        public EntityLayer()
        {

        }

        public override void UpdateLogic(World world, Time deltaTime)
        {
            base.UpdateLogic(world, deltaTime);           
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            
        }
    }
}
