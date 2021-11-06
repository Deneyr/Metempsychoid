using Astrategia.Model.Card;
using Astrategia.Model.Card.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer.Actions
{
    public class ClearCardValueModifier : IBoardGameAction
    {
        public CardEntity CardToRemoveValue
        {
            get;
            private set;
        }

        public ICardBehavior BehaviorFrom
        {
            get;
            private set;
        }

        public ClearCardValueModifier(CardEntity cardToRemoveValue, ICardBehavior behaviorFrom)
        {
            this.CardToRemoveValue = cardToRemoveValue;

            this.BehaviorFrom = behaviorFrom;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToRemoveValue.Card.ClearValueModifier(this.BehaviorFrom);
        }

        public bool IsStillValid(BoardGameLayer layerToPerform)
        {
            return this.CardToRemoveValue.IsValid;
        }
    }
}
