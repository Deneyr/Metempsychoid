using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using Astrategia.Model.Layer.BoardNotifLayer.Behavior;
using Astrategia.Model.Node.TestWorld;

namespace Astrategia.Model.Card.Behaviors
{
    public class FoolActiveBehavior : ACardActiveBehavior
    {
        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new SwapCardNotifBehavior(this, starEntity.CardSocketed));

                return true;
            }
            return false;
        }

        public override void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = 1;

            (behavior as SwapCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CanBeMoved).ToList();
        }

        public override void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            CardEntity cardEntity = behavior.NodeLevel.BoardGameLayer.CardEntityPicked;

            (behavior as SwapCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed != cardEntity).ToList();
        }

        public override ICardBehavior Clone()
        {
            FoolActiveBehavior clone = new FoolActiveBehavior();

            clone.InitFrom(this);

            return clone;
        }
    }
}
