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
    public class CartActiveBehavior : ACardBehavior, ICardBehaviorOwner
    {
        public int NbUse
        {
            get;
            private set;
        }

        public CartActiveBehavior(int nbUse)
        {
            this.NbUse = nbUse;
        }

        public override void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                IBoardGameAction selfSocketCardAction = actionsOccured.FirstOrDefault(pElem => pElem is SocketCardAction && (pElem as SocketCardAction).CardToSocket == starEntity.CardSocketed);
                IBoardGameAction selfUnsocketCardAction = actionsOccured.FirstOrDefault(pElem => pElem is UnsocketCardAction && (pElem as UnsocketCardAction).CardToUnsocket == starEntity.CardSocketed);

                if (selfSocketCardAction != null && selfUnsocketCardAction != null)
                {
                    if(layer.StarToLinks[starEntity]
                        .Select(pElem => pElem.StarFrom != starEntity ? pElem.StarFrom : pElem.StarTo)
                        .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner != starEntity.CardSocketed.Card.CurrentOwner).Any() == false)
                    {
                        return;
                    }

                    layer.RegisterNotifBehavior(new DeleteCardNotifBehavior(this, starEntity.CardSocketed));
                }
            }
        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NbUse;

            DeleteCardNotifBehavior moveCardBehavior = behavior as DeleteCardNotifBehavior;
            moveCardBehavior.FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarToLinks[behavior.OwnerCardEntity.ParentStar]
                .Select(pElem => pElem.StarFrom != behavior.OwnerCardEntity.ParentStar ? pElem.StarFrom : pElem.StarTo)
                .Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner != behavior.OwnerCardEntity.Card.CurrentOwner).ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {

        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            
        }

        public override ICardBehavior Clone()
        {
            return new CartActiveBehavior(this.NbUse);
        }
    }
}
