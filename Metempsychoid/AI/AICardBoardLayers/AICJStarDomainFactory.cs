using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI.AICardBoardLayers
{
    public class AICJStarDomainFactory : AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            return this.CreateObjectAI(worldAI, null, obj);
        }

        public override IAIObject CreateObjectAI(AIWorld worldAI, AAILayer layerAI, IObject obj)
        {
            if (obj is CJStarDomain)
            {
                CJStarDomain entity = obj as CJStarDomain;

                return new AICJStarDomain(layerAI, this, entity);
            }

            return null;
        }
    }
}