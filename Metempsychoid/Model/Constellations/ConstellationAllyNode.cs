using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Constellations
{
    public class ConstellationAllyNode : ConstellationNode
    {
        public override bool IsStarValid(StarEntity star, StarEntity startStarEntity)
        {
            return base.IsStarValid(star, startStarEntity) && startStarEntity.CardSocketed.Card.CurrentOwner == star.CardSocketed.Card.CurrentOwner;
        }
    }
}
