using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Node.TestWorld;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public abstract class ACardNotifBehavior : ABoardNotifBehavior
    {
        private bool isActive;

        public ICardBehaviorOwner CardBehaviorOwner
        {
            get;
            private set;
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

        public HashSet<CardEntity> ModifiedCardEntities
        {
            get;
            private set;
        }

        public override void EndNotif(World world)
        { 
            foreach (StarEntity starEntity in this.NodeLevel.BoardGameLayer.StarSystem)
            {
                starEntity.CurrentNotifBehavior = null;
            }

            this.CardBehaviorOwner.OnBehaviorEnd(this);
        }

        public override void StartNotif(World world)
        {
            this.isActive = true;

            foreach(StarEntity starEntity in this.NodeLevel.BoardGameLayer.StarSystem)
            {
                starEntity.CurrentNotifBehavior = this;
            }

            this.ModifiedCardEntities.Clear();

            this.CardBehaviorOwner.OnBehaviorStart(this);
        }

        public virtual bool CanSocketCardOn(StarEntity starEntity, CardEntity cardToSocket)
        {
            //return this.ToStarEntities.Contains(starEntity);
            return false;
        }

        public ACardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity) 
            : base(ownerCardEntity)
        {
            this.CardBehaviorOwner = cardBehaviorOwner;

            this.NbBehaviorUse = 1;

            this.ModifiedCardEntities = new HashSet<CardEntity>();
        }

        protected virtual void ExecuteBehavior(StarEntity starEntity)
        {
            this.ModifiedCardEntities.Add(starEntity.CardSocketed);
        }
    }
}
