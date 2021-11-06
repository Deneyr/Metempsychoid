using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using Astrategia.Model.Layer.BoardNotifLayer.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Card.Behaviors
{
    public class MagicianActiveBehavior : ACardActiveBehavior
    {
        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            if(layer.StarSystem
                .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner == starEntity.CardSocketed.Card.CurrentOwner && pElem.CardSocketed != starEntity.CardSocketed).Any() == false)
            {
                return;
            }

            if (layer.StarToLinks[starEntity]
                .Select(pElem => pElem.StarFrom != starEntity ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed == null).Any() == false)
            {
                return;
            }

            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new MoveCardNotifBehavior(this, starEntity.CardSocketed));

                return true;
            }
            return false;
        }

        public override void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = 1;

            MoveCardNotifBehavior moveCardBehavior = behavior as MoveCardNotifBehavior;
            moveCardBehavior.FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem
                .Where(pElem => pElem.CardSocketed != null
                && pElem.CardSocketed.Card.CanBeMoved
                && pElem.CardSocketed.Card.CurrentOwner == behavior.OwnerCardEntity.Card.CurrentOwner 
                && pElem.CardSocketed != behavior.OwnerCardEntity).ToList();
        }

        public override void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            MoveCardNotifBehavior moveCardBehavior = behavior as MoveCardNotifBehavior;

            moveCardBehavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarToLinks[behavior.OwnerCardEntity.ParentStar]
                .Select(pElem => pElem.StarFrom != behavior.OwnerCardEntity.ParentStar ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed == null).ToList();
        }

        public override ICardBehavior Clone()
        {
            MagicianActiveBehavior clone = new MagicianActiveBehavior();

            clone.InitFrom(this);

            return clone;
        }
    }
}

