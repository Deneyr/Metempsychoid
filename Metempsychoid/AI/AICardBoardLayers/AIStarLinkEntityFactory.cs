using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI.AICardBoardLayers
{
    public class AIStarLinkEntityFactory: AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            return this.CreateObjectAI(worldAI, null, obj);
        }

        public override IAIObject CreateObjectAI(AIWorld worldAI, AAILayer layerAI, IObject obj)
        {
            if (obj is StarLinkEntity)
            {
                StarLinkEntity entity = obj as StarLinkEntity;

                return new AIStarLinkEntity(layerAI, this, entity);
            }

            return null;
        }
    }
}