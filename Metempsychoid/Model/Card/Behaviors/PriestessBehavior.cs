using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class PriestessBehavior : ICardBehavior
    {
        public string[] CardNames
        {
            get;
            private set;
        }

        public int Value
        {
            get;
            private set;
        }

        public PriestessBehavior(int value, params string[] cardNames)
        {
            this.CardNames = cardNames;

            this.Value = value;
        }

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                if (actionsOccured.Any(pElem => pElem is SocketCardAction || pElem is UnsocketCardAction))
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
            int bonus = 0;
            foreach (string cardName in this.CardNames)
            {
                if (layer.NameToOnBoardCardEntities.TryGetValue(cardName, out HashSet<CardEntity> cardEntities))
                {
                    bonus = cardEntities.Count;

                    if (cardEntities.Contains(starEntity.CardSocketed))
                    {
                        bonus--;
                    }
                }
            }

            if(starEntity.CardSocketed.Card.BehaviorToValueModifier.TryGetValue(this, out int currentValue))
            {
                bonus -= currentValue;
            }

            if (bonus != 0)
            {
                layer.PendingActions.Add(new AddCardValueModifier(starEntity.CardSocketed.Card, this, bonus));
            }
        }

        public void OnUnawakened(BoardGameLayer layer, StarEntity starEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(starEntity.CardSocketed.Card, this));
        }
    }
}
