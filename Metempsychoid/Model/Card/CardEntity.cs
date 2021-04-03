using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.EntityLayer;

namespace Metempsychoid.Model.Card
{
    public class CardEntity : AEntity
    {
        private StarEntity parentStar;

        private bool isFliped;

        public Card Card
        {
            get;
            private set;
        }

        public StarEntity ParentStar
        {
            get
            {
                return this.parentStar;
            }
            set
            {
                if (this.parentStar != value)
                {
                    this.parentStar = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsSocketed");
                    }
                }
            }
        }

        public bool IsFliped
        {
            get
            {
                return this.isFliped;
            }
            set
            {
                if (this.isFliped != value)
                {
                    this.isFliped = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsFliped");
                    }
                }
            }
        }

        public CardEntity(EntityLayer entityLayer, Card card, bool isFliped) : base(entityLayer)
        {
            this.Card = card;

            this.parentStar = null;

            this.isFliped = isFliped;
        }
    }
}
