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
    public class WheelActiveBehavior : ACardActiveBehavior
    {
        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            if (layer.StarToLinks[starEntity]
                .Select(pElem => pElem.StarFrom != starEntity ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed != null).Any() == false)
            {
                return;
            }

            if (layer.StarSystem
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
            moveCardBehavior.FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarToLinks[behavior.OwnerCardEntity.ParentStar]
                .Select(pElem => pElem.StarFrom != behavior.OwnerCardEntity.ParentStar ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CanBeMoved).ToList();
        }

        public override void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            MoveCardNotifBehavior moveCardBehavior = behavior as MoveCardNotifBehavior;

            moveCardBehavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
        }

        public override ICardBehavior Clone()
        {
            WheelActiveBehavior clone = new WheelActiveBehavior();

            clone.InitFrom(this);

            return clone;
        }
    }
}
