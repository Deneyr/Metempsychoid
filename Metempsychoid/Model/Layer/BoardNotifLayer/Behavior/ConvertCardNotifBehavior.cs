using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class ConvertCardNotifBehavior : DeleteCardNotifBehavior
    {
        public override DeleteState State
        {
            get
            {
                return this.state;
            }

            protected set
            {
                if (this.state != value)
                {
                    this.state = value;

                    this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorPhaseChanged("ConvertCardNotifBehavior." + this.State.ToString());
                }
            }
        }

        public ConvertCardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity) : base(cardBehaviorOwner, ownerCardEntity)
        {
        }

        protected override void ExecuteBehavior(StarEntity starEntity)
        {
            this.ModifiedCardEntities.Add(starEntity.CardSocketed);

            this.NodeLevel.BoardGameLayer.ConvertCard(starEntity, this.OwnerCardEntity.Card.CurrentOwner);
        }
    }
}
