using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;

namespace Astrategia.Model.Constellations
{
    public class HoldingConstellation : AConstellation
    {
        protected ConstellationPattern constellationPattern;

        protected ConstellationPattern holdingConstellationPattern;

        public HoldingConstellation(ConstellationPattern constellationPattern, ConstellationPattern holdingConstellationPattern)
        {
            this.constellationPattern = constellationPattern;

            this.holdingConstellationPattern = holdingConstellationPattern;
        }

        public override void OnOtherStarEntitiesChanged(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {
            if (this.isAwakened)
            {
                StarEntity starEntityChanged = this.NodeToStarEntity.Values.FirstOrDefault(pElem => starEntitiesChanged.Contains(pElem));

                if (starEntityChanged != null)
                {
                    bool isStillAwake = this.holdingConstellationPattern.CreateConstellationSystem(
                        boardGameLayer,
                        starEntity,
                        this.NodeToStarEntity,
                        this.LinkToStarLinkEntity);

                    if(isStillAwake == false)
                    {
                        this.TryToAwake(boardGameLayer, starEntity, starEntitiesChanged);
                    }
                }
            }
            else
            {
                this.TryToAwake(boardGameLayer, starEntity, starEntitiesChanged);
            }
        }

        private void TryToAwake(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {
            bool isAwakening = this.constellationPattern.CreateConstellationSystem(
                                        boardGameLayer,
                                        starEntity,
                                        this.NodeToStarEntity,
                                        this.LinkToStarLinkEntity);



            if (isAwakening)
            {
                this.OnAwakening(boardGameLayer, starEntity, starEntitiesChanged);

                isAwakening = this.holdingConstellationPattern.CreateConstellationSystem(
                    boardGameLayer,
                    starEntity,
                    this.NodeToStarEntity,
                    this.LinkToStarLinkEntity);
            }

            this.IsAwakened = isAwakening;
        }

        protected virtual void OnAwakening(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {
            // To Override
        }

        public override IConstellation Clone(Card.Card parentCard)
        {
            HoldingConstellation newConstellation = new HoldingConstellation(this.constellationPattern, this.holdingConstellationPattern);

            newConstellation.parentCard = new WeakReference<Card.Card>(parentCard);

            return newConstellation;
        }
    }
}
