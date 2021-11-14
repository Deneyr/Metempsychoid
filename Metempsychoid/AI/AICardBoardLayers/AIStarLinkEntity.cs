using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;

namespace Astrategia.AI.AICardBoardLayers
{
    public class AIStarLinkEntity : AAIEntity
    {
        public AIStarEntity StarFrom
        {
            get;
            protected set;
        }

        public AIStarEntity StarTo
        {
            get;
            protected set;
        }

        public AIStarLinkEntity(AAILayer parentLayer, IAIObjectFactory factory, AEntity entity) 
            : base(parentLayer, factory, entity)
        {
        }

        public void UpdateReference()
        {
            if(this.parentLayer.TryGetTarget(out AAILayer parentLayer))
            {
                StarLinkEntity starLinkEntity = parentLayer.GetEntityFromAIEntity(this) as StarLinkEntity;

                this.StarFrom = parentLayer.GetAIEntityFromEntity(starLinkEntity.StarFrom) as AIStarEntity;
                this.StarTo = parentLayer.GetAIEntityFromEntity(starLinkEntity.StarTo) as AIStarEntity;
            }
        }

        public override void UpdateAI(Time deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
