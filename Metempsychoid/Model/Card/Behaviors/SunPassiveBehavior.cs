using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class SunPassiveBehavior : ICardBehavior
    {
        private List<CardEntity> affectedCardEntities;

        public int Value
        {
            get;
            private set;
        }

        public SunPassiveBehavior(int value)
        {
            this.Value = value;

            this.affectedCardEntities = new List<CardEntity>();
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
            this.affectedCardEntities.Clear();

            this.UpdateValue(layer, starEntity);
        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            HashSet<StarLinkEntity> starLinkEntities = layer.StarToLinks[starEntity];

            List<CardEntity> currentAffectedStarEntity = new List<CardEntity>();
            foreach (Constellation constellation in starEntity.CardSocketed.Card.Constellations)
            {
                currentAffectedStarEntity.AddRange(constellation.NodeToStarEntity.Values.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner == starEntity.CardSocketed.Card.CurrentOwner).Select(pElem => pElem.CardSocketed));
            }

            IEnumerable<CardEntity> noMoreAffected = this.affectedCardEntities.Except(currentAffectedStarEntity);
            IEnumerable<CardEntity> newAffected = currentAffectedStarEntity.Except(this.affectedCardEntities);

            foreach (CardEntity cardEntity in noMoreAffected)
            {
                layer.PendingActions.Add(new ClearCardValueModifier(cardEntity.Card, this));
            }

            foreach (CardEntity cardEntity in newAffected)
            {
                layer.PendingActions.Add(new SetCardValueModifier(cardEntity.Card, this, this.Value));
            }

            this.affectedCardEntities = currentAffectedStarEntity;
        }

        public void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            foreach (CardEntity cardEntity in this.affectedCardEntities)
            {
                layer.PendingActions.Add(new ClearCardValueModifier(cardEntity.Card, this));
            }

            this.affectedCardEntities.Clear();
        }

        public void OnDestroyed(BoardGameLayer layer, CardEntity cardEntity)
        {

        }

        public ICardBehavior Clone()
        {
            return new SunPassiveBehavior(this.Value);
        }
    }
}
