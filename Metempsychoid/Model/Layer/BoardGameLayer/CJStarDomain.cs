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

        public Player.Player DomainOwner
        {
            get
            {
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

        public Dictionary<Player.Player, int> PlayerToPoints
        {
            get;
            private set;
        }

        public HashSet<StarEntity> Domain
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


        public CJStarDomain(EntityLayer.EntityLayer entityLayer, HashSet<StarEntity> starDomain, int priority, bool isFilled = true) : base(entityLayer)
        {
            this.Domain = starDomain;

            this.IsFilled = isFilled;

            this.Priority = priority;

            this.domainOwner = null;

            this.PlayerToPoints = new Dictionary<Player.Player, int>();
        }

        public void EvaluateDomainOwner()
        {
            this.PlayerToPoints.Clear();
            foreach (StarEntity starEntity in this.Domain)
            {
                if(starEntity.CardSocketed != null)
                {
                    if (this.PlayerToPoints.ContainsKey(starEntity.CardSocketed.Card.Player) == false)
                    {
                        this.PlayerToPoints.Add(starEntity.CardSocketed.Card.Player, starEntity.CardSocketed.Card.Value);
                    }
                    else
                    {
                        this.PlayerToPoints[starEntity.CardSocketed.Card.Player] += starEntity.CardSocketed.Card.Value;
                    }
                }
            }

            List<KeyValuePair<Player.Player, int>> playerPointsSorted = this.PlayerToPoints.ToList();
            playerPointsSorted.Sort(new PlayerValueComparer());

            if(playerPointsSorted.Count == 1
                || (playerPointsSorted.Count > 1 && playerPointsSorted[0].Value > playerPointsSorted[1].Value))
            {
                this.DomainOwner = playerPointsSorted[0].Key;
            }
            else
            {
                this.DomainOwner = null;
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
