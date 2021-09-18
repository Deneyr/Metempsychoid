using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public abstract class AConstellation: IConstellation
    {
        protected WeakReference<Card.Card> parentCard;

        protected bool isAwakened;

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
            protected set
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

        public AConstellation()
        {
            this.NodeToStarEntity = new Dictionary<ConstellationNode, StarEntity>();
            this.LinkToStarLinkEntity = new Dictionary<ConstellationLink, List<StarLinkEntity>>();

            this.isAwakened = false;
        }

        public abstract void OnOtherStarEntitiesChanged(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged);


        public virtual void ResetConstellation()
        {
            this.NodeToStarEntity.Clear();
            this.LinkToStarLinkEntity.Clear();
            this.isAwakened = false;
        }

        public void Dispose()
        {
            this.NodeToStarEntity.Clear();
            this.LinkToStarLinkEntity.Clear();

            this.isAwakened = false;
        }

        public abstract IConstellation Clone(Card.Card parentCard);
    }
}
