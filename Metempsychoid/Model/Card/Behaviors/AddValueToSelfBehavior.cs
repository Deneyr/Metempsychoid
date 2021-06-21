using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class AddValueToSelfBehavior : ICardBehavior
    {
        public int Value
        {
            get;
            private set;
        }

        public AddValueToSelfBehavior(int value)
        {
            this.Value = value;
        }

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            //if (starEntity.CardSocketed.Card.IsAwakened)
            //{
            //    IBoardGameAction socketCardAction = actionsOccured.FirstOrDefault(pElem => (pElem is SocketCardAction) && ((SocketCardAction)pElem).OwnerStar == starEntity);

            //    if (socketCardAction is default(IBoardGameAction) == false)
            //    {
            //        layer.PendingActions.Add(new AddCardValueModifier(starEntity.CardSocketed.Card, starEntity.CardSocketed, -this.Value, true));
            //    }
            //}
        }

        public void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            layer.PendingActions.Add(new AddCardValueModifier(starEntity.CardSocketed.Card, this, this.Value));
        }

        public void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(ownerCardEntity.Card, this));
        }

        public ICardBehavior Clone()
        {
            return new AddValueToSelfBehavior(this.Value);
        }
    }
}
