using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public interface ICardBehavior
    {
        //bool AllowAwakening
        //{
        //    get;
        //}

        //bool AllowUnawakening
        //{
        //    get;
        //}
        bool IsActive
        {
            get;
        }

        void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionOccured);

        void OnAwakened(BoardGameLayer layer, StarEntity starEntity);

        void OnUnawakened(BoardGameLayer layer, CardEntity cardEntity);

        void OnDestroyed(BoardGameLayer layer, CardEntity cardEntity);

        ICardBehavior Clone();
    }
}
