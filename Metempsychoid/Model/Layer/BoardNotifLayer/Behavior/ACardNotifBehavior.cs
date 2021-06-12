using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Node.TestWorld;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public abstract class ACardNotifBehavior : ABoardNotifBehavior
    {
        public ICardBehaviorOwner CardBehaviorOwner
        {
            get;
            private set;
        }

        public List<StarEntity> FromStarEntities
        {
            get;
            set;
        }

        public List<StarEntity> ToStarEntities
        {
            get;
            set;
        }

        public int NbBehaviorUse
        {
            get;
            set;
        }

        public override bool IsActive
        {
            get
            {
                return this.NbBehaviorUse > 0;
            }
        }

        public ACardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity) 
            : base(ownerCardEntity)
        {
            this.CardBehaviorOwner = cardBehaviorOwner;

            this.NbBehaviorUse = 1;
        }
    }
}
