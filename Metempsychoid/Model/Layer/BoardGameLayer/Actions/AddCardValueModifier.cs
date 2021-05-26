using Metempsychoid.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public struct AddCardValueModifier : IBoardGameAction
    {
        public Card.Card CardToAddValue
        {
            get;
            private set;
        }

        public AEntity EntityFrom
        {
            get;
            private set;
        }

        public int Value
        {
            get;
            private set;
        }

        public bool MustDeleteIfNull
        {
            get;
            private set;
        }

        public AddCardValueModifier(Card.Card cardToAddValue, AEntity entityFrom, int value, bool mustDeleteIfNull = false)
        {
            this.CardToAddValue = cardToAddValue;

            this.EntityFrom = entityFrom;

            this.Value = value;

            this.MustDeleteIfNull = mustDeleteIfNull;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToAddValue.AddValueModifier(this.EntityFrom, this.Value, this.MustDeleteIfNull);
        }
    }
}
