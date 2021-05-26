using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public struct ClearCardValueModifier : IBoardGameAction
    {
        public Card.Card CardToRemoveValue
        {
            get;
            private set;
        }

        public AEntity EntityFrom
        {
            get;
            private set;
        }

        public ClearCardValueModifier(Card.Card cardToRemoveValue, AEntity entityFrom)
        {
            this.CardToRemoveValue = cardToRemoveValue;

            this.EntityFrom = entityFrom;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToRemoveValue.ClearValueModifier(this.EntityFrom);
        }
    }
}
