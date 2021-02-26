using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.EntityLayer;

namespace Metempsychoid.Model.Card
{
    public class CardEntity : AEntity
    {
        private bool isSocketed;

        public Card Card
        {
            get;
            private set;
        }

        public bool IsSocketed
        {
            get
            {
                return this.isSocketed;
            }
            set
            {
                if (this.isSocketed != value)
                {
                    this.isSocketed = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsSocketed");
                    }
                }
            }
        }

        public CardEntity(EntityLayer entityLayer, Card card) : base(entityLayer)
        {
            this.Card = card;

            this.IsSocketed = false;
        }
    }
}
