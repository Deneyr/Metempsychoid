using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class StrengthPassiveBehavior : ICardBehavior
    {
        private Dictionary<Card, List<CardEntity>> instanceToAffectedCardEntities;

        public int Value
        {
            get;
            private set;
        }

        public StrengthPassiveBehavior(int value)
        {
            this.Value = value;

            this.instanceToAffectedCardEntities = new Dictionary<Card, List<CardEntity>>();
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
            this.instanceToAffectedCardEntities[starEntity.CardSocketed.Card] = new List<CardEntity>();

            this.UpdateValue(layer, starEntity);
        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            HashSet<StarLinkEntity> starLinkEntities = layer.StarToLinks[starEntity];

            List<CardEntity> previousAffectedStarEntity = this.instanceToAffectedCardEntities[starEntity.CardSocketed.Card];
            List<CardEntity> currentAffectedStarEntity = new List<CardEntity>();
            foreach(StarLinkEntity starLinkEntity in starLinkEntities)
            {
                StarEntity otherStarEntity = starLinkEntity.StarFrom;
                if (otherStarEntity == starEntity)
                {
                    otherStarEntity = starLinkEntity.StarTo;
                }

                if(otherStarEntity.CardSocketed != null && otherStarEntity.CardSocketed.Card.Player == starEntity.CardSocketed.Card.Player)
                {
                    currentAffectedStarEntity.Add(otherStarEntity.CardSocketed);
                }
            }

            IEnumerable<CardEntity> noMoreAffected = previousAffectedStarEntity.Except(currentAffectedStarEntity);
            IEnumerable<CardEntity> newAffected = currentAffectedStarEntity.Except(previousAffectedStarEntity);

            foreach(CardEntity cardEntity in noMoreAffected)
            {
                layer.PendingActions.Add(new ClearCardValueModifier(cardEntity.Card, this));
            }

            foreach (CardEntity cardEntity in newAffected)
            {
                layer.PendingActions.Add(new SetCardValueModifier(cardEntity.Card, this, this.Value));
            }

            this.instanceToAffectedCardEntities[starEntity.CardSocketed.Card] = currentAffectedStarEntity;
        }

        public void OnUnawakened(BoardGameLayer layer, StarEntity starEntity)
        {
            List<CardEntity> previousAffectedStarEntity = this.instanceToAffectedCardEntities[starEntity.CardSocketed.Card];

            foreach (CardEntity cardEntity in previousAffectedStarEntity)
            {
                layer.PendingActions.Add(new ClearCardValueModifier(cardEntity.Card, this));
            }

            this.instanceToAffectedCardEntities.Remove(starEntity.CardSocketed.Card);
        }

        public ICardBehavior Clone()
        {
            return new StrengthPassiveBehavior(this.Value);
        }
    }
}
