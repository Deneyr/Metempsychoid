using Metempsychoid.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public struct UnsocketCardAction : IModifyStarEntityAction
    {
        public CardEntity CardToSocket
        {
            get;
            private set;
        }

        public StarEntity OwnerStar
        {
            get;
            private set;
        }

        public UnsocketCardAction(CardEntity cardEntity, StarEntity starEntity)
        {
            this.CardToSocket = cardEntity;

            this.OwnerStar = starEntity;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            //this.CardToSocket.Card.CardUnsocketed(layerToPerform, this.OwnerStar);
            this.OwnerStar.CardSocketed = null;
        }
    }
}
