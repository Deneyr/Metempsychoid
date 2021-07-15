using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class ConstellationSpecificCardNode : ConstellationNode
    {
        public string CardName
        {
            get;
            private set;
        }

        public ConstellationSpecificCardNode(string cardName)
        {
            this.CardName = cardName;
        }

        public override bool IsStarValid(StarEntity star, StarEntity startStarEntity)
        {
            return base.IsStarValid(star, startStarEntity) && startStarEntity.CardSocketed.Card.Name == this.CardName;
        }
    }
}

