﻿using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Constellations
{
    public class ConstellationCardEntityNode: ConstellationNode
    {
        public CardEntity AssociatedCardEntity
        {
            get;
            private set;
        }

        public ConstellationCardEntityNode(CardEntity associatedCardEntity)
        {
            this.AssociatedCardEntity = associatedCardEntity;
        }

        public override bool IsStarValid(StarEntity star, StarEntity startStarEntity)
        {
            return star.CardSocketed == this.AssociatedCardEntity;
        }
    }
}
