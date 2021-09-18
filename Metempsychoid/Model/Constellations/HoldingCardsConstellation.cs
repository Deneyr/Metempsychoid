using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;

namespace Metempsychoid.Model.Constellations
{
    public class HoldingCardsConstellation : HoldingConstellation
    {
        public HoldingCardsConstellation(ConstellationPattern constellationPattern) 
            : base(constellationPattern, null)
        {
        }

        protected override void OnAwakening(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {
            this.holdingConstellationPattern = ConstellationPatternFactory.CreateAssociatedCardsPatternFrom(this.constellationPattern, this);
        }

        public override IConstellation Clone(Card.Card parentCard)
        {
            HoldingCardsConstellation newConstellation = new HoldingCardsConstellation(this.constellationPattern);

            newConstellation.parentCard = new WeakReference<Card.Card>(parentCard);

            return newConstellation;
        }
    }
}
