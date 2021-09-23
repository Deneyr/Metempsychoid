using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;

namespace Metempsychoid.Model.Card.Behaviors
{
    public abstract class ACardActiveBehavior: ACardBehavior, ICardBehaviorOwner
    {
        public int MaxActivationNb
        {
            get;
            set;
        }

        public int CurrentActivationNb
        {
            get;
            private set;
        }

        public override bool IsActive
        {
            get
            {
                if(this.MaxActivationNb < 0)
                {
                    return true;
                }
                return this.CurrentActivationNb < this.MaxActivationNb;
            }
        }

        public ACardActiveBehavior()
        {
            this.MaxActivationNb = -1;
            this.CurrentActivationNb = 0;
        }

        protected virtual bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (this.IsActive)
            {
                this.CurrentActivationNb++;

                return true;
            }
            return false;
        }

        protected override void InitFrom(ACardBehavior cardBehaviorModel)
        {
            ACardActiveBehavior cardActiveBehavior = cardBehaviorModel as ACardActiveBehavior;

            this.MaxActivationNb = cardActiveBehavior.MaxActivationNb;
        }

        public virtual void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            
        }

        public virtual void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            
        }

        public virtual void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            
        }
    }
}
