using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public interface ICardBehaviorOwner
    {
        void OnBehaviorStart(ACardNotifBehavior behavior);

        void OnBehaviorCardPicked(ACardNotifBehavior behavior);

        void OnBehaviorEnd(ACardNotifBehavior behavior);
    }
}
