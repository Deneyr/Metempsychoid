using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class StarPassiveBehavior : ACardBehavior, ICardBehaviorOwner
    {
        public int PointsToAdd
        {
            get;
            private set;
        }

        public int NbCardsToAffect
        {
            get;
            private set;
        }

        public HashSet<CardEntity> AffectedCards
        {
            get;
            private set;
        }

        public StarPassiveBehavior(int nbCardToConvert, int pointsToAdd)
        {
            this.NbCardsToAffect = nbCardToConvert;
            this.PointsToAdd = pointsToAdd;

            this.AffectedCards = new HashSet<CardEntity>();
        }

        public override void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (this.AffectedCards.Count > 0)
            {
                if (actionsOccured.Any(pElem => pElem is IModifyStarEntityAction))
                {
                    this.UpdateValue(layer, starEntity);
                }
            }
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.AffectedCards.Clear();

            if (layer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CanBeValueModified).Any())
            {
                layer.RegisterNotifBehavior(new AddPointsNotifBehavior(this, starEntity.CardSocketed, this.PointsToAdd));
            }
        }

        public override void OnUnawakened(BoardGameLayer layer, CardEntity cardEntity)
        {
            foreach (CardEntity cardEntityConverted in this.AffectedCards)
            {
                if (cardEntityConverted.ParentStar != null)
                {
                    layer.PendingActions.Add(new ClearCardValueModifier(cardEntityConverted, this));
                }
            }

            this.AffectedCards.Clear();
        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NbCardsToAffect;

            (behavior as AddPointsNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem
                .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CanBeValueModified)
                .ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            foreach (CardEntity cardEntityConverted in behavior.ModifiedCardEntities)
            {
                this.AffectedCards.Add(cardEntityConverted);
            }
        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {

        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            HashSet<StarLinkEntity> starLinkEntities = layer.StarToLinks[starEntity];

            List<CardEntity> notSocketedAnymore = this.AffectedCards.Where(pElem => pElem.ParentStar == null).ToList();

            foreach (CardEntity cardEntity in notSocketedAnymore)
            {
                layer.PendingActions.Add(new ClearCardValueModifier(cardEntity, this));

                this.AffectedCards.Remove(cardEntity);
            }
        }

        public override ICardBehavior Clone()
        {
            return new StarPassiveBehavior(this.NbCardsToAffect, this.PointsToAdd);
        }
    }
}