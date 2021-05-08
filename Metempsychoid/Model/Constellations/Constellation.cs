using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class Constellation: IDisposable
    {
        private ConstellationPattern constellationPattern;

        private WeakReference<Card.Card> parentCard;

        private bool isAwakened;

        public Dictionary<ConstellationNode, StarEntity> NodeToStarEntity
        {
            get;
            private set;
        }

        public Dictionary<ConstellationLink, List<StarLinkEntity>> LinkToStarLinkEntity
        {
            get;
            private set;
        }

        public bool IsAwakened
        {
            get
            {
                return this.isAwakened;
            }
            private set
            {
                if (this.isAwakened != value)
                {
                    this.isAwakened = value;

                    if (this.parentCard.TryGetTarget(out Card.Card parentCard))
                    {
                        if (this.isAwakened)
                        {
                            parentCard.OnConstellationAwakened(this);
                        }
                        else
                        {
                            parentCard.OnConstellationUnawakened(this);
                        }
                    }
                }
            }
        }

        public Constellation(Card.Card parentCard, ConstellationPattern constellationPattern)
        {
            this.parentCard = new WeakReference<Card.Card>(parentCard);

            this.constellationPattern = constellationPattern;

            this.NodeToStarEntity = new Dictionary<ConstellationNode, StarEntity>();
            this.LinkToStarLinkEntity = new Dictionary<ConstellationLink, List<StarLinkEntity>>();

            this.isAwakened = false;
        }

        public void OnOtherCardSocketed(BoardGameLayer boardGameLayer, StarEntity starEntity, StarEntity starFromSocketedCard)
        {
            if(this.isAwakened == false)
            {
                this.IsAwakened = this.constellationPattern.CreateConstellationSystem(
                    boardGameLayer,
                    starEntity,
                    this.NodeToStarEntity,
                    this.LinkToStarLinkEntity);
            }
        }

        public void OnOtherCardUnsocketed(BoardGameLayer boardGameLayer, StarEntity starEntity, StarEntity starFromUnsocketedCard)
        {
            if (this.isAwakened)
            {
                if (this.NodeToStarEntity.ContainsValue(starFromUnsocketedCard))
                {
                    this.IsAwakened = this.constellationPattern.CreateConstellationSystem(
                        boardGameLayer,
                        starEntity,
                        this.NodeToStarEntity,
                        this.LinkToStarLinkEntity);
                }
            }
        }

        public void OnCardSocketed(BoardGameLayer boardGameLayer, StarEntity starEntity)
        {
            //this.parentStarEntity = new WeakReference<StarEntity>(starEntity);

            this.IsAwakened = this.constellationPattern.CreateConstellationSystem(
                boardGameLayer,
                starEntity,
                this.NodeToStarEntity,
                this.LinkToStarLinkEntity);
        }

        public void OnCardUnsocketed(BoardGameLayer boardGameLayer, StarEntity starEntity)
        {
            //this.parentStarEntity = null;

            this.NodeToStarEntity.Clear();
            this.LinkToStarLinkEntity.Clear();

            this.IsAwakened = false;
        }

        public void Dispose()
        {
            //this.parentStarEntity = null;

            this.NodeToStarEntity.Clear();
            this.LinkToStarLinkEntity.Clear();

            this.isAwakened = false;
        }
    }
}
