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
    public class HierophantActiveBehavior : ACardActiveBehavior
    {
        public List<string> NewCardsToAdd
        {
            get;
            private set;
        }

        public HierophantActiveBehavior(List<string> newCardsToAdd)
        {
            this.NewCardsToAdd = newCardsToAdd;
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new SocketNewCardNotifBehavior(this, starEntity.CardSocketed, this.NewCardsToAdd));

                return true;
            }
            return false;
        }

        public override void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NewCardsToAdd.Count;
        }

        public override void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            (behavior as SocketNewCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
        }

        public override ICardBehavior Clone()
        {
            HierophantActiveBehavior clone = new HierophantActiveBehavior(this.NewCardsToAdd);

            clone.InitFrom(this);

            return clone;
        }
    }
}
