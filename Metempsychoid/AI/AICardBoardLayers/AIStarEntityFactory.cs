using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;

namespace Astrategia.AI.AICardBoardLayers
{
    public class AIStarEntityFactory: AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            return this.CreateObjectAI(worldAI, null, obj);
        }

        public override IAIObject CreateObjectAI(AIWorld worldAI, AAILayer layerAI, IObject obj)
        {
            if (obj is StarEntity)
            {
                StarEntity entity = obj as StarEntity;

                return new AIStarEntity(layerAI, this, entity);
            }

            return null;
        }
    }
}
