using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class Constellation: AConstellation
    {
        protected ConstellationPattern constellationPattern;

        public Constellation(ConstellationPattern constellationPattern)
        {
            this.constellationPattern = constellationPattern;
        }

        public override void OnOtherStarEntitiesChanged(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {
            if (this.isAwakened)
            {
                StarEntity starEntityChanged = this.NodeToStarEntity.Values.FirstOrDefault(pElem => starEntitiesChanged.Contains(pElem));

                if (starEntityChanged != null)
                {
                    this.IsAwakened = this.constellationPattern.CreateConstellationSystem(
                        boardGameLayer,
                        starEntity,
                        this.NodeToStarEntity,
                        this.LinkToStarLinkEntity);
                }
            }
            else
            {
                this.IsAwakened = this.constellationPattern.CreateConstellationSystem(
                    boardGameLayer,
                    starEntity,
                    this.NodeToStarEntity,
                    this.LinkToStarLinkEntity);
            }
        }

        public override IConstellation Clone(Card.Card parentCard)
        {
            Constellation newConstellation = new Constellation(this.constellationPattern);

            newConstellation.parentCard = new WeakReference<Card.Card>(parentCard);

            return newConstellation;
        }
    }
}
