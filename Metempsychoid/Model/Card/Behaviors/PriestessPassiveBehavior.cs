﻿using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Card.Behaviors
{
    public class PriestessPassiveBehavior : ACardBehavior
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

        public PriestessPassiveBehavior(int value, params string[] cardNames)
        {
            this.CardNames = cardNames;

            this.Value = value;
        }

        public override void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                if (actionsOccured.Any(pElem => pElem is IModifyStarEntityAction))
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
            int bonus = 0;
            foreach (string cardName in this.CardNames)
            {
                if (layer.NameToOnBoardCardEntities.TryGetValue(cardName, out HashSet<CardEntity> cardEntities))
                {
                    bonus += cardEntities.Count;

                    if (cardEntities.Contains(starEntity.CardSocketed))
                    {
                        bonus--;
                    }
                }
            }

            bonus *= this.Value;

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
            return new PriestessPassiveBehavior(this.Value, this.CardNames);
        }
    }
}
