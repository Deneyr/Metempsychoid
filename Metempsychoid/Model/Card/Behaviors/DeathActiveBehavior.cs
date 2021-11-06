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
    public class DeathActiveBehavior : ACardActiveBehavior
    {
        public int NbUse
        {
            get;
            private set;
        }

        public DeathActiveBehavior(int nbUse)
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
                layer.RegisterNotifBehavior(new DeleteCardNotifBehavior(this, starEntity.CardSocketed));

                return true;
            }
            return false;
        }

        public override void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NbUse;

            (behavior as DeleteCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public override ICardBehavior Clone()
        {
            DeathActiveBehavior clone = new DeathActiveBehavior(this.NbUse);

            clone.InitFrom(this);

            return clone;
        }
    }
}