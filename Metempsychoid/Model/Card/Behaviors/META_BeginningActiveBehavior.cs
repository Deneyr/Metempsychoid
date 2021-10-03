using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class META_BeginningActiveBehavior : ACardActiveBehavior
    {
        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new SendInternalEventNotifBehavior(starEntity.CardSocketed, Event.InternalEventType.GO_TO_LEVEL, "CardBoardLevel"));

                return true;
            }
            return false;
        }

        public override ICardBehavior Clone()
        {
            return new META_BeginningActiveBehavior();
        }
    }
}
