using Metempsychoid.Model.Card.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class SetCardValueModifier : IBoardGameAction
    {
        public Card.Card CardToSetValue
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

        public SetCardValueModifier(Card.Card cardToSetValue, ICardBehavior behaviorFrom, int value)
        {
            this.CardToSetValue = cardToSetValue;

            this.BehaviorFrom = behaviorFrom;

            this.Value = value;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToSetValue.SetValueModifier(this.BehaviorFrom, this.Value);
        }
    }
}
