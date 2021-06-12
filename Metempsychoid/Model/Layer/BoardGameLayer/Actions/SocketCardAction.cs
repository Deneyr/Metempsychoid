using Metempsychoid.Model.Card;
using SFML.System;
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

        //public Vector2f PositionInNotifBoard
        //{
        //    get;
        //    private set;
        //}

        public SocketCardAction(CardEntity cardEntity, StarEntity starEntity) //, Vector2f positionInNotifBoard)
        {
            this.CardToSocket = cardEntity;

            this.OwnerStar = starEntity;

            //this.PositionInNotifBoard = positionInNotifBoard;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            //this.CardToSocket.Card.CardSocketed(layerToPerform, this.OwnerStar);
            this.OwnerStar.CardSocketed = this.CardToSocket;
            //this.OwnerStar.CardSocketed.PositionInNotifBoard = this.PositionInNotifBoard;

            if (layerToPerform.NameToOnBoardCardEntities.TryGetValue(this.CardToSocket.Card.Name, out HashSet<CardEntity> cardEntities))
            {
                cardEntities.Add(this.CardToSocket);
            }
            else
            {
                layerToPerform.NameToOnBoardCardEntities.Add(this.CardToSocket.Card.Name, new HashSet<CardEntity>() { this.CardToSocket });
            }

            layerToPerform.CardEntityPicked = null;
        }
    }
}
