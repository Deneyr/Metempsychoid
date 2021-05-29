using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class HangedManPassiveBehavior : ICardBehavior
    {
        public int Value
        {
            get;
            private set;
        }

        public HangedManPassiveBehavior(int value)
        {
            this.Value = value;
        }

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                if (actionsOccured.Any(pElem => pElem is UnsocketCardAction))
                {
                    this.UpdateValue(layer, starEntity);
                }
            }
        }

        public void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.UpdateValue(layer, starEntity);
        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            int bonus = layer.GetNbOpponentDeadCard(starEntity.CardSocketed.Card.Player);

            bool mustSetValue = starEntity.CardSocketed.Card.BehaviorToValueModifier.TryGetValue(this, out int currentValue) == false || currentValue != bonus;

            if (mustSetValue)
            {
                layer.PendingActions.Add(new SetCardValueModifier(starEntity.CardSocketed.Card, this, bonus));
            }
        }

        public void OnUnawakened(BoardGameLayer layer, StarEntity starEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(starEntity.CardSocketed.Card, this));
        }

        public ICardBehavior Clone()
        {
            return new HangedManPassiveBehavior(this.Value);
        }
    }
}
