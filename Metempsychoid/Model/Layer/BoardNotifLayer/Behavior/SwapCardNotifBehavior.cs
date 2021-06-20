using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class SwapCardNotifBehavior : MoveCardNotifBehavior
    {
        public SwapCardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity) 
            : base(cardBehaviorOwner, ownerCardEntity)
        {
        }

        protected override void ExecuteBehavior(StarEntity starEntity)
        {
            this.NodeLevel.BoardGameLayer.SwapCard(starEntity);
        }
    }
}
