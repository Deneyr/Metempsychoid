using Astrategia.Model.Card;
using Astrategia.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public interface ICardBehaviorOwner
    {
        void OnBehaviorStart(ACardNotifBehavior behavior);

        void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked);

        void OnBehaviorEnd(ACardNotifBehavior behavior);
    }
}
