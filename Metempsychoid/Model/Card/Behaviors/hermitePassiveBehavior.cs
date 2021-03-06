﻿using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class HermitePassiveBehavior : ICardBehavior
    {
        public int Value
        {
            get;
            private set;
        }

        public HermitePassiveBehavior(int value)
        {
            this.Value = value;
        }

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                if (actionsOccured.Any(pElem => pElem is IModifyStarEntityAction))
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
            int bonus = layer.StarToLinks[starEntity].Where(pElem =>
            {
                StarEntity otherStarEntity = pElem.StarFrom;
                if(otherStarEntity == starEntity)
                {
                    otherStarEntity = pElem.StarTo;
                }

                return otherStarEntity.CardSocketed != null;
            }).Count();


            bool mustSetValue = starEntity.CardSocketed.Card.BehaviorToValueModifier.TryGetValue(this, out int currentValue) == false || currentValue != bonus;

            if (mustSetValue)
            {
                layer.PendingActions.Add(new SetCardValueModifier(starEntity.CardSocketed.Card, this, bonus));
            }
        }

        public void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(ownerCardEntity.Card, this));
        }

        public void OnDestroyed(BoardGameLayer layer, CardEntity cardEntity)
        {

        }

        public ICardBehavior Clone()
        {
            return new HermitePassiveBehavior(this.Value);
        }
    }
}
