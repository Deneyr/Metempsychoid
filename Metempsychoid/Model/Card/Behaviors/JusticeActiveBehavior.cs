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
    public class JusticeActiveBehavior : ACardActiveBehavior
    {
        public int NbUse
        {
            get;
            private set;
        }

        public JusticeActiveBehavior(int nbUse)
        {
            this.NbUse = nbUse;
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new ResurrectCardNotifBehavior(this, starEntity.CardSocketed));

                return true;
            }
            return false;
        }

        public override void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NbUse;

            (behavior as ResurrectCardNotifBehavior).FromCardEntities = behavior.NodeLevel.GetLayerFromPlayer(behavior.OwnerCardEntity.Card.CurrentOwner).CardsCemetery.ToList();
        }

        public override void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            (behavior as ResurrectCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
        }

        public override ICardBehavior Clone()
        {
            JusticeActiveBehavior clone = new JusticeActiveBehavior(this.NbUse);

            clone.InitFrom(this);

            return clone;
        }
    }
}