using Metempsychoid.Model.Card.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class ClearCardValueModifier : IBoardGameAction
    {
        public Card.Card CardToRemoveValue
        {
            get;
            private set;
        }

        public ICardBehavior BehaviorFrom
        {
            get;
            private set;
        }

        public ClearCardValueModifier(Card.Card cardToRemoveValue, ICardBehavior behaviorFrom)
        {
            this.CardToRemoveValue = cardToRemoveValue;

            this.BehaviorFrom = behaviorFrom;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToRemoveValue.ClearValueModifier(this.BehaviorFrom);
        }
    }
}
