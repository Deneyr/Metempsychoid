using Astrategia.Model;
using Astrategia.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI.AICard
{
    public class AICardEntityFactory : AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            return this.CreateObjectAI(worldAI, null, obj);
        }

        public override IAIObject CreateObjectAI(AIWorld worldAI, AAILayer layerAI, IObject obj)
        {
            if (obj is CardEntity)
            {
                CardEntity entity = obj as CardEntity;

                return new AICardEntity(layerAI, this, entity);
            }

            return null;
        }
    }
}