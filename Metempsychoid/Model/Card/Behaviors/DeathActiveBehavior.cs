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
    public class DeathActiveBehavior : ACardBehavior, ICardBehaviorOwner
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
            layer.RegisterNotifBehavior(new DeleteCardNotifBehavior(this, starEntity.CardSocketed));
        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NbUse;

            (behavior as DeleteCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            
        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {

        }

        public override ICardBehavior Clone()
        {
            return new DeathActiveBehavior(this.NbUse);
        }
    }
}