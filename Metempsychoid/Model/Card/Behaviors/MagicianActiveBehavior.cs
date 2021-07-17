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
    public class MagicianActiveBehavior : ICardBehavior, ICardBehaviorOwner
    {

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionOccured)
        {

        }

        public void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            if (layer.StarToLinks[starEntity]
                .Select(pElem => pElem.StarFrom != starEntity ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed == null).Any())
            {
                layer.RegisterNotifBehavior(new MoveCardNotifBehavior(this, starEntity.CardSocketed));
            }
        }

        public void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {

        }

        public void OnDestroyed(BoardGameLayer layer, CardEntity cardEntity)
        {

        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = 1;

            MoveCardNotifBehavior moveCardBehavior = behavior as MoveCardNotifBehavior;
            moveCardBehavior.FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner == behavior.OwnerCardEntity.Card.CurrentOwner && pElem.CardSocketed != behavior.OwnerCardEntity).ToList();
            //behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            //(behavior as ResurrectCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
            //behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            MoveCardNotifBehavior moveCardBehavior = behavior as MoveCardNotifBehavior;

            moveCardBehavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarToLinks[behavior.OwnerCardEntity.ParentStar]
                .Select(pElem => pElem.StarFrom != behavior.OwnerCardEntity.ParentStar ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed == null).ToList();
        }

        public ICardBehavior Clone()
        {
            return new MagicianActiveBehavior();
        }
    }
}

