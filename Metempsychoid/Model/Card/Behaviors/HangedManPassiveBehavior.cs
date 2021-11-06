using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Card.Behaviors
{
    public class HangedManPassiveBehavior : ACardBehavior
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

        public override void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                if (actionsOccured.Any(pElem => pElem is UnsocketCardAction || pElem is SocketCardAction))
                {
                    this.UpdateValue(layer, starEntity);
                }
            }
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.UpdateValue(layer, starEntity);
        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            int bonus = layer.GetNbOpponentDeadCard(starEntity.CardSocketed.Card.CurrentOwner) * this.Value;

            bool mustSetValue = starEntity.CardSocketed.Card.BehaviorToValueModifier.TryGetValue(this, out int currentValue) == false || currentValue != bonus;

            if (mustSetValue)
            {
                layer.PendingActions.Add(new SetCardValueModifier(starEntity.CardSocketed, this, bonus));
            }
        }

        public override void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(ownerCardEntity, this));
        }

        public override ICardBehavior Clone()
        {
            return new HangedManPassiveBehavior(this.Value);
        }
    }
}
