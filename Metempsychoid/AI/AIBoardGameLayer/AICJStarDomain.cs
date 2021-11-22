using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;

namespace Astrategia.AI.AIBoardGameLayer
{
    public class AICJStarDomain : AAIEntity
    {
        public string DomainOwnerName
        {
            get;
            set;
        }

        public HashSet<AIStarEntity> StarEntities
        {
            get;
            protected set;
        }

        public AICJStarDomain(AAILayer parentLayer, IAIObjectFactory factory, AEntity entity) 
            : base(parentLayer, factory, entity)
        {
            CJStarDomain starDomain = entity as CJStarDomain;

            this.StarEntities = new HashSet<AIStarEntity>();

            this.DomainOwnerName = starDomain.DomainOwner != null ?  starDomain.DomainOwner.PlayerName : null;
        }

        public void UpdateReference()
        {
            if (this.parentLayer.TryGetTarget(out AAILayer parentLayer))
            {
                CJStarDomain starDomain = parentLayer.GetEntityFromAIEntity(this) as CJStarDomain;

                this.StarEntities.Clear();
                foreach(StarEntity starEntity in starDomain.Domain)
                {
                    this.StarEntities.Add(parentLayer.GetAIEntityFromEntity(starEntity) as AIStarEntity);
                }
            }
        }

        public override void UpdateAI(Time deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
