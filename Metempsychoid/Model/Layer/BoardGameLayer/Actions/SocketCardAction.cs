using Metempsychoid.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class SocketCardAction : IModifyStarEntityAction
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

        public SocketCardAction(CardEntity cardEntity, StarEntity starEntity)
        {
            this.CardToSocket = cardEntity;

            this.OwnerStar = starEntity;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            //this.CardToSocket.Card.CardSocketed(layerToPerform, this.OwnerStar);
            this.OwnerStar.CardSocketed = this.CardToSocket;

            if(layerToPerform.NameToOnBoardCardEntities.TryGetValue(this.CardToSocket.Card.Name, out HashSet<CardEntity> cardEntities))
            {
                cardEntities.Add(this.CardToSocket);
            }
            else
            {
                layerToPerform.NameToOnBoardCardEntities.Add(this.CardToSocket.Card.Name, new HashSet<CardEntity>() { this.CardToSocket });
            }
        }
    }
}
