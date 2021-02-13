using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class StarEntity: AEntity
    {
        private Card.Card cardSocketed;

        public virtual Card.Card CardSocketed
        {
            get
            {
                return this.cardSocketed;
            }
            set
            {
                if (value != this.cardSocketed)
                {
                    this.cardSocketed = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "CardSocketed");
                    }
                }
            }
        }

        public StarEntity(BoardGameLayer boardGameLayer) : base(boardGameLayer)
        {
        }
    }
}
