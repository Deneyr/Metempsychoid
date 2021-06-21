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
        private bool isActive;

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

        public override bool IsThereBehaviorLabel
        {
            get
            {
                return true;
            }
        }

        public override bool IsThereEndButton
        {
            get
            {
                return true;
            }
        }

        public override bool IsActive
        {
            get
            {
                return this.isActive && this.NbBehaviorUse > 0;
            }

            protected set
            {
                this.isActive = value;
            }
        }

        public override void EndNotif(World world)
        {
            foreach (StarEntity starEntity in this.NodeLevel.BoardGameLayer.StarSystem)
            {
                starEntity.CurrentNotifBehavior = null;
            }
        }

        public override void StartNotif(World world)
        {
            this.isActive = true;

            foreach(StarEntity starEntity in this.NodeLevel.BoardGameLayer.StarSystem)
            {
                starEntity.CurrentNotifBehavior = this;
            }
        }

        public virtual bool CanSocketCardOn(StarEntity starEntity, CardEntity cardToSocket)
        {
            return this.ToStarEntities.Contains(starEntity);
        }

        public ACardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity) 
            : base(ownerCardEntity)
        {
            this.CardBehaviorOwner = cardBehaviorOwner;

            this.NbBehaviorUse = 1;
        }
    }
}
