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
    public class LoverPassiveBehavior : ACardBehavior, ICardBehaviorOwner
    {
        public int NbCardsToConvert
        {
            get;
            private set;
        }

        public HashSet<CardEntity> ConvertedCards
        {
            get;
            private set;
        }

        public LoverPassiveBehavior(int nbCardToConvert)
        {
            this.NbCardsToConvert = nbCardToConvert;

            this.ConvertedCards = new HashSet<CardEntity>();
        }

        public override void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (this.ConvertedCards.Count > 0)
            {
                if (actionsOccured.Any(pElem => pElem is IModifyStarEntityAction))
                {
                    this.UpdateValue(layer, starEntity);
                }
            }
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ConvertedCards.Clear();

            if (layer.StarToLinks[starEntity]
                .Select(pElem => pElem.StarFrom != starEntity ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner != starEntity.CardSocketed.Card.CurrentOwner).Any())
            {
                layer.RegisterNotifBehavior(new ConvertCardNotifBehavior(this, starEntity.CardSocketed));
            }
        }

        public override void OnUnawakened(BoardGameLayer layer, CardEntity cardEntity)
        {
            foreach (CardEntity cardEntityConverted in this.ConvertedCards)
            {
                if (cardEntityConverted.ParentStar != null && cardEntityConverted.Card.CurrentOwner != cardEntityConverted.Card.FirstOwner)
                {
                    layer.ConvertCard(cardEntityConverted.ParentStar, cardEntityConverted.Card.FirstOwner);
                }
            }

            this.ConvertedCards.Clear();
        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NbCardsToConvert;

            (behavior as ConvertCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarToLinks[behavior.OwnerCardEntity.ParentStar]
                .Select(pElem => pElem.StarFrom != behavior.OwnerCardEntity.ParentStar ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner != behavior.OwnerCardEntity.Card.CurrentOwner)
                .ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            foreach (CardEntity cardEntityConverted in behavior.ModifiedCardEntities)
            {
                this.ConvertedCards.Add(cardEntityConverted);
            }
        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {

        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            HashSet<StarLinkEntity> starLinkEntities = layer.StarToLinks[starEntity];

            List<CardEntity> notLinkedAnymore = this.ConvertedCards.Where(pElem => pElem.ParentStar == null || starLinkEntities.Any(pLinkElem => pLinkElem.StarFrom == pElem.ParentStar || pLinkElem.StarTo == pElem.ParentStar) == false).ToList();

            foreach (CardEntity cardEntity in notLinkedAnymore)
            {
                if (cardEntity.ParentStar != null && cardEntity.Card.CurrentOwner != cardEntity.Card.FirstOwner)
                {
                    layer.ConvertCard(cardEntity.ParentStar, cardEntity.Card.FirstOwner);
                }

                this.ConvertedCards.Remove(cardEntity);
            }
        }

        public override ICardBehavior Clone()
        {
            return new LoverPassiveBehavior(this.NbCardsToConvert);
        }
    }
}
