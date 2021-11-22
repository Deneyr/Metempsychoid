using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.AI.AICard;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;

namespace Astrategia.AI.AIBoardGameLayer
{
    public class AIStarEntity : AAIEntity
    {
        public AICardEntity CardEntitySocketed
        {
            get;
            set;
        }

        public AIStarEntity(AAILayer parentLayer, IAIObjectFactory factory, AEntity entity) 
            : base(parentLayer, factory, entity)
        {
            
        }

        //public void UpdateReference()
        //{
        //    if (this.parentLayer.TryGetTarget(out AAILayer parentLayer))
        //    {
        //        StarEntity starEntity = parentLayer.GetEntityFromEntity2D(this) as StarEntity;

        //        if(starEntity.CardSocketed != null)
        //        {
        //            this.CardEntitySocketed = parentLayer.GetEntity2DFromEntity(starEntity.CardSocketed) as AICardEntity;
        //        }
        //        else
        //        {
        //            this.CardEntitySocketed = null;
        //        }
        //    }
        //}

        public override void UpdateAI(Time deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
