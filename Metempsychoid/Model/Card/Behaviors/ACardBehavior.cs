using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;

namespace Metempsychoid.Model.Card.Behaviors
{
    public abstract class ACardBehavior : ICardBehavior
    {
        //public virtual bool AllowAwakening
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        //public virtual bool AllowUnawakening
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        public virtual void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionOccured)
        {
            
        }

        public virtual void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            
        }

        public virtual void OnDestroyed(BoardGameLayer layer, CardEntity cardEntity)
        {
            
        }

        public virtual void OnUnawakened(BoardGameLayer layer, CardEntity cardEntity)
        {
            
        }

        public abstract ICardBehavior Clone();
    }
}
