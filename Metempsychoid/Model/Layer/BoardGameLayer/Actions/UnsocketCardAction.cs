using Metempsychoid.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class UnsocketCardAction : IModifyStarEntityAction
    {
        public CardEntity CardToUnsocket
        {
            get;
            private set;
        }

        public StarEntity OwnerStar
        {
            get;
            private set;
        }

        public bool OffBoard
        {
            get;
            private set;
        }

        public UnsocketCardAction(CardEntity cardEntity, bool offBoard)
        {
            this.CardToUnsocket = cardEntity;

            this.OwnerStar = cardEntity.ParentStar;

            this.OffBoard = offBoard;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            //this.CardToSocket.Card.CardUnsocketed(layerToPerform, this.OwnerStar);
            this.OwnerStar.CardSocketed = null;

            layerToPerform.CardEntityPicked = null;

            if (this.OffBoard)
            {
                HashSet<CardEntity> cardEntities = layerToPerform.NameToOnBoardCardEntities[this.CardToUnsocket.Card.Name];

                cardEntities.Remove(this.CardToUnsocket);

                if (cardEntities.Count == 0)
                {
                    layerToPerform.NameToOnBoardCardEntities.Remove(this.CardToUnsocket.Card.Name);
                }

                layerToPerform.CardsOffBoard.Add(this.CardToUnsocket);
            }
            //else
            //{
            //    this.OwnerStar.CardSocketed = null;
            //}
        }
    }
}
