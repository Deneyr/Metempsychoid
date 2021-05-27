using Metempsychoid.Model.Card;
using Metempsychoid.Model.Card.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class AddCardValueModifier : IBoardGameAction
    {
        public Card.Card CardToAddValue
        {
            get;
            private set;
        }

        public ICardBehavior BehaviorFrom
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

        public AddCardValueModifier(Card.Card cardToAddValue, ICardBehavior behaviorFrom, int value, bool mustDeleteIfNull = false)
        {
            this.CardToAddValue = cardToAddValue;

            this.BehaviorFrom = behaviorFrom;

            this.Value = value;

            this.MustDeleteIfNull = mustDeleteIfNull;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToAddValue.AddValueModifier(this.BehaviorFrom, this.Value, this.MustDeleteIfNull);
        }
    }
}
