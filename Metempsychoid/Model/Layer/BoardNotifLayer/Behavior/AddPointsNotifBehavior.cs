using Astrategia.Model.Card;
using Astrategia.Model.Card.Behaviors;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public class AddPointsNotifBehavior : DeleteCardNotifBehavior
    {
        public int PointsToAdd
        {
            get;
            private set;
        }

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

                    this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorPhaseChanged("AddPointsNotifBehavior." + this.State.ToString());
                }
            }
        }

        public AddPointsNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity, int pointsToAdd) : base(cardBehaviorOwner, ownerCardEntity)
        {
            this.PointsToAdd = pointsToAdd;
        }

        protected override void ExecuteBehavior(StarEntity starEntity)
        {
            this.ModifiedCardEntities.Add(starEntity.CardSocketed);

            this.NodeLevel.BoardGameLayer.PendingActions.Add(new SetCardValueModifier(starEntity.CardSocketed, this.CardBehaviorOwner as ICardBehavior, this.PointsToAdd));
        }
    }
}
