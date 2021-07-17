using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.EntityLayer;
using Metempsychoid.Model.Player;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class CJStarDomain : AEntity
    {
        private Player.Player domainOwner;

        private Player.Player temporaryDomainOwner;

        public Player.Player DomainOwner
        {
            get
            {
                if(this.TemporaryDomainOwner != null)
                {
                    return this.TemporaryDomainOwner;
                }

                return this.domainOwner;
            }
            private set
            {
                if (this.domainOwner != value)
                {
                    this.domainOwner = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "DomainOwner");
                    }
                }
            }
        }

        public Player.Player TemporaryDomainOwner
        {
            get
            {
                return this.temporaryDomainOwner;
            }
            set
            {
                if(this.temporaryDomainOwner != value)
                {
                    this.temporaryDomainOwner = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "DomainOwner");
                    }
                }
            }
        }

        public bool IsThereAtLeastOneCard
        {
            get;
            private set;
        }

        public Dictionary<Player.Player, int> PlayerToPoints
        {
            get;
            private set;
        }

        public List<StarEntity> Domain
        {
            get;
            private set;
        }

        public Dictionary<StarEntity, StarLinkEntity> DomainLinks
        {
            get;
            private set;
        }

        public bool IsFilled
        {
            get;
            private set;
        }

        public int Priority
        {
            get;
            private set;
        }


        public CJStarDomain(EntityLayer.EntityLayer entityLayer, List<StarEntity> starDomain, int priority, bool isFilled = true) : base(entityLayer)
        {
            this.Domain = starDomain;
            this.CreateDomainLinks(entityLayer as BoardGameLayer);

            this.IsFilled = isFilled;

            this.IsThereAtLeastOneCard = false;

            this.Priority = priority;

            this.domainOwner = null;

            this.temporaryDomainOwner = null;

            this.PlayerToPoints = new Dictionary<Player.Player, int>();
        }

        private void CreateDomainLinks(BoardGameLayer boardGameLayer)
        {
            this.DomainLinks = new Dictionary<StarEntity, StarLinkEntity>();

            for(int i = 0; i < this.Domain.Count; i++)
            {
                StarEntity currentStarEntity = this.Domain[i];
                StarEntity nextStarEntity = this.Domain[(i + 1) % this.Domain.Count];

                StarLinkEntity currentLink = boardGameLayer.StarToLinks[currentStarEntity].FirstOrDefault(pElem => pElem.StarFrom == nextStarEntity || pElem.StarTo == nextStarEntity);
                if(currentLink != null)
                {
                    this.DomainLinks.Add(currentStarEntity, currentLink);
                }
            }
        }

        public void EvaluateDomainOwner()
        {
            this.PlayerToPoints.Clear();
            foreach (StarEntity starEntity in this.Domain)
            {
                if(starEntity.CardSocketed != null)
                {
                    if (this.PlayerToPoints.ContainsKey(starEntity.CardSocketed.Card.CurrentOwner) == false)
                    {
                        this.PlayerToPoints.Add(starEntity.CardSocketed.Card.CurrentOwner, starEntity.CardSocketed.Card.Value);
                    }
                    else
                    {
                        this.PlayerToPoints[starEntity.CardSocketed.Card.CurrentOwner] += starEntity.CardSocketed.Card.Value;
                    }
                }
            }

            List<KeyValuePair<Player.Player, int>> playerPointsSorted = this.PlayerToPoints.ToList();
            playerPointsSorted.Sort(new PlayerValueComparer());

            if(playerPointsSorted.Count == 0
                || playerPointsSorted[0].Value == 0)
            {
                this.IsThereAtLeastOneCard = false;

                this.DomainOwner = null;
            }
            else
            {
                this.IsThereAtLeastOneCard = true;

                if (playerPointsSorted.Count == 1
                    || playerPointsSorted[0].Value > playerPointsSorted[1].Value)
                {
                    this.DomainOwner = playerPointsSorted[0].Key;
                }
                else
                {
                    this.DomainOwner = null;
                }
            }

            if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
            {
                entityLayer.NotifyObjectPropertyChanged(this, "PlayerToPoints");
            }
        }

        private class PlayerValueComparer : IComparer<KeyValuePair<Player.Player, int>>
        {
            public int Compare(KeyValuePair<Player.Player, int> x, KeyValuePair<Player.Player, int> y)
            {
                return y.Value - x.Value;
            }
        }
    }
}
