using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.EntityLayer;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class CJStarDomain : AEntity
    {
        public Player.Player DomainOwner
        {
            get;
            private set;
        }

        public Dictionary<Player.Player, int> playerToPoints
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


        public CJStarDomain(EntityLayer.EntityLayer entityLayer, HashSet<StarEntity> starDomain, bool isFilled = true) : base(entityLayer)
        {
            this.Domain = starDomain;

            this.IsFilled = isFilled;

            this.playerToPoints = new Dictionary<Player.Player, int>();
        }

        public void EvaluateDomainOwner()
        {
            this.playerToPoints.Clear();
            foreach (StarEntity starEntity in this.Domain)
            {
                if(starEntity.CardSocketed != null)
                {
                    if (this.playerToPoints.ContainsKey(starEntity.CardSocketed.Card.Player) == false)
                    {
                        this.playerToPoints.Add(starEntity.CardSocketed.Card.Player, starEntity.CardSocketed.Card.DefaultValue);
                    }
                }
            }
        }
    }
}
