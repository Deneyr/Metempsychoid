using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.AI.AICardBoardLayers;
using Astrategia.Model;
using Astrategia.Model.Card;
using SFML.System;

namespace Astrategia.AI.AICard
{
    public class AICardEntity : AAIEntity
    {
        //public AIStarEntity ParentStarEntity
        //{
        //    get;
        //    set;
        //}

        public bool IsFliped
        {
            get;
            set;
        }

        public bool IsSocketed
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public bool IsAwakened
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public string CardOwnerName
        {
            get;
            set;
        }

        public AICardEntity(AAILayer parentLayer, IAIObjectFactory factory, AEntity entity) : base(parentLayer, factory, entity)
        {
            CardEntity cardEntity = entity as CardEntity;

            this.IsSocketed = cardEntity.ParentStar != null;
            this.IsSelected = cardEntity.IsSelected;
            this.IsAwakened = cardEntity.Card.IsAwakened;
            this.CardOwnerName = cardEntity.Card.CurrentOwner.PlayerName;

            this.Value = cardEntity.CardValue;
        }

        public override void UpdateAI(Time deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
