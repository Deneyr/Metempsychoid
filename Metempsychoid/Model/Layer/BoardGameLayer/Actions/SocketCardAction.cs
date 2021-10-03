using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
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

        public bool MustTravel
        {
            get;
            private set;
        }

        public SocketCardAction(CardEntity cardEntity, StarEntity starEntity, bool mustTravel = false)
        {
            this.CardToSocket = cardEntity;

            this.OwnerStar = starEntity;

            this.MustTravel = mustTravel;

            //this.PositionInNotifBoard = positionInNotifBoard;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            Vector2f previousPosition = this.CardToSocket.Position;

            this.OwnerStar.CardSocketed = this.CardToSocket;

            if (this.MustTravel)
            {
                this.CardToSocket.Position = previousPosition;
                IAnimation positionAnimation = new PositionAnimation(previousPosition, this.OwnerStar.Position, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);

                this.CardToSocket.PlayAnimation(positionAnimation);
            }

            if (layerToPerform.NameToOnBoardCardEntities.TryGetValue(this.CardToSocket.Card.Name, out HashSet<CardEntity> cardEntities))
            {
                cardEntities.Add(this.CardToSocket);
            }
            else
            {
                layerToPerform.NameToOnBoardCardEntities.Add(this.CardToSocket.Card.Name, new HashSet<CardEntity>() { this.CardToSocket });
            }

            //layerToPerform.CardEntityPicked = null;
        }

        public bool IsStillValid(BoardGameLayer layerToPerform)
        {
            return this.CardToSocket.IsValid;
        }
    }
}
