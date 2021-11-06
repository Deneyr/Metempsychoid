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
    public class WorldActiveBehavior : ACardActiveBehavior
    {
        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new SecureDomainNotifBehavior(starEntity.CardSocketed));

                return true;
            }
            return false;
        }

        public override ICardBehavior Clone()
        {
            WorldActiveBehavior clone = new WorldActiveBehavior();

            clone.InitFrom(this);

            return clone;
        }
    }
}