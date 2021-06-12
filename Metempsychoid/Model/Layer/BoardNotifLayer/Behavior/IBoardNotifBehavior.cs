using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public interface IBoardNotifBehavior
    {
        CardEntity OwnerCardEntity
        {
            get;
        }

        TestLevel NodeLevel
        {
            get;
            set;
        }

        void StartNotif(World world);

        bool UpdateNotif(World world);

        void EndNotif(World world);

        void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents);
    }
}
