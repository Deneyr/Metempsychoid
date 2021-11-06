using Astrategia.Model.Card;
using Astrategia.Model.Card.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer.Actions
{
    public class SetCardValueModifier : IBoardGameAction
    {
        public CardEntity CardToSetValue
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

        public SetCardValueModifier(CardEntity cardToSetValue, ICardBehavior behaviorFrom, int value)
        {
            this.CardToSetValue = cardToSetValue;

            this.BehaviorFrom = behaviorFrom;

            this.Value = value;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToSetValue.Card.SetValueModifier(this.BehaviorFrom, this.Value);
        }

        public bool IsStillValid(BoardGameLayer layerToPerform)
        {
            return this.CardToSetValue.IsValid;
        }
    }
}
